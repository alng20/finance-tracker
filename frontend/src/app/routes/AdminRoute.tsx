import { Navigate, Outlet } from "react-router-dom";

import { useAuth } from "../../features/Authentication/hooks/useAuth";

function AdminRoute() {
  const { user, authState } = useAuth();

  if (authState === "Unknown") {
    return null;
  }

  if (user?.role !== "Admin") {
    return <Navigate to="/dashboard" replace />;
  }

  return <Outlet />;
}

export default AdminRoute;
