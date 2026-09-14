export const currencies = ["NZD", "RUB", "USD"] as const;

export type Currency = (typeof currencies)[number];
