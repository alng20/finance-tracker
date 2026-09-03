import "./css/ExpenseRow.css";

import { useState } from "react";
import type { ExpenseDto } from "../../types/GetExpensesResponse";
import { deleteExpense, getExpenseById } from "../../api/expensesApi";
import type {
  ExpenseDetailDto,
  GetExpenseByIdResponse,
} from "../../types/GetExpenseByIdResponse";
import type { ExpenseDetailRowData } from "./data/ExpenseDetailRowData";
import ExpenseDetailsTable from "./ExpenseDetailsTable";
import type { Currency } from "../../types/Defs";
import type { ExpenseDetailsSummary } from "./data/ExpenseDetailsSummary";
import { defaultCurrency } from "../../../shared/common/consts";

type ExpenseRowProps = {
  expense: ExpenseDto;
  onEdit: (expense: GetExpenseByIdResponse) => void;
  onChanged: () => void;
};

export function mapExpenseDetail(
  currency: Currency,
  dto: ExpenseDetailDto,
): ExpenseDetailRowData {
  return {
    id: dto.id,
    itemName: dto.itemName,
    categoryName: dto.categoryName,
    unit: dto.unit,
    discountPercent: dto.discountPercent,
    totalPrice: dto.totalPrice,
    currency: currency,
    quantity: dto.quantity,
  };
}

function ExpenseRow({ expense, onEdit, onChanged }: ExpenseRowProps) {
  const [isExpanded, setIsExpanded] = useState(false);
  const [details, setDetails] = useState<ExpenseDetailRowData[]>([]);
  const [summary, setSummary] = useState<ExpenseDetailsSummary>({
    number: 0,
    currency: defaultCurrency,
    detailed: 0,
    undetailed: 0,
  });

  async function onClickExpand() {
    if (isExpanded) {
      setIsExpanded(false);
      return;
    }

    try {
      const data = await getExpenseById(expense.id);
      const mappedDetails = data.details.map((detail) =>
        mapExpenseDetail(data.currency, detail),
      );
      setDetails(mappedDetails);
      setSummary({
        number: data.details.length,
        currency: data.currency,
        detailed: data.detailedAmount,
        undetailed: data.undetailedAmount,
      });
      setIsExpanded(true);
    } catch (error) {
      console.error("Failed to load expense details:", error);
    }
  }

  async function handleEdit() {
    try {
      onEdit(await getExpenseById(expense.id));
    } catch (error) {
      console.error("Failed to load expense:", error);
    }
  }

  async function handleDelete() {
    if (!window.confirm("Delete this expense?")) {
      return;
    }

    try {
      await deleteExpense(expense.id);
      onChanged();
    } catch (error) {
      console.error("Failed to delete expense:", error);
    }
  }

  return (
    <div className="expense-row">
      <div className="expense-row__info">
        <div className="expense-row__date">
          <span className="expense-row__label">Date</span>
          <strong>{expense.expenseDate}</strong>
        </div>
        <div className="expense-row__shop">
          <span className="expense-row__label">Shop</span>
          <strong>{expense.shopName ?? "Unknown"}</strong>
        </div>
        <div className="expense-row__info_button_amount">
          <div className="expense-row__amount" aria-label="Expense amount">
            <span className="expense-row__label">Amount</span>
            {expense.totalAmount} {expense.currency}
          </div>
          <button
            id={expense.id}
            className="expense-row__button"
            type="button"
            onClick={onClickExpand}
            aria-expanded={isExpanded}
            aria-label={
              isExpanded ? "Hide expense details" : "Show expense details"
            }
          >
            {isExpanded ? "-" : "+"}
          </button>
        </div>
      </div>
      <div className="expense-row__edit_delete_buttons">
        <button
          id={expense.id}
          className="expense-row__delete_button"
          type="button"
          onClick={handleDelete}
        >
          Delete
        </button>
        <button
          id={expense.id}
          className="expense-row__edit_button"
          type="button"
          onClick={handleEdit}
        >
          Edit expense
        </button>
      </div>
      {isExpanded && (
        <ExpenseDetailsTable details={details} summary={summary} />
      )}
    </div>
  );
}

export default ExpenseRow;
