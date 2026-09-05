import { useCallback, useEffect, useState } from "react";

import type { ExpenseDto } from "../types/GetExpensesResponse";
import * as expensesApi from "../api/expensesApi";
import type { ExpenseFiltersData } from "../types/ExpenseFiltersData";
import type { PageData } from "../types/PageData";

function useGetExpenses(page: PageData, filters: ExpenseFiltersData | null) {
  const [expenses, setExpenses] = useState<ExpenseDto[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  const refresh = useCallback(async () => {
    setIsLoading(true);
    setError(null);

    expensesApi
      .getExpenses(page, filters)
      .then((response) => {
        setExpenses(response.data);
        setTotalCount(response.totalCount);
      })
      .catch(setError)
      .finally(() => setIsLoading(false));
  }, [filters, page]);

  useEffect(() => {
    refresh();
  }, [refresh]);

  return {
    expenses,
    totalCount,
    isLoading,
    error,
    refresh,
  };
}

export default useGetExpenses;
