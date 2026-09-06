import { useCallback, useEffect, useState } from "react";

import type { ExpenseDto } from "../types/GetExpensesResponse";
import * as expensesApi from "../api/expensesApi";
import type { ExpenseFiltersData } from "../types/ExpenseFiltersData";
import type { PageRequestData } from "../types/PageRequestData";
import type { PaginationData } from "../types/PaginationData";

function useGetExpenses(
  page: PageRequestData,
  filters: ExpenseFiltersData | null,
) {
  const [expenses, setExpenses] = useState<ExpenseDto[]>([]);
  const [pagination, setPagination] = useState<PaginationData>({
    pagesTotalCount: 0,
    dataTotalCount: 0,
    hasNextPage: false,
    hasPreviousPage: false,
  });
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  const refresh = useCallback(async () => {
    setIsLoading(true);
    setError(null);

    expensesApi
      .getExpenses(page, filters)
      .then((response) => {
        setExpenses(response.data);
        setPagination({
          pagesTotalCount: response.totalPages,
          dataTotalCount: response.totalCount,
          hasNextPage: response.hasNextPage,
          hasPreviousPage: response.hasPreviousPage,
        });
      })
      .catch(setError)
      .finally(() => setIsLoading(false));
  }, [filters, page]);

  useEffect(() => {
    refresh();
  }, [refresh]);

  return {
    expenses,
    pagination,
    isLoading,
    error,
    refresh,
  };
}

export default useGetExpenses;
