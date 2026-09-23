import "./css/Dashboard.css";

import MonthAmountCard from "./MonthAmountCard";
import MonthWeeklyAmountCard from "./MonthWeeklyAmount";
import RecentExpenses from "./RecentExpenses";

function Dashboard() {
  return (
    <div className="dashboard">
      <MonthAmountCard />
      <RecentExpenses />
      <MonthWeeklyAmountCard />
    </div>
  );
}

export default Dashboard;
