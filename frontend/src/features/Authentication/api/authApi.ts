import { apiClient } from "../../../api/api";
import type { LoginResponse } from "../types/Login";

const API_AUTH = "api/auth";

export async function login(
  email: string,
  password: string,
): Promise<LoginResponse> {
  const response = await apiClient.post(`${API_AUTH}/login`, {
    email,
    password,
  });

  if (!response.ok) {
    throw new Error("Invalid credentials");
  }

  return response.json();
}

export async function refresh(): Promise<LoginResponse> {
  const response = await apiClient.post(`${API_AUTH}/refresh`);

  if (!response.ok) {
    throw new Error("Failed to refresh authentication");
  }

  return response.json();
}

export async function logout(): Promise<void> {
  const response = await apiClient.post(`${API_AUTH}/logout`);

  if (!response.ok) {
    throw new Error("Logout failed");
  }
}
