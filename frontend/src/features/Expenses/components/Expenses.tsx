import { useState } from "react";
import AddExpenseButton from "./AddExpense/AddExpenseButton";
import "./Expenses.css";
import AddExpenseModal from "./AddExpense/AddExpenseModal";
import ExpensesTable from "./GetExpenses/ExpensesTable";
import useGetExpenses from "../hooks/useGetExpenses";
import type { GetExpenseByIdResponse } from "../types/GetExpenseByIdResponse";
import ExpenseFilters from "./GetExpenses/ExpenseFilters";
import type { ExpenseFiltersData } from "../types/ExpenseFiltersData";
import {
  initialPageRequest,
  type PageRequestData,
} from "../types/PageRequestData";
import { ExpenseResultsSummary } from "./GetExpenses/ExpenseResultsSummary";
import Pagination from "./GetExpenses/Pagintation";

function Expenses() {
  const [isAddExpenseOpen, setIsAddExpenseOpen] = useState<boolean>(false);
  const [isExpenseEdited, setIsExpenseEdited] =
    useState<GetExpenseByIdResponse | null>(null);

  const [page, setPage] = useState<PageRequestData>(initialPageRequest); // TODO: Implement pagination
  const [filters, setFilters] = useState<ExpenseFiltersData | null>(null);
  const { expenses, pagination, isLoading, error, refresh } = useGetExpenses(
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

      {expenses.length > 0 && (
        <ExpenseResultsSummary
          expenses={expenses}
          totalCount={pagination.dataTotalCount}
        />
      )}

      <ExpenseFilters onApply={setFilters} />

      <Pagination
        pageNumber={page.pageNumber}
        pagesTotalCount={pagination.pagesTotalCount}
        hasNextPage={pagination.hasNextPage}
        hasPreviousPage={pagination.hasPreviousPage}
        onPageChange={(newPage) =>
          setPage((current) => ({
            ...current,
            pageNumber: newPage,
          }))
        }
      />

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
