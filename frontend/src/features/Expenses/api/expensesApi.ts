import { apiClient } from "../../../api/api";
import { mapExpenseDetailToRequest } from "../mappers/ExpenseDetailMapper";
import type {
  CreateExpenseRequest,
  CreateExpenseData,
} from "../types/CreateExpenseRequest";
import type { GetExpenseByIdResponse } from "../types/GetExpenseByIdResponse";
import type { GetExpensesResponse } from "../types/GetExpensesResponse";

const API_EXPENSES = "api/expenses";

export async function getExpenses(): Promise<GetExpensesResponse> {
  const response = await apiClient.get(`${API_EXPENSES}`);

  if (!response.ok) {
    throw new Error("Failed to create expense");
  }

  return response.json();
}

export async function getExpenseById(
  id: string,
): Promise<GetExpenseByIdResponse> {
  const response = await apiClient.get(`/api/expenses/${id}`);

  if (!response.ok) {
    throw new Error("Failed to get expense by id");
  }

  return response.json();
}

export async function createExpense(expense: CreateExpenseData) {
  const request: CreateExpenseRequest = {
    sharedGroupId: null,
    shopId: expense.shopId,
    totalAmount: expense.totalAmount,
    currency: expense.currency,
    expenseDate: expense.expenseDate,
    details: expense.details.map(mapExpenseDetailToRequest),
  };

  const response = await apiClient.post(`${API_EXPENSES}`, request);

  if (!response.ok) {
    throw new Error("Failed to create expense");
  }

  const data = await response.json();

  return data;
}
