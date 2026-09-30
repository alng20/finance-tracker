import { useEffect, useState } from "react";

import * as reportsApi from "../api/reportsApi";
import type { GetGroupedAmountResponse } from "../types/GetGroupedAmountResponse";
import type { ReportGroupingType } from "../types/GetGroupedAmountResponse";
import type { ReportFiltersData } from "../types/ReportFiltersData";

function useGroupedAmount(
  filters: ReportFiltersData,
  groupingType: ReportGroupingType,
) {
  const [groupedAmount, setGroupedAmount] =
    useState<GetGroupedAmountResponse | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    const controller = new AbortController();

    reportsApi
      .getGroupedAmount(
        filters.currency,
        groupingType,
        filters.fromDate,
        filters.toDate,
        { signal: controller.signal },
      )
      .then((response) => {
        if (controller.signal.aborted) {
          return;
        }
        setGroupedAmount(response);
        setIsLoading(false);
      })
      .catch((err) => {
        if (err.name === "AbortError" || controller.signal.aborted) {
          return;
        }
        setError(err);
        setIsLoading(false);
      });

    return () => {
      controller.abort();
    };
  }, [filters.currency, filters.fromDate, filters.toDate, groupingType]);

  return {
    groupedAmount,
    isLoading,
    error,
  };
}

export default useGroupedAmount;
