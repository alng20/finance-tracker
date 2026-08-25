import "./css/ExpenseDetailsInfo.css";

import { useEffect, useState } from "react";
import AddExpenseDetail from "./AddExpenseDetail";
import type { ExpenseDetail } from "./AddExpenseDetail";

type AddExpenseDetailButtonProps = {
  expenseAmount: string;
  isAddingExpenseDetail: boolean;
  onClick: () => void;
  onCancel: () => void;
  onSave: () => void;
};

type ExpenseDetailsSummary = {
  number: number;
  detailed: number;
  undetailed: number;
};

function ExpenseDetailsInfo({
  expenseAmount,
  isAddingExpenseDetail,
  onClick,
  onCancel,
  onSave,
}: AddExpenseDetailButtonProps) {
  const [summary, setSummary] = useState<ExpenseDetailsSummary | null>(null);
  const [details, setExpenseDetails] = useState<ExpenseDetail[]>([]);
  const [editingDetailIndex, setEditingDetailIndex] = useState<number | null>(
    null,
  );

  function storeDetail(detail: ExpenseDetail) {
    if (editingDetailIndex !== null) {
      setExpenseDetails((prev) =>
        prev.map((item, detailIndex) =>
          detailIndex === editingDetailIndex ? detail : item,
        ),
      );

      setEditingDetailIndex(null);
    } else {
      setExpenseDetails((prev) => [...prev, detail]);
    }
    onSave();
  }

  function cancelDetail() {
    setEditingDetailIndex(null);
    onCancel();
  }

  function calculateSummary() {
    const detailed = details.reduce((sum, detail) => sum + detail.price, 0);
    const undetailed = Number(expenseAmount) - detailed;
    const summary: ExpenseDetailsSummary = {
      number: details.length,
      detailed: detailed,
      undetailed: undetailed,
    };
    setSummary(summary);
  }

  useEffect(() => {
    calculateSummary();
  }, [details, expenseAmount]);

  return (
    <section className="expense-details-info">
      <h2>Details</h2>

      <table className="expense-details-table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Category</th>
            <th>Quantity</th>
            <th>Unit</th>
            <th>Discount</th>
            <th>Price</th>
            <th></th>
            <th></th>
          </tr>
        </thead>

        <tbody>
          {details.map((detail, index) => (
            <tr>
              <td>{detail.itemName}</td>
              <td>{detail.categoryName}</td>
              <td>{detail.quantity}</td>
              <td>{detail.unit}</td>
              <td>{detail.discount}</td>
              <td>{detail.price}</td>
              <td>
                <button
                  type="button"
                  className="expense-details-edit-button"
                  onClick={() => {
                    setEditingDetailIndex(index);
                  }}
                >
                  /
                </button>
              </td>
              <td>
                <button
                  type="button"
                  className="expense-details-delete-button"
                  onClick={() => {
                    setExpenseDetails((prev) =>
                      prev.filter((_, detailIndex) => detailIndex !== index),
                    );
                  }}
                >
                  X
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className="expense-details-summary">
        <span>number: {summary?.number ?? "0"} </span>
        <span>•</span>
        <span>detailed: {summary?.detailed ?? "0"} NZD</span>
        <span>•</span>
        <span>undetailed: {summary?.undetailed ?? "0"} NZD</span>
      </div>

      <button
        onClick={onClick}
        type="button"
        className="expense-details-add-button"
      >
        + Detail
      </button>

      {(isAddingExpenseDetail || editingDetailIndex != null) && (
        <AddExpenseDetail
          key={editingDetailIndex ?? "new"}
          initialDetail={
            editingDetailIndex !== null
              ? details[editingDetailIndex]
              : undefined
          }
          onSave={storeDetail}
          onCancel={cancelDetail}
        />
      )}
    </section>
  );
}

export default ExpenseDetailsInfo;
