import "../css/ApplicationLayout.css";

import { Outlet } from "react-router-dom";

import Header from "../../features/Header/Header";
import Sidebar from "../../features/shared/components/Sidebar/Sidebar";

function ApplicationLayout() {
  return (
    <div className="application-layout">
      <Header title="Finance Tracker" />

      <Sidebar />

      <main className="application-layout__main">
        <Outlet />
      </main>
    </div>
  );
}

export default ApplicationLayout;
