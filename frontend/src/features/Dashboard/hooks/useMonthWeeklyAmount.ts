import { useEffect, useState } from "react";

import * as reportsApi from "../../Reports/api/reportsApi";
import type { GetGroupedAmountResponse } from "../../Reports/types/GetGroupedAmountResponse";
import { defaultCurrency } from "../../shared/common/consts";
import {
  mapWeekAmount,
  type WeekAmountData,
} from "../components/data/WeekAmountData";

function useMonthWeeklyAmount() {
  const [monthWeeklyAmount, setMonthWeeklyAmount] = useState<WeekAmountData[]>(
    [],
  );
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  function getDatePeriod() {
    const end = new Date();

    const start = new Date();
    start.setDate(end.getDate() - 27);

    function format(date: Date) {
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, "0");
      const dayOfMonth = String(date.getDate()).padStart(2, "0");

      return `${year}-${month}-${dayOfMonth}`;
    }
    return [format(start), format(end)];
  }

  useEffect(() => {
    const groupingType = "Week";

    function setResult(response: GetGroupedAmountResponse) {
      setMonthWeeklyAmount(
        response.amountByPeriod.map((amount) => mapWeekAmount(amount)),
      );
      setIsLoading(false);
    }

    reportsApi
      .getGroupedAmount(defaultCurrency, groupingType, ...getDatePeriod())
      .then(setResult)
      .catch((err) => {
        setError(err);
        setIsLoading(false);
      });
  }, []);

  return {
    monthWeeklyAmount,
    isLoading,
    error,
  };
}

export default useMonthWeeklyAmount;
