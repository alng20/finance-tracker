import { useEffect, useState } from "react";

import * as reportsApi from "../../Reports/api/reportsApi";
import type { GetGroupedAmountResponse } from "../../Reports/types/GetGroupedAmountResponse";
import type { Currency } from "../../shared/types/Currency";
import { getTotalAmount } from "../../Reports/types/GetGroupedAmountResponse";
import {
  formatAmount,
  getFirstDayOfCurrentMonth,
} from "../../shared/common/utils";
import { defaultCurrency } from "../../shared/common/consts";

type MonthAmountResult = {
  currency: Currency;
  amount: number;
  amountFormatted: string;
};

function useMonthAmount() {
  const [monthAmount, setMonthAmount] = useState<MonthAmountResult | null>(
    null,
  );
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  function setResult(response: GetGroupedAmountResponse) {
    const amount = response ? getTotalAmount(response) : 0;
    const currency = response?.currency ?? "NZD";
    const result: MonthAmountResult = {
      currency: response?.currency ?? "NZD",
      amount: amount,
      amountFormatted: formatAmount(amount, currency),
    };
    setMonthAmount(result);
  }

  useEffect(() => {
    setIsLoading(true);
    setError(null);

    const groupingType = "Month";

    reportsApi
      .getGroupedAmount(
        defaultCurrency,
        groupingType,
        getFirstDayOfCurrentMonth(),
      )
      .then(setResult)
      .catch(setError)
      .finally(() => setIsLoading(false));
  }, []);

  return {
    monthAmount,
    isLoading,
    error,
  };
}

export default useMonthAmount;
