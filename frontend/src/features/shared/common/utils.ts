import { CurrenciesDict, type Currency } from "../types/Currency";

export function formatAmount(amount: number, currency: Currency): string {
  return (
    new Intl.NumberFormat("en-NZ", {
      style: "currency",
      currency: currency,
    }).format(amount) + (currency == CurrenciesDict.NZD ? ` ${currency}` : ``)
  );
}

export function formatDate(
  date: string | Date,
  options: Intl.DateTimeFormatOptions = {
    day: "numeric",
    month: "short",
    year: "numeric",
  },
): string {
  return new Intl.DateTimeFormat("en-NZ", options).format(new Date(date));
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

function getDayOfCurrentWeek(offset: number): string {
  const today = new Date();
  const day = today.getDay();
  const diff = (day === 0 ? -6 : 1 - day) + offset;

  const date = new Date(today);
  date.setDate(today.getDate() + diff);

  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const dayOfMonth = String(date.getDate()).padStart(2, "0");

  return `${year}-${month}-${dayOfMonth}`;
}

export function getFirstDayOfCurrentWeek(): string {
  return getDayOfCurrentWeek(0);
}

export function getLastDayOfCurrentWeek(): string {
  return getDayOfCurrentWeek(6);
}
