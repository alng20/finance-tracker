import "./css/MonthAmountCard.css";
import useMonthAmount from "../hooks/useMonthAmount";

function MonthAmountCard() {
  const { monthAmount: amount, isLoading, error } = useMonthAmount();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Failed to load current month amount.</div>;
  }

  return (
    <section className="month_amount_card">
      <h2 className="month_amount_card__title">This Month</h2>
      <span className="month_amount_card__value">
        {amount?.amountFormatted}
      </span>
    </section>
  );
}

export default MonthAmountCard;
