import { Route, Routes } from "react-router-dom";

import LoginPage from "../../features/Authentication/pages/LoginPage";
import RegisterPage from "../../features/Authentication/pages/RegisterPage";
import DashboardPage from "../../features/Dashboard/pages/DashboardPage";
import ExpensesPage from "../../features/Expenses/pages/ExpensesPage";
import GroupPage from "../../features/Groups/pages/GroupsPage";
import HomePage from "../../features/Home/pages/HomePage";
import ItemsPage from "../../features/Items/pages/ItemsPage";
import ReportsPage from "../../features/Reports/pages/ReportsPage";
import SettingsPage from "../../features/Settings/pages/SettingsPage";
import ShopsPage from "../../features/Shops/pages/ShopsPage";
import ApplicationLayout from "../layouts/ApplicationLayout";
import AdminRoute from "./AdminRoute";
import ProtectedRoute from "./ProtectedRoutes";
import PublicRoute from "./PublicRoute";

function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route element={<PublicRoute />}>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
      </Route>

      <Route element={<ProtectedRoute />}>
        <Route element={<ApplicationLayout />}>
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/expenses" element={<ExpensesPage />} />
          <Route path="/reports" element={<ReportsPage />} />
          <Route path="/group" element={<GroupPage />} />
          <Route path="/settings" element={<SettingsPage />} />
          <Route element={<AdminRoute />}>
            <Route path="/admin/shops" element={<ShopsPage />} />
            <Route path="/admin/items" element={<ItemsPage />} />
          </Route>
        </Route>
      </Route>
    </Routes>
  );
}

export default AppRoutes;
