import { apiClient } from "../../../api/api";
import { mapExpenseDetailToRequest } from "../mappers/ExpenseDetailMapper";
import type {
  CreateExpenseRequest,
  CreateExpenseParams,
} from "../types/CreateExpenseRequest";
import type { Currency } from "../types/Defs";
import type { Discount } from "../types/Discount";
import type { ExpenseFiltersData } from "../types/ExpenseFiltersData";
import type { GetExpenseByIdResponse } from "../types/GetExpenseByIdResponse";
import type { GetExpensesResponse } from "../types/GetExpensesResponse";
import type { PageRequestData } from "../types/PageRequestData";
import type { UpdateExpenseParams } from "../types/UpdateExpenseParams";

const API_EXPENSES = "api/expenses";

export async function getExpenses(
  page: PageRequestData,
  filters?: ExpenseFiltersData | null,
): Promise<GetExpensesResponse> {
  const params = new URLSearchParams({
    page: page.pageNumber.toString(),
    pageSize: page.pageSize.toString(),
  });

  if (filters) {
    if (filters.fromDate) {
      params.append("fromDate", filters.fromDate);
    }

    if (filters.toDate) {
      params.append("toDate", filters.toDate);
    }

    filters.shops?.forEach((elem) => {
      params.append("shopIds", elem.id);
    });

    filters.categories?.forEach((elem) => {
      params.append("categoryIds", elem.id);
    });

    filters.items?.forEach((elem) => {
      params.append("itemIds", elem.id);
    });

    filters.retailers?.forEach((elem) => {
      params.append("retailerIds", elem.id);
    });

    if (filters.currency) {
      params.append("currency", filters.currency);
    }

    if (filters.fromAmount !== undefined) {
      params.append("fromAmount", filters.fromAmount.toString());
    }

    if (filters.toAmount !== undefined) {
      params.append("toAmount", filters.toAmount.toString());
    }
  }

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
