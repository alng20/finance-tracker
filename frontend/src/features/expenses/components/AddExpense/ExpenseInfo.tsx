import "./css/ExpenseInfo.css";

type ExpenseInfoProps = {
  amount: string;
  onAmountChange: (value: string) => void;
};

function ExpenseInfo({ amount, onAmountChange }: ExpenseInfoProps) {
  return (
    <section className="expense-info">
      <div className="expense-info__fields">
        <div className="expense-field">
          <label htmlFor="Date">Date</label>
          <input id="Date" type="date" />
        </div>
        <div className="expense-field">
          <label htmlFor="shop">Shop</label>
          <input id="shop" type="text" />
        </div>
        <div className="expense-field">
          <label htmlFor="Amount">Amount</label>
          <input
            id="amount"
            type="number"
            value={amount}
            onChange={(event) => {
              onAmountChange(event.target.value);
            }}
          />
        </div>
        <div className="expense-field">
          <label htmlFor="Currency">Currency</label>
          <select id="Currency" name="currency" defaultValue="NZD">
            <option value="NZD">NZD</option>
            <option value="USD">USD</option>
            <option value="RUB">RUB</option>
          </select>
        </div>
      </div>
    </section>
  );
}

export default ExpenseInfo;
