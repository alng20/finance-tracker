import { Navigate, Outlet } from "react-router-dom";

import { useAuth } from "../../features/Authentication/hooks/useAuth";

function ProtectedRoute() {
  const { authState } = useAuth();

  if (authState === "Unknown") {
    return null;
  }

  if (authState === "Unauthenticated") {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
}

export default ProtectedRoute;
