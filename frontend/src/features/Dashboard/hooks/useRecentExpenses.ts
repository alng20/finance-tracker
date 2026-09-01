import { useEffect, useState } from "react";

import * as expensesApi from "../../Expenses/api/expensesApi";
import type { GetExpensesResponse } from "../../Expenses/types/GetExpensesResponse";
import {
  mapRecentExpense,
  type RecentExpense,
} from "../components/data/RecentExpense";

function useRecentExpenses() {
  const [recentExpenses, setRecentExpenses] = useState<RecentExpense[] | null>(
    null,
  );
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  function setResult(response: GetExpensesResponse) {
    setRecentExpenses(
      response.data.map((expense) => mapRecentExpense(expense)),
    );
  }

  useEffect(() => {
    setIsLoading(true);
    setError(null);

    const page: number = 1;
    const pageSize: number = 5;

    expensesApi
      .getExpenses(page, pageSize)
      .then(setResult)
      .catch(setError)
      .finally(() => setIsLoading(false));
  }, []);

  return {
    recentExpenses,
    isLoading,
    error,
  };
}

export default useRecentExpenses;
