import { useEffect, useState } from "react";

import * as expensesApi from "../../Expenses/api/expensesApi";
import type { GetExpensesResponse } from "../../Expenses/types/GetExpensesResponse";
import type { PageRequestData } from "../../shared/types/PageRequestData";
import {
  mapRecentExpense,
  type RecentExpenseData,
} from "../components/data/RecentExpenseData";

function useRecentExpenses() {
  const [recentExpenses, setRecentExpenses] = useState<RecentExpenseData[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    const page: PageRequestData = {
      pageNumber: 1,
      pageSize: 5,
    };

    function setResult(response: GetExpensesResponse) {
      setRecentExpenses(
        response.data.map((expense) => mapRecentExpense(expense)),
      );
      setIsLoading(false);
    }

    expensesApi
      .getExpenses(page)
      .then(setResult)
      .catch((err) => {
        setError(err);
        setIsLoading(false);
      });
  }, []);

  return {
    recentExpenses,
    isLoading,
    error,
  };
}

export default useRecentExpenses;
