import "./css/ExpenseInfo.css";
import { defaultCurrency } from "../../../shared/consts/consts";
import { useState } from "react";
import type { Shop } from "../../../Shops/types/Shop";
import { useSearchShop } from "./helpers/useSearchShops";
import type { ExpenseValidationErrors } from "./validation/expenseValidation";

type DateProps = {
  date: string;
  onDateChange: (value: string) => void;
};

type ShopProps = {
  shop: Shop | null;
  onShopChange: (value: Shop | null) => void;
};

type AmountProps = {
  amount: string;
  onAmountChange: (value: string) => void;
};

type CurrencyProps = {
  currency: string;
  onCurrencyChange: (value: string) => void;
};

type ExpenseInfoProps = { errors: ExpenseValidationErrors } & DateProps &
  ShopProps &
  AmountProps &
  CurrencyProps;

function ExpenseInfo(props: ExpenseInfoProps) {
  const [searchShop, setSearchShop] = useState("");
  const { shops, clearShops } = useSearchShop(searchShop, props.shop);

  return (
    <section className="expense-info">
      <div className="expense-info__fields">
        <div className="expense-info__field">
          <label htmlFor="Date">Date</label>
          <input
            id="Date"
            type="date"
            value={props.date}
            onChange={(event) => {
              props.onDateChange(event.target.value);
            }}
          />
          {props.errors.date && (
            <span className="expense-info__field_error">
              {props.errors.date}
            </span>
          )}
        </div>
        <div className="expense-info__field expense-info__shop_search">
          <label htmlFor="shop">Shop</label>
          <input
            id="shop"
            type="text"
            value={searchShop}
            onChange={(event) => {
              setSearchShop(event.target.value);
              props.onShopChange(null);
            }}
          />
          {props.errors.shop && (
            <span className="expense-info__field_error">
              {props.errors.shop}
            </span>
          )}

          {shops.length > 0 && (
            <div className="expense-info__shop_search_results">
              {shops.map((shop) => (
                <button
                  key={shop.id}
                  type="button"
                  onClick={() => {
                    setSearchShop(shop.name);
                    props.onShopChange(shop);

                    clearShops();
                  }}
                >
                  {shop.name}
                </button>
              ))}
            </div>
          )}
        </div>

        <div className="expense-info__field">
          <label htmlFor="Amount">Amount</label>
          <input
            id="amount"
            type="number"
            value={props.amount}
            onChange={(event) => {
              props.onAmountChange(event.target.value);
            }}
          />
          {props.errors.amount && (
            <span className="expense-info__field_error">
              {props.errors.amount}
            </span>
          )}
        </div>
        <div className="expense-info__field">
          <label htmlFor="Currency">Currency</label>
          <select
            id="Currency"
            name="currency"
            defaultValue={defaultCurrency}
            value={props.currency}
            onChange={(event) => {
              props.onCurrencyChange(event.target.value);
            }}
          >
            {/* TODO: use GET api/currencies */}
            <option value="NZD">NZD</option>
            <option value="USD">USD</option>
            <option value="RUB">RUB</option>
          </select>
          {props.errors.date && (
            <span className="expense-info__field_error">
              {props.errors.currency}
            </span>
          )}
        </div>
      </div>
    </section>
  );
}

export default ExpenseInfo;
