import "./css/ExpenseDetailsTable.css";
import type { ExpenseDetailRowData } from "./data/ExpenseDetailRowData";
import type { ExpenseDetailsSummary } from "./data/ExpenseDetailsSummary";

type ExpenseDetailsTableProps = {
  details: ExpenseDetailRowData[];
  summary: ExpenseDetailsSummary;
};

function ExpenseDetailsTable({ details, summary }: ExpenseDetailsTableProps) {
  return (
    <div className="expense-details-table">
      <div className="expense-details-table__header">
        <div>Item</div>
        <div>Category</div>
        <div>Quantity</div>
        <div>Unit</div>
        <div>Discount</div>
        <div>Price</div>
      </div>
      {details.map((detail) => (
        <div className="expense-details-table__data" key={detail.id}>
          <div className="expense-details-table__data-name">
            {detail.itemName}
          </div>
          <div className="expense-details-table__data-category">
            {detail.categoryName}
          </div>
          <div className="expense-details-table__data-quantity">
            {detail.quantity}
          </div>
          <div className="expense-details-table__data-unit">{detail.unit}</div>
          <div className="expense-details-table__data-discount">
            {detail.discountPercent == 0 ? "-" : `${detail.discountPercent}%`}
          </div>
          <div className="expense-details-table__data-price">
            {detail.totalPrice} {detail.currency}
          </div>
        </div>
      ))}
      <div className="expense-details-table__summary">
        <div>
          <span>Total items</span>
          <strong>{summary.number}</strong>
        </div>
        <div>
          <span>Detailed</span>
          <strong>
            {summary.detailed} {summary.currency}
          </strong>
        </div>
        <div>
          <span>Undetailed</span>
          <strong>
            {summary.undetailed} {summary.currency}
          </strong>
        </div>
      </div>
    </div>
  );
}

export default ExpenseDetailsTable;
