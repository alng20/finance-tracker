import { createContext, useState } from "react";
import { useEffect } from "react";

import { setAccessToken } from "../../api/apiToken";
import * as authApi from "./api/authApi";
import type { LoginResponse } from "./types/Login";

type UserData = {
  firstName: string | null;
  lastName: string | null;
};

type AuthContextValue = {
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

function makeUserData(result: LoginResponse): UserData {
  return {
    firstName: result.firstName,
    lastName: result.lastName,
  };
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [user, setUser] = useState<UserData | null>(null);

  async function login(email: string, password: string) {
    const result = await authApi.login(email, password);

    setAccessToken(result.accessToken);
    setIsAuthenticated(true);
    setUser(makeUserData(result));
  }

  async function refresh() {
    const result = await authApi.refresh();

    setAccessToken(result.accessToken);
    setIsAuthenticated(true);
    setUser(makeUserData(result));
  }

  async function logout() {
    await authApi.logout();

    setAccessToken(null);
    setIsAuthenticated(false);
    setUser(null);
  }

  useEffect(() => {
    refresh().catch(() => {
      setAccessToken(null);
      setIsAuthenticated(false);
      setUser(null);
    });
  }, []);

  return (
    <AuthContext.Provider
      value={{
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
