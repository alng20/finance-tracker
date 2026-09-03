import { apiClient } from "../../../api/api";
import { mapExpenseDetailToRequest } from "../mappers/ExpenseDetailMapper";
import type {
  CreateExpenseRequest,
  CreateExpenseParams,
} from "../types/CreateExpenseRequest";
import type { Currency } from "../types/Defs";
import type { Discount } from "../types/Discount";
import type { GetExpenseByIdResponse } from "../types/GetExpenseByIdResponse";
import type { GetExpensesResponse } from "../types/GetExpensesResponse";
import type { UpdateExpenseParams } from "../types/UpdateExpenseParams";

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

export async function updateExpense(id: string, expense: UpdateExpenseParams) {
  return apiClient.put<UpdateExpenseParams>(`${API_EXPENSES}/${id}`, {
    sharedGroupId: expense.sharedGroupId ?? null,
    shopId: expense.shopId,
    totalAmount: expense.totalAmount,
    currency: expense.currency,
    expenseDate: expense.expenseDate,
  });
}

export async function deleteExpense(id: string) {
  return apiClient.delete<void>(`${API_EXPENSES}/${id}`);
}

export async function updateExpenseDetail(
  expenseId: string,
  detailId: string,
  detail: {
    itemId: string;
    totalPrice: number;
    currency: Currency;
    quantity: number;
    discount: Discount | null;
  },
) {
  return apiClient.put(
    `${API_EXPENSES}/details/${expenseId}/${detailId}`,
    detail,
  );
}

export async function createExpenseDetail(
  expenseId: string,
  detail: {
    itemId: string;
    totalPrice: number;
    currency: Currency;
    quantity: number;
    discount: Discount | null;
  },
) {
  return apiClient.post(`${API_EXPENSES}/details/${expenseId}`, detail);
}

export async function deleteExpenseDetail(expenseId: string, detailId: string) {
  return apiClient.delete<void>(
    `${API_EXPENSES}/details/${expenseId}/${detailId}`,
  );
}
