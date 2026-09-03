import { Navigate, Outlet } from "react-router-dom";

import { useAuth } from "../../features/Authentication/hooks/useAuth";

function PublicRoute() {
  const { authState } = useAuth();

  if (authState === "Unknown") {
    return null;
  }

  if (authState === "Authenticated") {
    return <Navigate to="/dashboard" replace />;
  }

  return <Outlet />;
}

export default PublicRoute;
