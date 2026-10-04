import "./css/PurchaseRow.css";

import { useState } from "react";

import { formatAmount, formatDate } from "../../../shared/common/utils";
import type {
  PriceInfoDto,
  PurchaseDto,
} from "../../types/GetPurchasesResponse";
import PriceHistoryTable from "./PriceHistoryTable";

type PurchaseRowProps = {
  purchase: PurchaseDto;
  fromDate?: string;
  toDate?: string;
};

type PriceLineProps = {
  title: "Min price" | "Max price";
  price: PriceInfoDto;
};

function PriceLine({ title, price }: PriceLineProps) {
  const className = title === "Min price" ? "min" : "max";

  return (
    <div
      className={`purchase-row__price purchase-row__price--${className}`}
      aria-label={title}
    >
      <span className="purchase-row__label">{title}</span>
      <span>{formatDate(price.date)}</span>
      <span>{price.shopName}</span>
      <strong>{formatAmount(price.price, price.currency)}</strong>
    </div>
  );
}
function PurchaseRow({ purchase, fromDate, toDate }: PurchaseRowProps) {
  const [isExpanded, setIsExpanded] = useState(false);

  return (
    <div className="purchase-row">
      <div className="purchase-row__line">
        <div>
          <span className="purchase-row__label">Item</span>
          <strong>{purchase.name}</strong>
        </div>
        <div>
          <span className="purchase-row__label">Unit</span>
          <strong>{purchase.unit}</strong>
        </div>
        <div>
          <span className="purchase-row__label">Category</span>
          <strong>{purchase.categoryName}</strong>
        </div>
        <div className="purchase-row__prices">
          <PriceLine title="Min price" price={purchase.minPrice} />
          <PriceLine title="Max price" price={purchase.maxPrice} />
        </div>
        <button
          className="purchase-row__history_button"
          type="button"
          onClick={() => setIsExpanded((current) => !current)}
          aria-expanded={isExpanded}
          aria-label={
            isExpanded
              ? `Hide prices history of ${purchase.name}`
              : `Show prices history of ${purchase.name}`
          }
        >
          {isExpanded ? "-" : "+"}
        </button>
      </div>
      {isExpanded && (
        <PriceHistoryTable
          itemId={purchase.id}
          itemName={purchase.name}
          fromDate={fromDate}
          toDate={toDate}
        />
      )}
    </div>
  );
}

export default PurchaseRow;
