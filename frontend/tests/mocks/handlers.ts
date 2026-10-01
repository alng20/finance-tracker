import { categoriesHandlers } from "./handlers/categories.handlers";
import { expensesHandlers } from "./handlers/expenses.handlers";
import { reportsHandlers } from "./handlers/reports.handlers";
import { retailersHandlers } from "./handlers/retailers.handlers";
import { shopsHandlers } from "./handlers/shops.handlers";

export const handlers = [
  ...expensesHandlers,
  ...shopsHandlers,
  ...categoriesHandlers,
  ...retailersHandlers,
  ...reportsHandlers,
];
