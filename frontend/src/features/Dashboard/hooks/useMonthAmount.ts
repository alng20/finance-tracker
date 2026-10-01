import { useEffect, useState } from "react";

import * as reportsApi from "../../Reports/api/reportsApi";
import type { GetGroupedAmountResponse } from "../../Reports/types/GetGroupedAmountResponse";
import { getGroupedTotalAmount } from "../../Reports/types/GetGroupedAmountResponse";
import { defaultCurrency } from "../../shared/common/consts";
import {
  formatAmount,
  getFirstDayOfCurrentMonth,
} from "../../shared/common/utils";
import type { Currency } from "../../shared/types/Currency";

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

  useEffect(() => {
    const groupingType = "Month";

    function setResult(response: GetGroupedAmountResponse) {
      const amount = response ? getGroupedTotalAmount(response) : 0;
      const currency = response?.currency ?? "NZD";
      const result: MonthAmountResult = {
        currency: currency,
        amount: amount,
        amountFormatted: formatAmount(amount, currency),
      };
      setMonthAmount(result);
      setIsLoading(false);
    }

    reportsApi
      .getGroupedAmount(
        defaultCurrency,
        groupingType,
        getFirstDayOfCurrentMonth(),
      )
      .then(setResult)
      .catch((err) => {
        setError(err);
        setIsLoading(false);
      });
  }, []);

  return {
    monthAmount,
    isLoading,
    error,
  };
}

export default useMonthAmount;
