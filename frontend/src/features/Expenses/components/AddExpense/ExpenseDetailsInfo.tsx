import "./css/ExpenseDetailsInfo.css";

import { useEffect, useState } from "react";
import AddExpenseDetail from "./AddExpenseDetail";
import type { ExpenseDetail } from "../../types/ExpenseDetail";

type AddExpenseDetailButton = {
  isAddingExpenseDetail: boolean;
  onClickAddDetail: () => void;
};

type ExpenseDetailRowButtons = {
  onClickSaveDetail: (detail: ExpenseDetail) => void;
  onClickEditDetail: (index: number) => void;
  onClickDeleteDetail: (index: number) => void;
  onClickCancelDetail: () => void;
  editingDetailIdx: number | null;
};

type ExpenseDetailsInfoProps = {
  expenseAmount: string;
  currency: string;
  details: ExpenseDetail[];
} & AddExpenseDetailButton &
  ExpenseDetailRowButtons;

type ExpenseDetailsSummary = {
  number: number;
  detailed: number;
  undetailed: number;
};

function ExpenseDetailsInfo(props: ExpenseDetailsInfoProps) {
  const [summary, setSummary] = useState<ExpenseDetailsSummary | null>(null);

  function calculateSummary() {
    const detailed = props.details.reduce(
      (sum, detail) => sum + detail.price,
      0,
    );
    const undetailed = Number(props.expenseAmount) - detailed;
    const summary: ExpenseDetailsSummary = {
      number: props.details.length,
      detailed: detailed,
      undetailed: undetailed,
    };
    setSummary(summary);
  }

  useEffect(() => {
    calculateSummary();
  }, [props.details, props.expenseAmount]);

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
          {props.details.map((detail, index) => (
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
                    props.onClickEditDetail(index);
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
                    props.onClickDeleteDetail(index);
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
        <span>
          detailed: {summary?.detailed ?? "0"} {props.currency}
        </span>
        <span>•</span>
        <span>
          undetailed: {summary?.undetailed ?? "0"} {props.currency}
        </span>
      </div>

      <button
        onClick={props.onClickAddDetail}
        type="button"
        className="expense-details-add-button"
      >
        + Detail
      </button>

      {(props.isAddingExpenseDetail || props.editingDetailIdx != null) && (
        <AddExpenseDetail
          key={props.editingDetailIdx ?? "new"}
          initialDetail={
            props.editingDetailIdx !== null
              ? props.details[props.editingDetailIdx]
              : undefined
          }
          onSave={props.onClickSaveDetail}
          onCancel={props.onClickCancelDetail}
        />
      )}
    </section>
  );
}

export default ExpenseDetailsInfo;
