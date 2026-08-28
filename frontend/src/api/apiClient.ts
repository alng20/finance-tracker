import { getAccessToken, setAccessToken } from "./apiToken";

const API_URL = "http://localhost:5067";

function buildUrl(url: string): string {
  return `${API_URL.replace(/\/$/, "")}/${url.replace(/^\//, "")}`;
}

type ApiClient = {
  get: (url: string, options?: RequestInit) => Promise<Response>;
  post: (
    url: string,
    body?: unknown,
    options?: RequestInit,
  ) => Promise<Response>;
  put: (
    url: string,
    body?: unknown,
    options?: RequestInit,
  ) => Promise<Response>;
  delete: (url: string, options?: RequestInit) => Promise<Response>;
};

export function createApiClient(): ApiClient {
  let refreshPromise: Promise<boolean> | null = null;

  async function send(
    method: string,
    url: string,
    body?: unknown,
    options: RequestInit = {},
  ): Promise<Response> {
    const accessToken = getAccessToken();

    const headers = new Headers(options.headers);

    if (accessToken) {
      headers.set("Authorization", `Bearer ${accessToken}`);
    }

    if (body !== undefined) {
      headers.set("Content-Type", "application/json");
    }

    return fetch(buildUrl(url), {
      ...options,
      method,
      headers,
      credentials: "include",
      body: body !== undefined ? JSON.stringify(body) : undefined,
    });
  }

  async function refreshAccessToken(): Promise<boolean> {
    const response = await fetch(`${API_URL}/api/auth/refresh`, {
      method: "POST",
      credentials: "include",
    });

    if (!response.ok) {
      return false;
    }

    const data = await response.json();

    setAccessToken(data.accessToken);

    return true;
  }

  async function request(
    method: string,
    url: string,
    body?: unknown,
    options: RequestInit = {},
  ): Promise<Response> {
    const response = await send(method, url, body, options);
    if (response.status !== 401) {
      return response;
    }

    if (refreshPromise === null) {
      refreshPromise = refreshAccessToken();
    }

    const currentRefreshRef = refreshPromise;
    let refreshResult: boolean;

    try {
      refreshResult = await currentRefreshRef;
    } finally {
      if (refreshPromise === currentRefreshRef) {
        refreshPromise = null;
      }
    }

    if (!refreshResult) {
      return response;
    }

    return send(method, url, body, options);
  }

  return {
    get: (url, options) => request("GET", url, undefined, options),
    post: (url, body, options) => request("POST", url, body, options),
    put: (url, body, options) => request("PUT", url, body, options),
    delete: (url, options) => request("DELETE", url, undefined, options),
  };
}
