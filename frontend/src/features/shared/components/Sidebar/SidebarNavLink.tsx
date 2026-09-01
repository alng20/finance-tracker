import "./css/SidebarNavLink.css";

import { NavLink } from "react-router-dom";

type SidebarNavLinkProps = {
  toLink: string;
  children: React.ReactNode;
};

function SidebarNavLink({ toLink, children }: SidebarNavLinkProps) {
  return (
    <NavLink
      to={toLink}
      className={({ isActive }) =>
        isActive
          ? "sidebar__navlink sidebar__navlink_active"
          : "sidebar__navlink"
      }
    >
      <div>{children}</div>
    </NavLink>
  );
}

export default SidebarNavLink;
