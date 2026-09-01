import "../css/ApplicationLayout.css";

import { Outlet } from "react-router-dom";
import Sidebar from "../../features/shared/components/Sidebar/Sidebar";
import Header from "../../features/Expenses/components/Header/Header";

function ApplicationLayout() {
  return (
    <div className="application-layout">
      <Header
        title="Finance Tracker"
        date={new Date().toLocaleDateString("en-GB")}
      />

      <Sidebar />

      <main className="application-layout__main">
        <Outlet />
      </main>
    </div>
  );
}

export default ApplicationLayout;
