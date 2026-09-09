import { useEffect, useState } from "react";

import * as expensesApi from "../../Expenses/api/expensesApi";
import type { GetExpensesResponse } from "../../Expenses/types/GetExpensesResponse";
import {
  mapRecentExpense,
  type RecentExpenseData,
} from "../components/data/RecentExpenseData";
import type { PageRequestData } from "../../shared/types/PageRequestData";

function useRecentExpenses() {
  const [recentExpenses, setRecentExpenses] = useState<RecentExpenseData[]>([]);
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

    const page: PageRequestData = {
      pageNumber: 1,
      pageSize: 5,
    };

    expensesApi
      .getExpenses(page)
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
