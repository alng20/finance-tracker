import { useCallback, useEffect, useState } from "react";

import { setAccessToken } from "../../api/apiToken";
import * as authApi from "./api/authApi";
import { AuthContext, type AuthState, type UserData } from "./AuthContext";
import type { GetProfileResponse, UserRole } from "./types/GetProfileResponse";
import type { LoginRequest } from "./types/LoginRequest";

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

  const profile = useCallback(async () => {
    const result = await authApi.profile();
    setIsAuthenticated(true);
    setAuthState("Authenticated");
    setUser(makeUserData(result));
  }, []);

  const refresh = useCallback(async () => {
    const result = await authApi.refresh();
    setAccessToken(result.accessToken);
    await profile();
  }, [profile]);

  const login = useCallback(
    async (request: LoginRequest) => {
      const result = await authApi.login(request);
      setAccessToken(result.accessToken);
      await profile();
    },
    [profile],
  );

  const logout = useCallback(async () => {
    await authApi.logout();
    setAccessToken(null);
    setIsAuthenticated(false);
    setAuthState("Unauthenticated");
    setUser(null);
  }, []);

  useEffect(() => {
    const timer = setTimeout(() => {
      refresh().catch(() => {
        setAccessToken(null);
        setIsAuthenticated(false);
        setAuthState("Unauthenticated");
        setUser(null);
      });
    }, 0);
    return () => clearTimeout(timer);
  }, [refresh]);

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
