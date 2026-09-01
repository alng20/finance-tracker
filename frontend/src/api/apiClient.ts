import { getAccessToken, setAccessToken } from "./apiToken";

// TODO: Move to env
const API_URL = "http://localhost:5067";

function buildUrl(url: string): string {
  return `${API_URL.replace(/\/$/, "")}/${url.replace(/^\//, "")}`;
}

type ApiClient = {
  get: <T>(url: string, options?: RequestInit) => Promise<T>;
  post: <T>(url: string, body?: unknown, options?: RequestInit) => Promise<T>;
  put: <T>(url: string, body?: unknown, options?: RequestInit) => Promise<T>;
  delete: <T>(url: string, options?: RequestInit) => Promise<T>;
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

  async function parseResponse<T>(response: Response): Promise<T> {
    if (response.status === 204) {
      return undefined as T;
    }

    return response.json() as Promise<T>;
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

  function checkResponseStatus(response: Response): void {
    if (!response.ok) {
      throw new Error(`Request failed with status ${response.status}`);
    }
  }

  return {
    get: async <T>(url: string, options?: RequestInit): Promise<T> => {
      const response = await request("GET", url, undefined, options);
      checkResponseStatus(response);
      return parseResponse<T>(response);
    },
    post: async <T>(
      url: string,
      body?: unknown,
      options?: RequestInit,
    ): Promise<T> => {
      const response = await request("POST", url, body, options);
      checkResponseStatus(response);
      return parseResponse<T>(response);
    },
    put: async <T>(
      url: string,
      body?: unknown,
      options?: RequestInit,
    ): Promise<T> => {
      const response = await request("PUT", url, body, options);
      checkResponseStatus(response);
      return parseResponse<T>(response);
    },
    delete: async <T>(url: string, options?: RequestInit): Promise<T> => {
      const response = await request("DELETE", url, undefined, options);
      checkResponseStatus(response);
      return parseResponse<T>(response);
    },
  };
}
