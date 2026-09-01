import { useAuth } from "../../../Authentication/hooks/useAuth";
import "./css/Sidebar.css";

import { useNavigate } from "react-router-dom";
import SidebarNavLink from "./SidebarNavLink";

function Sidebar() {
  const { logout } = useAuth();

  const navigate = useNavigate();
  async function onClickLogout() {
    await logout();
    navigate("/login");
  }

  return (
    <aside className="sidebar">
      <nav className="sidebar__navigation">
        <SidebarNavLink toLink="/dashboard" children="Dashboard" />
        <SidebarNavLink toLink="/expenses" children="Expenses" />
        <SidebarNavLink toLink="/reports" children="Reports" />
        <SidebarNavLink toLink="/group" children="Groups" />
        <SidebarNavLink toLink="/settings" children="Settings" />
      </nav>
      <button
        className="sidebar__button_logout"
        type="button"
        onClick={onClickLogout}
      >
        Logout
      </button>
    </aside>
  );
}

export default Sidebar;
