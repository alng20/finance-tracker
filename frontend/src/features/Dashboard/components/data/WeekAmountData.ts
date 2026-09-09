import type { Currency } from "../../../shared/types/Defs";
import type { PeriodAmountReport } from "../../../Reports/types/GetGroupedAmountResponse";
import { defaultCurrency } from "../../../shared/common/consts";

export type WeekAmountData = {
  fromDate: string;
  toDate: string;
  amount: number;
  currency: Currency;
};

export function mapWeekAmount(amount: PeriodAmountReport): WeekAmountData {
  return {
    fromDate: amount.reportPeriod.fromDate,
    toDate: amount.reportPeriod.toDate,
    amount: amount.amount,
    currency: defaultCurrency, // TODO: Use currency from request
  };
}
