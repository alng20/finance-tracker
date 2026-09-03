import { createContext, useState } from "react";
import { useEffect } from "react";

import { setAccessToken } from "../../api/apiToken";
import * as authApi from "./api/authApi";
import type { GetProfileResponse, UserRole } from "./types/GetProfileResponse";

type AuthState = "Authenticated" | "Unauthenticated" | "Unknown";

type UserData = {
  firstName: string | null;
  lastName: string | null;
  role: UserRole;
};

type AuthContextValue = {
  authState: AuthState;
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<void>;
  refresh: () => Promise<void>;
  logout: () => Promise<void>;
  user: UserData | null;
};

export const AuthContext = createContext<AuthContextValue | null>(null);

type AuthProviderProps = {
  children: React.ReactNode;
};

function makeUserData(result: GetProfileResponse): UserData {
  return {
    firstName: result.firstName,
    lastName: result.lastName,
    role: result.role as UserRole,
  };
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [authState, setAuthState] = useState<AuthState>("Unknown");
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [user, setUser] = useState<UserData | null>(null);

  async function login(email: string, password: string) {
    const result = await authApi.login(email, password);
    setAccessToken(result.accessToken);

    await profile();
  }

  async function refresh() {
    const result = await authApi.refresh();
    setAccessToken(result.accessToken);

    await profile();
  }

  async function logout() {
    await authApi.logout();

    setAccessToken(null);
    setIsAuthenticated(false);
    setAuthState("Unauthenticated");
    setUser(null);
  }

  async function profile() {
    const result = await authApi.profile();
    setIsAuthenticated(true);
    setAuthState("Authenticated");
    setUser(makeUserData(result));
  }

  useEffect(() => {
    refresh().catch(() => {
      setAccessToken(null);
      setIsAuthenticated(false);
      setAuthState("Unknown");
      setUser(null);
    });
  }, []);

  return (
    <AuthContext.Provider
      value={{
        authState,
        isAuthenticated,
        login,
        refresh,
        logout,
        user,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
