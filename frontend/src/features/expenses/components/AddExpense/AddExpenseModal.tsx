import "./css/AddExpenseModal.css";

import { useState } from "react";
import ExpenseInfo from "./ExpenseInfo";
import ExpenseDetailsInfo from "./ExpenseDetailsInfo";

type AddExpenseModalProps = {
  onClose: () => void;
};

function AddExpenseModal({ onClose }: AddExpenseModalProps) {
  const [isAddingExpenseDetail, setIsAddingExpenseDetail] = useState(false);
  const [expenseAmount, setExpenseAmount] = useState("");
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
            amount={expenseAmount}
            onAmountChange={setExpenseAmount}
          />
          <ExpenseDetailsInfo
            expenseAmount={expenseAmount}
            isAddingExpenseDetail={isAddingExpenseDetail}
            onClick={() => setIsAddingExpenseDetail(true)}
            onCancel={() => setIsAddingExpenseDetail(false)}
            onSave={() => setIsAddingExpenseDetail(false)}
          />
        </div>

        <footer className="add-expense-modal__footer">
          <button type="button">Add</button>
        </footer>
      </div>
    </div>
  );
}

export default AddExpenseModal;
