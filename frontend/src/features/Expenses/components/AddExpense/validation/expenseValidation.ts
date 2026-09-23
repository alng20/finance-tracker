import type { Currency } from "../../../../shared/types/Currency";
import type { ShopDto } from "../../../../Shops/types/dto/ShopDto";
import type { AddExpenseDetailData } from "../../../types/AddExpenseDetailData";

export type ExpenseValidationErrors = {
  date?: string;
  shop?: string;
  currency?: string;
  amount?: string;
  details?: string;
  detailErrors?: Record<number, ExpenseDetailValidationErrors>;
};

export type ExpenseDetailValidationErrors = {
  itemId?: string;
  categoryName?: string;
  quantity?: string;
  unit?: string;
  discount?: string;
  price?: string;
};

export function validateExpense({
  date,
  shop,
  currency,
  amount,
  details,
}: {
  date: string;
  shop: ShopDto | null;
  currency: Currency;
  amount: string;
  details: AddExpenseDetailData[];
}): ExpenseValidationErrors {
  const errors: ExpenseValidationErrors = {};

  if (!date) {
    errors.date = "Date is required";
  }

  // TODO: Create warning
  if (!shop) {
    errors.shop = "Shop is required";
  }

  if (!currency) {
    errors.currency = "Currency is required";
  }

  if (!amount.trim()) {
    errors.amount = "Amount is required";
  } else if (Number(amount) <= 0) {
    errors.amount = "Amount must be greater than 0";
  }

  details.forEach((detail, index) => {
    const detailErrors = validateExpenseDetail(detail);

    if (Object.keys(detailErrors).length > 0) {
      if (!errors.detailErrors) {
        errors.detailErrors = {};
      }

      errors.detailErrors[index] = detailErrors;
    }
  });

  return errors;
}

export function validateExpenseDetail(
  detail: AddExpenseDetailData,
): ExpenseDetailValidationErrors {
  const errors: ExpenseDetailValidationErrors = {};

  if (!detail.itemId) {
    errors.itemId = "Item is required";
  }

  if (!detail.categoryName) {
    errors.categoryName = "Category is required";
  }

  if (detail.quantity <= 0) {
    errors.quantity = "Quantity must be greater than 0";
  }

  if (!detail.unit) {
    errors.unit = "Unit is required";
  }

  if (detail.discount < 0 || detail.discount > detail.price) {
    errors.discount = `Discount amount must be between 0 and ${detail.price}`;
  }

  if (detail.price <= 0) {
    errors.price = "Price must be greater than 0";
  }

  return errors;
}
