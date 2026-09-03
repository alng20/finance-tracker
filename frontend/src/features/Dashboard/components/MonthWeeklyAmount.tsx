import { formatAmount, formatDate } from "../../shared/common/utils";
import useMonthWeeklyAmount from "../hooks/useMonthWeeklyAmount";
import "./css/MonthWeeklyAmount.css";

function MonthWeeklyAmountCard() {
  const {
    monthWeeklyAmount: amounts,
    isLoading,
    error,
  } = useMonthWeeklyAmount();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Failed to load month weekly amounts.</div>;
  }

  return (
    <section className="month_weekly_amount_card">
      <h2 className="month_weekly_amount_card__header">Weekly spendings</h2>

      <div className="month_weekly_amount_card__amounts">
        {amounts.length === 0 ? (
          <p className="month_weekly_amount_card__no_amounts">
            No weekly spendings.
          </p>
        ) : (
          amounts.map((amount, index) => (
            <div key={index} className="month_weekly_amount_card__row">
              <span className="month_weekly_amount_card__amount">
                {formatAmount(amount.amount, amount.currency)}
              </span>
              <div className="month_weekly_amount_card__period">
                <span className="month_weekly_amount_card__from_date">
                  {formatDate(amount.fromDate)}
                </span>
                <span className="month_weekly_amount_card__to_date">
                  {formatDate(amount.toDate)}
                </span>
              </div>
            </div>
          ))
        )}
      </div>
    </section>
  );
}

export default MonthWeeklyAmountCard;
