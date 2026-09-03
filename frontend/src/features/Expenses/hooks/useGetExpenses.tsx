import { useCallback, useEffect, useState } from "react";

import type { ExpenseDto } from "../types/GetExpensesResponse";
import * as expensesApi from "../api/expensesApi";

function useGetExpenses() {
  const [expenses, setExpenses] = useState<ExpenseDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  const refresh = useCallback(async () => {
    setIsLoading(true);
    setError(null);

    expensesApi
      .getExpenses()
      .then((response) => {
        setExpenses(response.data);
      })
      .catch(setError)
      .finally(() => setIsLoading(false));
  }, []);

  useEffect(() => {
    refresh();
  }, [refresh]);

  return {
    expenses,
    isLoading,
    error,
    refresh,
  };
}

export default useGetExpenses;
