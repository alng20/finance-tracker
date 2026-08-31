import type { CreateExpenseDetailRequest } from "../types/CreateExpenseRequest";
import type { AddExpenseDetailData } from "../types/AddExpenseDetailData";

// TODO: Make changeable
const discountType = "Amount";

export function mapExpenseDetailToRequest(
  detail: AddExpenseDetailData,
): CreateExpenseDetailRequest {
  return {
    itemId: detail.itemId,
    totalPrice: detail.price,
    quantity: detail.quantity,
    discount: detail.discount
      ? {
          value: detail.discount,
          type: discountType,
        }
      : null,
  };
}
