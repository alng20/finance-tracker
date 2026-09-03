import { apiClient } from "../../../api/api";
import type { GetProfileResponse } from "../types/GetProfileResponse";
import type { LoginResponse } from "../types/Login";

const API_AUTH = "api/auth";

export async function login(
  email: string,
  password: string,
): Promise<LoginResponse> {
  return apiClient.post<LoginResponse>(`${API_AUTH}/login`, {
    email,
    password,
  });
}

export async function refresh(): Promise<LoginResponse> {
  return apiClient.post<LoginResponse>(`${API_AUTH}/refresh`);
}

export async function logout(): Promise<void> {
  return apiClient.post(`${API_AUTH}/logout`);
}

export async function profile(): Promise<GetProfileResponse> {
  return apiClient.get<GetProfileResponse>(`/api/profile`);
}
