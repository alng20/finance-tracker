export const CurrenciesDict = {
  NZD: "NZD",
  RUB: "RUB",
  USD: "USD",
} as const;

export type Currency = (typeof CurrenciesDict)[keyof typeof CurrenciesDict];

export const currencies = Object.values(CurrenciesDict);
