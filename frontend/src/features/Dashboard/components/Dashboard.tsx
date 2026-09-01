import "./css/Dashboard.css";
import MonthAmountCard from "./MonthAmountCard";
import RecentExpenses from "./RecentExpenses";

function Dashboard() {
  return (
    <div className="dashboard">
      <MonthAmountCard />
      <RecentExpenses />
    </div>
  );
}

export default Dashboard;
