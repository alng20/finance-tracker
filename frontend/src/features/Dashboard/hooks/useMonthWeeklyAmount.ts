import { useEffect, useState } from "react";

import * as reportsApi from "../../Reports/api/reportsApi";
import type { GetGroupedAmountResponse } from "../../Reports/types/GetGroupedAmountResponse";
import {
  getFirstDayOfCurrentMonth,
  getLastDayOfCurrentMonth,
} from "../../shared/common/utils";
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

  function setResult(response: GetGroupedAmountResponse) {
    setMonthWeeklyAmount(
      response.amountByPeriod.map((amount) => mapWeekAmount(amount)),
    );
  }

  useEffect(() => {
    setIsLoading(true);
    setError(null);

    const groupingType = "Week";

    reportsApi
      .getGroupedAmount(
        defaultCurrency,
        groupingType,
        getFirstDayOfCurrentMonth(),
        getLastDayOfCurrentMonth(),
      )
      .then(setResult)
      .catch(setError)
      .finally(() => setIsLoading(false));
  }, []);

  return {
    monthWeeklyAmount,
    isLoading,
    error,
  };
}

export default useMonthWeeklyAmount;
