import { useEffect, useState } from "react";

import type { Currency } from "../../shared/types/Currency";
import type { ReportFiltersData } from "../types/ReportFiltersData";

type ReportAmountGetter<T> = (
  currency: Currency,
  fromDate: string | undefined,
  toDate: string | undefined,
  reqInit: RequestInit,
) => Promise<T>;

function useReportAmount<T>(
  filters: ReportFiltersData,
  getter: ReportAmountGetter<T>,
) {
  const [result, setResult] = useState<T | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    const controller = new AbortController();

    getter(filters.currency, filters.fromDate, filters.toDate, {
      signal: controller.signal,
    })
      .then((response) => {
        if (controller.signal.aborted) {
          return;
        }
        setResult(response);
        setError(null);
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
  }, [filters.currency, filters.fromDate, filters.toDate, getter]);

  return {
    result,
    isLoading,
    error,
  };
}

export default useReportAmount;
