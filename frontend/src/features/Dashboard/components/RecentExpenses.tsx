import "./css/RecentExpenses.css";

import useRecentExpenses from "../hooks/useRecentExpenses";

function RecentExpenses() {
  const { recentExpenses, isLoading, error } = useRecentExpenses();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Failed to load dashboard.</div>;
  }
  return (
    <section className="recent_expenses">
      <h2 className="recent_expenses__header">Recent Expenses</h2>

      <div className="recent_expenses__expenses">
        <div className="recent_expenses__data">
          {recentExpenses?.map((expense) => (
            <div className="recent_expenses__data_row" key={expense.id}>
              {expense.expenseDate}: {expense.shopName}
              {" — "}
              {expense.amountFormatted}
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

export default RecentExpenses;
