import "./css/ExpenseDetailsInfo.css";

import { useMemo } from "react";

import { formatAmount } from "../../../shared/common/utils";
import type { Currency } from "../../../shared/types/Currency";
import type { AddExpenseDetailData } from "../../types/AddExpenseDetailData";
import AddExpenseDetail from "./AddExpenseDetail";

type AddExpenseDetailButton = {
  isAddingExpenseDetail: boolean;
  onClickAddDetail: () => void;
};

type ExpenseDetailRowDataButtons = {
  onClickSaveDetail: (detail: AddExpenseDetailData) => void;
  onClickEditDetail: (index: number) => void;
  onClickDeleteDetail: (index: number) => void;
  onClickCancelDetail: () => void;
  editingDetailIdx: number | null;
};

type ExpenseDetailsInfoProps = {
  expenseAmount: string;
  currency: Currency;
  details: AddExpenseDetailData[];
} & AddExpenseDetailButton &
  ExpenseDetailRowDataButtons;

function round(value: number): number {
  return Math.round(value * 100) / 100;
}

function ExpenseDetailsInfo(props: ExpenseDetailsInfoProps) {
  const summary = useMemo(() => {
    const detailed = props.details.reduce(
      (sum, detail) => sum + detail.price,
      0,
    );
    const undetailed = Number(props.expenseAmount) - detailed;

    return {
      number: props.details.length,
      detailed: round(detailed),
      undetailed: round(undetailed),
    };
  }, [props.details, props.expenseAmount]);

  return (
    <section className="expense-details-info">
      <div className="expense-details-header_and_add">
        <h2>Details</h2>
        <button
          onClick={props.onClickAddDetail}
          type="button"
          className="expense-details-add-button"
        >
          + Add detail
        </button>
      </div>

      <div className="expense-details-summary">
        <span>number: {summary?.number ?? "0"} </span>
        <span>•</span>
        <span>
          detailed: {formatAmount(summary?.detailed ?? +0.0, props.currency)}
        </span>
        <span>•</span>
        <span>
          undetailed:{" "}
          {formatAmount(summary?.undetailed ?? +0.0, props.currency)}
        </span>
      </div>

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

      {props.details.length > 0 && (
        <table className="expense-details-info__table">
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
              <tr key={detail.id ?? `${detail.itemId}-${index}`}>
                <td>{detail.itemName}</td>
                <td>{detail.categoryName}</td>
                <td>{detail.quantity}</td>
                <td>{detail.unit}</td>
                <td>{detail.discount}</td>
                <td>{formatAmount(detail.price, props.currency)}</td>
                <td>
                  <button
                    type="button"
                    className="expense-details-edit-button"
                    onClick={() => {
                      props.onClickEditDetail(index);
                    }}
                  >
                    Edit
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
                    Delete
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}

export default ExpenseDetailsInfo;
