import type { Currency } from "../../types/Defs";
import type { ExpenseDto } from "../../types/GetExpensesResponse";
import "./css/ExpenseResultsSummary.css";

type ExpenseResultsSummaryProps = {
  expenses: ExpenseDto[];
  totalCount: number;
};

const currencyOrder: Currency[] = ["NZD", "RUB", "USD"];

function formatAmount(amount: number, currency: Currency) {
  return new Intl.NumberFormat(undefined, {
    style: "currency",
    currency,
    maximumFractionDigits: 2,
  }).format(amount);
}

export function ExpenseResultsSummary({
  expenses,
  totalCount,
}: ExpenseResultsSummaryProps) {
  const amountsByCurrency = expenses.reduce<Partial<Record<Currency, number>>>(
    (totals, expense) => ({
      ...totals,
      [expense.currency]: (totals[expense.currency] ?? 0) + expense.totalAmount,
    }),
    {},
  );

  const shopCounts = expenses.reduce<Record<string, number>>(
    (counts, expense) => {
      const shopName = expense.shopName ?? "Unknown shop";
      counts[shopName] = (counts[shopName] ?? 0) + 1;
      return counts;
    },
    {},
  );

  const topShop = Object.entries(shopCounts).sort(
    ([, firstCount], [, secondCount]) => secondCount - firstCount,
  )[0];

  return (
    <section className="expense-results-summary" aria-label="Expense summary">
      <div className="expense-results-summary__heading">
        <div>
          <p className="expense-results-summary__eyebrow">Spending overview</p>
          <h2>Spending snapshot</h2>
        </div>
        <span className="expense-results-summary__count">
          {totalCount} {totalCount === 1 ? "expense" : "expenses"}
        </span>
      </div>

      <div className="expense-results-summary__metrics">
        <div className="expense-results-summary__metric">
          <span>Amount on this page</span>
          <strong>
            {currencyOrder
              .filter((currency) => amountsByCurrency[currency] !== undefined)
              .map((currency) => (
                <span key={currency}>
                  {formatAmount(amountsByCurrency[currency] ?? 0, currency)}
                </span>
              ))}
          </strong>
        </div>
        <div className="expense-results-summary__metric">
          <span>Average per expense</span>
          <strong>
            {currencyOrder
              .filter((currency) => amountsByCurrency[currency] !== undefined)
              .map((currency) => {
                const count = expenses.filter(
                  (expense) => expense.currency === currency,
                ).length;
                return (
                  <span key={currency}>
                    {formatAmount(
                      (amountsByCurrency[currency] ?? 0) / count,
                      currency,
                    )}
                  </span>
                );
              })}
          </strong>
        </div>
        <div className="expense-results-summary__metric">
          <span>Most frequent shop</span>
          <strong>{topShop?.[0] ?? "Not available"}</strong>
        </div>
      </div>
    </section>
  );
}
