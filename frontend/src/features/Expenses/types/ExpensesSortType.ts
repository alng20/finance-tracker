export const expensesSortTypes = ["Date", "TotalAmount", "Shop"] as const;

export type ExpensesSortType = (typeof expensesSortTypes)[number];
