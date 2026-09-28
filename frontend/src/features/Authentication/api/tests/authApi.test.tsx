import { describe, expect, it, vi } from "vitest";

import { apiClient } from "../../../../api/api";
import type { RegisterRequest } from "../../types/RegisterRequest";
import type { RegisterResponse } from "../../types/RegisterResponse";
import { register } from "../authApi";

vi.mock("../../../../api/api", () => ({
  apiClient: {
    post: vi.fn(),
  },
}));

describe("authApi", () => {
  it("register user OK", async () => {
    const request: RegisterRequest = {
      firstName: "User",
      lastName: "First",
      email: "user@ft.dev",
      password: "user_password",
      phone: "1234567",
    };

    const response: RegisterResponse = {
      id: "user-1",
      firstName: "User",
      lastName: "First",
      email: "user@ft.dev",
      role: "User",
    };

    vi.mocked(apiClient.post).mockResolvedValue(response);

    const result = await register(request);
    expect(apiClient.post).toHaveBeenCalledWith("api/auth/register", request);
    expect(result).toEqual(response);
  });

  it("register user Conflict", async () => {
    const request: RegisterRequest = {
      firstName: "User",
      lastName: "First",
      email: "user@ft.dev",
      password: "user_password",
      phone: "1234567",
    };

    const error = new Error("User already exists");

    vi.mocked(apiClient.post).mockRejectedValue(error);

    await expect(register(request)).rejects.toThrow("User already exists");
    expect(apiClient.post).toHaveBeenCalledWith("api/auth/register", request);
  });
});
