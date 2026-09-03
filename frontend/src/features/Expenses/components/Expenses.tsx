import { useState } from "react";
import AddExpenseButton from "./AddExpense/AddExpenseButton";
import "./Expenses.css";
import AddExpenseModal from "./AddExpense/AddExpenseModal";
import ExpensesTable from "./GetExpenses/ExpensesTable";
import useGetExpenses from "../hooks/useGetExpenses";
import type { GetExpenseByIdResponse } from "../types/GetExpenseByIdResponse";

function Expenses() {
  const [isAddExpenseOpen, setIsAddExpenseOpen] = useState<boolean>(false);
  const [isExpenseEdited, setIsExpenseEdited] =
    useState<GetExpenseByIdResponse | null>(null);
  const { expenses, isLoading, error, refresh } = useGetExpenses();

  return (
    <div className="expenses">
      <div className="expenses_page__header">
        <div>
          <p className="expenses_page__eyebrow">Spending overview</p>
          <h1 className="expenses_page__header_title">My Expenses</h1>
          <p className="expenses_page__header_description">
            Control the spendings
          </p>
        </div>
        <AddExpenseButton
          onClick={() => setIsAddExpenseOpen(true)}
          text="Add Expense"
        />
      </div>
      {(isAddExpenseOpen || isExpenseEdited) && (
        <AddExpenseModal
          onClose={() => {
            setIsAddExpenseOpen(false);
            setIsExpenseEdited(null);
          }}
          onCreated={async () => {
            await refresh();
            setIsAddExpenseOpen(false);
            setIsExpenseEdited(null);
          }}
          initialExpense={isExpenseEdited ?? undefined}
        />
      )}

      <ExpensesTable
        expenses={expenses}
        isLoading={isLoading}
        error={error}
        onEdit={(expense) => setIsExpenseEdited(expense)}
        onChanged={refresh}
      />
    </div>
  );
}

export default Expenses;
