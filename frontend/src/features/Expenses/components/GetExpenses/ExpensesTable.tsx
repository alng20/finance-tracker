import "./css/ExpensesTable.css";

import type { GetExpenseByIdResponse } from "../../types/GetExpenseByIdResponse";
import type { ExpenseDto } from "../../types/GetExpensesResponse";
import ExpenseRow from "./ExpenseRow";

export type ExpensesTableProps = {
  expenses: ExpenseDto[];
  isLoading: boolean;
  error: Error | null;
  onEdit: (expense: GetExpenseByIdResponse) => void;
  onChanged: () => void;
};

function ExpensesTable(props: ExpensesTableProps) {
  if (props.isLoading) {
    return <div className="expenses-table">Loading...</div>;
  }

  if (props.error) {
    return <div className="expenses-table">Failed to load expenses.</div>;
  }

  if (props.expenses.length === 0) {
    return (
      <div className="expenses-table">You don't have any expenses yet.</div>
    );
  }

  return (
    <div className="expenses-table">
      <div className="expenses-table__data">
        {props.expenses.length === 0 ? (
          <p className="expenses-table__no_expenses">
            You don't have any expenses yet.
          </p>
        ) : (
          props.expenses.map((expense) => (
            <ExpenseRow
              key={expense.id}
              expense={expense}
              onEdit={props.onEdit}
              onChanged={props.onChanged}
            />
          ))
        )}
      </div>
    </div>
  );
}

export default ExpensesTable;
