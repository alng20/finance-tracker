import Dashboard from "../components/Dashboard";

function DashboardPage() {
  return (
    <div>
      <div className="page-heading">
        <h1>My dashboard</h1>
        <p>Monthly spending activity</p>
      </div>
      <Dashboard />
    </div>
  );
}

export default DashboardPage;
