import { createContext } from "react";

import type { UserRole } from "./types/GetProfileResponse";
import type { LoginRequest } from "./types/LoginRequest";

export type AuthState = "Authenticated" | "Unauthenticated" | "Unknown";

export type UserData = {
  firstName: string | null;
  lastName: string | null;
  role: UserRole;
};

type AuthContextValue = {
  authState: AuthState;
  isAuthenticated: boolean;
  login: (request: LoginRequest) => Promise<void>;
  refresh: () => Promise<void>;
  logout: () => Promise<void>;
  user: UserData | null;
};

export const AuthContext = createContext<AuthContextValue | null>(null);
