import type { AddExpenseDetailData } from "../types/AddExpenseDetailData";
import type { CreateExpenseDetailRequest } from "../types/CreateExpenseRequest";

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
