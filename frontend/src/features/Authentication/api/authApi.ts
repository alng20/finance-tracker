import { apiClient } from "../../../api/api";
import type { GetProfileResponse } from "../types/GetProfileResponse";
import type { LoginRequest } from "../types/LoginRequest";
import type { LoginResponse } from "../types/LoginResponse";
import type { RegisterRequest } from "../types/RegisterRequest";
import type { RegisterResponse } from "../types/RegisterResponse";

const API_AUTH = "api/auth";

export async function login(request: LoginRequest): Promise<LoginResponse> {
  return apiClient.post<LoginResponse>(`${API_AUTH}/login`, request);
}

export async function register(
  request: RegisterRequest,
): Promise<RegisterResponse> {
  return apiClient.post<RegisterResponse>(`${API_AUTH}/register`, request);
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
