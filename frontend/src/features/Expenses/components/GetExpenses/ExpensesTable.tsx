import "./css/ExpensesTable.css";
import { useEffect, useState } from "react";
import { getExpenses } from "../../api/expensesApi";
import type { ExpenseDto } from "../../types/GetExpensesResponse";
import ExpenseRow from "./ExpenseRow";

export type ExpensesTableProps = {
  isAddExpenseOpen: boolean;
};

function ExpensesTable(props: ExpensesTableProps) {
  const [expenses, setExpenses] = useState<ExpenseDto[]>([]);
  useEffect(() => {
    async function load() {
      try {
        const data = await getExpenses();
        setExpenses(data.data);
      } catch (error) {
        console.error("Failed to get expenses:", error);
      }
    }
    load();
  }, [props.isAddExpenseOpen]);

  return (
    <div className="expenses-table">
      {expenses.length > 0 && (
        <div className="expenses-table__data">
          {expenses.map((expense) => (
            <ExpenseRow expense={expense} />
          ))}
        </div>
      )}
      {expenses.length == 0 && (
        <div className="expenses-table__data">
          You don't have any expenses yet.
        </div>
      )}
    </div>
  );
}

export default ExpensesTable;
