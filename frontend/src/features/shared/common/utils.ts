import type { Currency } from "../../Expenses/types/Defs";

export function formatAmount(amount: number, currency: Currency): string {
  return (
    new Intl.NumberFormat("en-NZ", {
      style: "currency",
      currency: currency,
    }).format(amount) + ` ${currency}`
  );
}
