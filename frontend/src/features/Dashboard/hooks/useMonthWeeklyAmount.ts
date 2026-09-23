import { useEffect, useState } from "react";

import * as reportsApi from "../../Reports/api/reportsApi";
import type { GetGroupedAmountResponse } from "../../Reports/types/GetGroupedAmountResponse";
import { defaultCurrency } from "../../shared/common/consts";
import {
  getFirstDayOfCurrentMonth,
  getLastDayOfCurrentMonth,
} from "../../shared/common/utils";
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

  useEffect(() => {
    const groupingType = "Week";

    function setResult(response: GetGroupedAmountResponse) {
      setMonthWeeklyAmount(
        response.amountByPeriod.map((amount) => mapWeekAmount(amount)),
      );
      setIsLoading(false);
    }

    reportsApi
      .getGroupedAmount(
        defaultCurrency,
        groupingType,
        getFirstDayOfCurrentMonth(),
        getLastDayOfCurrentMonth(),
      )
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
