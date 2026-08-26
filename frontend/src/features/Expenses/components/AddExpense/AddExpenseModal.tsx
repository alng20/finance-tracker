import "./css/AddExpenseModal.css";

import { useState } from "react";
import ExpenseInfo from "./ExpenseInfo";
import ExpenseDetailsInfo from "./ExpenseDetailsInfo";
import type { ExpenseDetail } from "../../types/ExpenseDetail";
import { defaultCurrency } from "../../../shared/consts/consts";
import type { Shop } from "../../../Shops/types/Shop";

import { validateExpense } from "./validation/expenseValidation";
import type { ExpenseValidationErrors } from "./validation/expenseValidation";

type AddExpenseModalProps = {
  onClose: () => void;
};

function AddExpenseModal({ onClose }: AddExpenseModalProps) {
  const [date, setDate] = useState("");
  const [shop, setShop] = useState<Shop | null>(null);
  const [amount, setAmount] = useState("");
  const [currency, setCurrency] = useState(defaultCurrency);
  const [details, setDetails] = useState<ExpenseDetail[]>([]);

  const [isAddingExpenseDetail, setIsAddingExpenseDetail] = useState(false);
  const [editingDetailIdx, setEditingDetailIdx] = useState<number | null>(null);

  const [validationErrors, setValidationErrors] =
    useState<ExpenseValidationErrors>({});

  function handleSave() {
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
    } else {
      setValidationErrors({});
    }
  }

  function saveDetail(detail: ExpenseDetail) {
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
