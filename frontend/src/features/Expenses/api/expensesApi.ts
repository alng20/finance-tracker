import { apiClient } from "../../../api/api";
import { mapExpenseDetailToRequest } from "../mappers/ExpenseDetailMapper";
import type {
  CreateExpenseRequest,
  CreateExpenseParams,
} from "../types/CreateExpenseRequest";
import type { GetExpenseByIdResponse } from "../types/GetExpenseByIdResponse";
import type { GetExpensesResponse } from "../types/GetExpensesResponse";

const API_EXPENSES = "api/expenses";

export async function getExpenses(
  page?: number,
  pageSize?: number,
  fromDate?: string,
  toDate?: string,
): Promise<GetExpensesResponse> {
  const params = new URLSearchParams({
    ...(page !== undefined && { page: page.toString() }),
    ...(pageSize !== undefined && { pageSize: pageSize.toString() }),
    ...(fromDate && { fromDate }),
    ...(toDate && { toDate }),
  });

  const query = params.toString();

  return apiClient.get<GetExpensesResponse>(
    `${API_EXPENSES}${query ? `?${query}` : ""}`,
  );
}

export async function getExpenseById(
  id: string,
): Promise<GetExpenseByIdResponse> {
  return apiClient.get<GetExpenseByIdResponse>(`${API_EXPENSES}/${id}`);
}

export async function createExpense(expense: CreateExpenseParams) {
  const request: CreateExpenseRequest = {
    sharedGroupId: null,
    shopId: expense.shopId,
    totalAmount: expense.totalAmount,
    currency: expense.currency,
    expenseDate: expense.expenseDate,
    details: expense.details.map(mapExpenseDetailToRequest),
  };
  return apiClient.post<CreateExpenseRequest>(`${API_EXPENSES}`, request);
}
