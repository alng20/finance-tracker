import "./css/ExpenseRow.css";

import { useState } from "react";
import type { ExpenseDto } from "../../types/GetExpensesResponse";
import { getExpenseById } from "../../api/expensesApi";
import type { ExpenseDetailDto } from "../../types/GetExpenseByIdResponse";
import type { ExpenseDetailRowData } from "./data/ExpenseDetailRowData";
import ExpenseDetailsTable from "./ExpenseDetailsTable";
import type { Currency } from "../../types/Defs";
import type { ExpenseDetailsSummary } from "./data/ExpenseDetailsSummary";
import { defaultCurrency } from "../../../shared/common/consts";

type ExpenseRowProps = {
  expense: ExpenseDto;
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

function ExpenseRow({ expense }: ExpenseRowProps) {
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

  return (
    <div className="expense-row">
      <div className="expense-row__info">
        <div className="expense-row__date">{expense.expenseDate}</div>
        <div className="expense-row__shop">{expense.shopName ?? "Unknown"}</div>
        <div className="expense-row__info_button_amount">
          <div className="expense-row__amount">
            {expense.totalAmount} {expense.currency}
          </div>
          <button
            id={expense.id}
            className="expenses-table__button"
            type="button"
            onClick={onClickExpand}
          >
            {isExpanded ? "v" : ">"}
          </button>
        </div>
      </div>
      {isExpanded && (
        <ExpenseDetailsTable details={details} summary={summary} />
      )}
    </div>
  );
}

export default ExpenseRow;
