import type { Currency } from "../../shared/types/Defs";

export type GetGroupedAmountResponse = {
  currency: Currency;
  groupingType: ReportGroupingType;
  amountByPeriod: PeriodAmountReport[];
};

export type PeriodAmountReport = {
  reportPeriod: ReportPeriod;
  amount: number;
};

export type ReportPeriod = {
  fromDate: string;
  toDate: string;
};

export type ReportGroupingType =
  "Undefined" | "Day" | "Week" | "Month" | "Year";

export function getTotalAmount(response: GetGroupedAmountResponse): number {
  return response.amountByPeriod.reduce(
    (total, period) => total + period.amount,
    0,
  );
}
