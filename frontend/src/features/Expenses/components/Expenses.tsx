import "./Expenses.css";

import { useState } from "react";

import Pagination from "../../shared/components/Pagination/Pagintation";
import useGetWithFilters from "../../shared/hooks/useGetWithFilters";
import {
  initialPageRequest,
  type PageRequestData,
} from "../../shared/types/PageRequestData";
import { getExpenses } from "../api/expensesApi";
import type { ExpenseFiltersData } from "../types/ExpenseFiltersData";
import type { GetExpenseByIdResponse } from "../types/GetExpenseByIdResponse";
import {
  type ExpenseDto,
  type ExpenseMetadata,
  type GetExpensesResponse,
} from "../types/GetExpensesResponse";
import AddExpenseButton from "./AddExpense/AddExpenseButton";
import AddExpenseModal from "./AddExpense/AddExpenseModal";
import ExpenseFilters from "./GetExpenses/ExpenseFilters";
import { ExpenseResultsSummary } from "./GetExpenses/ExpenseResultsSummary";
import ExpensesTable from "./GetExpenses/ExpensesTable";

function Expenses() {
  const [isAddExpenseOpen, setIsAddExpenseOpen] = useState<boolean>(false);
  const [isExpenseEdited, setIsExpenseEdited] =
    useState<GetExpenseByIdResponse | null>(null);

  const [page, setPage] = useState<PageRequestData>(initialPageRequest);
  const [filters, setFilters] = useState<ExpenseFiltersData | null>(null);
  const {
    results: expenses,
    metadata: expensesMetadata,
    pagination,
    isLoading,
    error,
    refresh,
  } = useGetWithFilters<
    GetExpensesResponse,
    ExpenseFiltersData,
    ExpenseDto,
    ExpenseMetadata
  >(page, filters, getExpenses);

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
          expensesMetadata={expensesMetadata}
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
