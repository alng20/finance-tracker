import { useState } from "react";
import AddExpenseButton from "./AddExpense/AddExpenseButton";
import "./Expenses.css";
import AddExpenseModal from "./AddExpense/AddExpenseModal";
import ExpensesTable from "./GetExpenses/ExpensesTable";

function Expenses() {
  const [isAddExpenseOpen, setIsAddExpenseOpen] = useState<boolean>(false);
  return (
    <div className="expenses">
      <div className="expenses_page__header">
        <h1 className="expenses_page__header_title">My Expenses</h1>
        <AddExpenseButton
          onClick={() => setIsAddExpenseOpen(true)}
          text="Add Expense"
        />
      </div>
      {isAddExpenseOpen && (
        <AddExpenseModal onClose={() => setIsAddExpenseOpen(false)} />
      )}

      <ExpensesTable isAddExpenseOpen={isAddExpenseOpen} />
    </div>
  );
}

export default Expenses;
