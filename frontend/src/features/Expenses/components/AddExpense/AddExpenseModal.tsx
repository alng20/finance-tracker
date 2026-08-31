import "./css/AddExpenseModal.css";

import { useState } from "react";

import { defaultCurrency } from "../../../shared/consts";
import type { Shop } from "../../../Shops/types/Shop";
import type { AddExpenseDetailData } from "../../types/AddExpenseDetailData";
import ExpenseDetailsInfo from "./ExpenseDetailsInfo";
import ExpenseInfo from "./ExpenseInfo";
import type { ExpenseValidationErrors } from "./validation/expenseValidation";
import { validateExpense } from "./validation/expenseValidation";
import { createExpense } from "../../api/expensesApi";

type AddExpenseModalProps = {
  onClose: () => void;
};

function AddExpenseModal({ onClose }: AddExpenseModalProps) {
  const [date, setDate] = useState("");
  const [shop, setShop] = useState<Shop | null>(null);
  const [amount, setAmount] = useState("");
  const [currency, setCurrency] = useState(defaultCurrency);
  const [details, setDetails] = useState<AddExpenseDetailData[]>([]);

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
      await createExpense({
        sharedGroupId: null,
        shopId: shop?.id ?? null,
        totalAmount: Number(amount),
        currency,
        expenseDate: date,
        details,
      });

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
          <h2>New Expense</h2>
          <button type="button" onClick={onClose}>
            x
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

        <footer className="add-expense-modal__footer" onClick={handleSave}>
          <button type="button">Add</button>
        </footer>
      </div>
    </div>
  );
}

export default AddExpenseModal;
