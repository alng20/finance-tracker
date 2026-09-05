import { useState } from "react";
import AddExpenseButton from "./AddExpense/AddExpenseButton";
import "./Expenses.css";
import AddExpenseModal from "./AddExpense/AddExpenseModal";
import ExpensesTable from "./GetExpenses/ExpensesTable";
import useGetExpenses from "../hooks/useGetExpenses";
import type { GetExpenseByIdResponse } from "../types/GetExpenseByIdResponse";
import ExpenseFilters from "./GetExpenses/ExpenseFilters";
import type { ExpenseFiltersData } from "../types/ExpenseFiltersData";
import { initialPageData, type PageData } from "../types/PageData";
import PageSettings from "./GetExpenses/PageSettings";
import { ExpenseResultsSummary } from "./GetExpenses/ExpenseResultsSummary";

function Expenses() {
  const [isAddExpenseOpen, setIsAddExpenseOpen] = useState<boolean>(false);
  const [isExpenseEdited, setIsExpenseEdited] =
    useState<GetExpenseByIdResponse | null>(null);

  const [page, setPage] = useState<PageData>(initialPageData); // TODO: Implement pagination
  const [filters, setFilters] = useState<ExpenseFiltersData | null>(null);
  const { expenses, totalCount, isLoading, error, refresh } = useGetExpenses(
    page,
    filters,
  );

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

      <PageSettings setPage={setPage} />

      {expenses.length > 0 && (
        <ExpenseResultsSummary expenses={expenses} totalCount={totalCount} />
      )}

      <ExpenseFilters onApply={setFilters} />

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
