import "./css/AddExpenseModal.css";

import { useState } from "react";

import { defaultCurrency } from "../../../shared/common/consts";
import { getTodayDate } from "../../../shared/common/utils";
import type { Currency } from "../../../shared/types/Currency";
import type { SearchShopDto } from "../../../Shops/types/dto/SearchShopDto";
import {
  createExpense,
  createExpenseDetail,
  deleteExpenseDetail,
  updateExpense,
  updateExpenseDetail,
} from "../../api/expensesApi";
import type { AddExpenseDetailData } from "../../types/AddExpenseDetailData";
import type { GetExpenseByIdResponse } from "../../types/GetExpenseByIdResponse";
import ExpenseDetailsInfo from "./ExpenseDetailsInfo";
import ExpenseInfo from "./ExpenseInfo";
import type { ExpenseValidationErrors } from "./validation/expenseValidation";
import { validateExpense } from "./validation/expenseValidation";

type AddExpenseModalProps = {
  onClose: () => void;
  onCreated: () => void;
  initialExpense?: GetExpenseByIdResponse;
};

function AddExpenseModal({
  onClose,
  onCreated,
  initialExpense,
}: AddExpenseModalProps) {
  const isEditing = initialExpense !== undefined;
  const [date, setDate] = useState(
    initialExpense?.expenseDate ?? getTodayDate(),
  );
  const [shop, setShop] = useState<SearchShopDto | null>(
    initialExpense?.shopId && initialExpense.shopName
      ? {
          id: initialExpense.shopId,
          name: initialExpense.shopName,
          retailerId: "",
          retailerName: "",
          country: "",
          city: "",
        }
      : null,
  );
  const [amount, setAmount] = useState(
    initialExpense ? String(initialExpense.totalAmount) : "",
  );
  const [currency, setCurrency] = useState<Currency>(
    initialExpense?.currency ?? defaultCurrency,
  );
  const [details, setDetails] = useState<AddExpenseDetailData[]>(
    initialExpense?.details.map((detail) => ({
      id: detail.id,
      itemId: detail.itemId,
      itemName: detail.itemName,
      categoryId: detail.categoryId,
      categoryName: detail.categoryName,
      quantity: detail.quantity,
      unit: detail.unit,
      discount: detail.discountPercent,
      price: detail.totalPrice,
    })) ?? [],
  );

  const [isAddingExpenseDetail, setIsAddingExpenseDetail] = useState(false);
  const [editingDetailIdx, setEditingDetailIdx] = useState<number | null>(null);

  const [validationErrors, setValidationErrors] =
    useState<ExpenseValidationErrors>({});

  async function handleSave() {
    const errors = validateExpense({
      date,
      shop,
      currency,
      amount,
      details,
    });

    if (Object.keys(errors).length > 0) {
      setValidationErrors(errors);
      return;
    }
    setValidationErrors({});

    try {
      if (isEditing) {
        await updateExpense(initialExpense.id, {
          sharedGroupId: initialExpense.sharedGroupId,
          shopId: shop?.id ?? null,
          totalAmount: Number(amount),
          currency,
          expenseDate: date,
        });

        const currentDetailIds = new Set(
          details.flatMap((detail) => (detail.id ? [detail.id] : [])),
        );

        await Promise.all(
          initialExpense.details
            .filter((detail) => !currentDetailIds.has(detail.id))
            .map((detail) => deleteExpenseDetail(initialExpense.id, detail.id)),
        );

        await Promise.all(
          details.map((detail) => {
            const request = {
              itemId: detail.itemId,
              totalPrice: detail.price,
              currency,
              quantity: detail.quantity,
              discount: detail.discount
                ? { value: detail.discount, type: "Amount" as const }
                : null,
            };
            return detail.id
              ? updateExpenseDetail(initialExpense.id, detail.id, request)
              : createExpenseDetail(initialExpense.id, request);
          }),
        );
      } else {
        await createExpense({
          sharedGroupId: null,
          shopId: shop?.id ?? null,
          totalAmount: Number(amount),
          currency,
          expenseDate: date,
          details,
        });
      }

      onCreated();
      onClose();
    } catch {
      setValidationErrors({});
    }
  }

  function saveDetail(detail: AddExpenseDetailData) {
    if (editingDetailIdx !== null) {
      setDetails((prev) =>
        prev.map((item, detailIndex) =>
          detailIndex === editingDetailIdx ? detail : item,
        ),
      );

      setEditingDetailIdx(null);
    } else {
      setDetails((prev) => [...prev, detail]);
    }
    setIsAddingExpenseDetail(false);
  }

  function deleteDetail(index: number) {
    setDetails((prev) =>
      prev.filter((_, detailIndex) => detailIndex !== index),
    );
  }

  function cancelDetail() {
    setEditingDetailIdx(null);
    setIsAddingExpenseDetail(false);
  }

  return (
    <div className="modal-overlay">
      <div className="add-expense-modal">
        <header className="add-expense-modal__header">
          <div>
            <p className="add-expense-modal__eyebrow">
              {isEditing ? "Update your records" : "Track a purchase"}
            </p>
            <h2>{isEditing ? "Edit Expense" : "New Expense"}</h2>
          </div>
          <button type="button" onClick={onClose} aria-label="Close dialog">
            X
          </button>
        </header>

        <div className="add-expense-modal__content">
          <ExpenseInfo
            errors={validationErrors}
            date={date}
            onDateChange={setDate}
            shop={shop}
            onShopChange={setShop}
            amount={amount}
            onAmountChange={setAmount}
            currency={currency}
            onCurrencyChange={setCurrency}
          />
          <ExpenseDetailsInfo
            expenseAmount={amount}
            currency={currency}
            isAddingExpenseDetail={isAddingExpenseDetail}
            details={details}
            onClickAddDetail={() => setIsAddingExpenseDetail(true)}
            onClickSaveDetail={saveDetail}
            onClickDeleteDetail={deleteDetail}
            onClickCancelDetail={cancelDetail}
            onClickEditDetail={(index: number) => {
              setEditingDetailIdx(index);
            }}
            editingDetailIdx={editingDetailIdx}
          />
        </div>

        <footer className="add-expense-modal__footer">
          <button
            type="button"
            onClick={onClose}
            className="add-expense-modal__cancel"
          >
            Cancel
          </button>
          <button
            type="button"
            onClick={handleSave}
            className="add-expense-modal__save"
          >
            {isEditing ? "Save" : "Add"}
          </button>
        </footer>
      </div>
    </div>
  );
}

export default AddExpenseModal;
