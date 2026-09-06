import type { Currency } from "../../Expenses/types/Defs";

export function formatAmount(amount: number, currency: Currency): string {
  return (
    new Intl.NumberFormat("en-NZ", {
      style: "currency",
      currency: currency,
    }).format(amount) + ` ${currency}`
  );
}

export function formatDate(date: string | Date): string {
  return new Intl.DateTimeFormat("en-NZ", {
    day: "numeric",
    month: "short",
    year: "numeric",
  }).format(new Date(date));
}

export function getTodayDate() {
  const date: Date = new Date();

  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");

  return `${year}-${month}-${day}`;
}

export function getFirstDayOfCurrentMonth(): string {
  const now = new Date();
  const year = now.getFullYear();
  const month = String(now.getMonth() + 1).padStart(2, "0");

  return `${year}-${month}-01`;
}

export function getLastDayOfCurrentMonth(): string {
  const now = new Date();
  const year = now.getFullYear();
  const month = String(now.getMonth() + 1).padStart(2, "0");
  const day = new Date(year, Number(month), 0).getDate();

  return `${year}-${month}-${day}`;
}
