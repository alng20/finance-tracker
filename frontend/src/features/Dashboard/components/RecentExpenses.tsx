import "./css/RecentExpenses.css";

import useRecentExpenses from "../hooks/useRecentExpenses";
import RecentExpenseRow from "./RecentExpensesRow";

function RecentExpenses() {
  const { recentExpenses: expenses, isLoading, error } = useRecentExpenses();

  if (isLoading) {
    return (
      <section className="recent_expenses recent_expenses--state">
        <p className="recent_expenses__eyebrow">Your activity</p>
        <h2 className="recent_expenses__header">Recent Expenses</h2>
        <p className="recent_expenses__state">Loading recent activity...</p>
      </section>
    );
  }

  if (error) {
    return (
      <section className="recent_expenses recent_expenses--state">
        <p className="recent_expenses__eyebrow">Your activity</p>
        <h2 className="recent_expenses__header">Recent Expenses</h2>
        <p className="recent_expenses__state">
          Failed to load recent expenses.
        </p>
      </section>
    );
  }
  return (
    <section className="recent_expenses">
      <p className="recent_expenses__eyebrow">Your activity</p>
      <h2 className="recent_expenses__header">Recent Expenses</h2>

      {expenses.length === 0 ? (
        <p className="recent_expenses__state">No recent expenses.</p>
      ) : (
        <div className="recent_expenses__expenses">
          {expenses.map((expense) => (
            <RecentExpenseRow key={expense.id} expense={expense} />
          ))}
        </div>
      )}
    </section>
  );
}

export default RecentExpenses;
