import "./css/ExpenseFilters.css";
// import type { ExpenseFiltersData } from "../../types/ExpenseFiltersData";

// export type ExpenseFiltersProps = {
//   setFilters: React.Dispatch<React.SetStateAction<ExpenseFiltersData | null>>;
// };

// function ExpenseFilters({ setFilters }: ExpenseFiltersProps) {
//   async function onClickApply() {
//     const filters: ExpenseFiltersData = {
//         toDate: "2026-08-31",
//         shopIds: ["d301b84f-bd84-4768-85f7-2c6357710d02", "7017b78a-7b57-4c9d-86b2-808e9c86601e"],
//         retailerIds: ["60d19b9a-e6eb-4114-82f4-80304ff0b720"],
//     };
//     setFilters(filters);
//   }

//   async function onClickClear() {
//     setFilters(null);
//   }

//   return (
//     <section className="expense_filters">
//       <span>FILTERS</span>
//       <button onClick={onClickApply}>Apply</button>
//       <button onClick={onClickClear}>Clear</button>
//     </section>
//   );
// }

import { useState } from "react";
import { SelectFilter } from "./SelectFilter";
import type {
  ExpenseFiltersData,
  FilterData,
} from "../../types/ExpenseFiltersData";
import type { Currency } from "../../types/Defs";
import { useSearchItems } from "../../hooks/useSearchItems";
import type { GetShopsDto } from "../../../Shops/types/GetShopsDto";
import { useGet } from "../../hooks/useGet";
import { getShops } from "../../../Shops/api/shopsApi";
import type { GetCategoriesDto } from "../../../Categories/types/GetCategoriesDto";
import { getCategories } from "../../../Categories/api/categoriesApi";
import { getRetailers } from "../../../Retailers/api/retailersApi";
import type { GetRetailersDto } from "../../../Retailers/types/GetRetailersDto";

type ExpenseFiltersProps = {
  onApply: (filters: ExpenseFiltersData | null) => void;
};

const currencies: Currency[] = ["NZD", "RUB", "USD"];

export function ExpenseFilters({ onApply }: ExpenseFiltersProps) {
  const [draftFilters, setDraftFilters] = useState<ExpenseFiltersData>({});
  const [clearAllKey, setClearAllKey] = useState(0);

  const [itemSearch, setItemSearch] = useState("");
  const { items } = useSearchItems(itemSearch);

  const { results: shops } = useGet<GetShopsDto>(getShops);
  const { results: categories } = useGet<GetCategoriesDto>(getCategories);
  const { results: retailers } = useGet<GetRetailersDto>(getRetailers);

  const createFilterChangeHandler = (key: keyof ExpenseFiltersData) => {
    return (value: FilterData[]) => {
      setDraftFilters((current) => ({
        ...current,
        [key]: value,
      }));
    };
  };

  const createFilterClearHandler = (key: keyof ExpenseFiltersData) => {
    return () => {
      setDraftFilters((current) => ({
        ...current,
        [key]: [],
      }));
    };
  };

  const handleApply = () => {
    onApply(draftFilters);
  };

  const handleClearAll = () => {
    setDraftFilters({});
    setItemSearch("");
    setClearAllKey((current) => current + 1);
    onApply(null);
  };

  const handleDateChange = (key: "fromDate" | "toDate", value: string) => {
    setDraftFilters((current) => ({
      ...current,
      [key]: value || undefined,
    }));
  };

  const handleAmountChange = (
    key: "fromAmount" | "toAmount",
    value: string,
  ) => {
    setDraftFilters((current) => ({
      ...current,
      [key]: value === "" ? undefined : Number(value),
    }));
  };

  return (
    <div className="expense-filters">
      <SelectFilter
        label="Shop"
        options={shops}
        selected={draftFilters.shops ?? []}
        onChange={createFilterChangeHandler("shops")}
        onClear={createFilterClearHandler("shops")}
        resetKey={clearAllKey}
      />

      <SelectFilter
        label="Category"
        options={categories}
        selected={draftFilters.categories ?? []}
        onChange={createFilterChangeHandler("categories")}
        onClear={createFilterClearHandler("categories")}
        resetKey={clearAllKey}
      />

      <SelectFilter
        label="Item"
        options={items}
        selected={draftFilters.items ?? []}
        onChange={createFilterChangeHandler("items")}
        searchable
        onSearch={setItemSearch}
        onClear={createFilterClearHandler("items")}
        resetKey={clearAllKey}
      />

      <SelectFilter
        label="Retailer"
        options={retailers}
        selected={draftFilters.retailers ?? []}
        onChange={createFilterChangeHandler("retailers")}
        onClear={createFilterClearHandler("retailers")}
        resetKey={clearAllKey}
      />

      <fieldset className="expense-filters__group">
        <legend>Date range</legend>
        <label>
          From
          <input
            type="date"
            value={draftFilters.fromDate ?? ""}
            onChange={(event) =>
              handleDateChange("fromDate", event.target.value)
            }
          />
        </label>
        <label>
          To
          <input
            type="date"
            value={draftFilters.toDate ?? ""}
            onChange={(event) => handleDateChange("toDate", event.target.value)}
          />
        </label>
      </fieldset>

      <fieldset className="expense-filters__group">
        <legend>Amount range</legend>
        <label>
          From
          <input
            type="number"
            min="0"
            step="0.01"
            inputMode="decimal"
            placeholder="0.00"
            value={draftFilters.fromAmount ?? ""}
            onChange={(event) =>
              handleAmountChange("fromAmount", event.target.value)
            }
          />
        </label>
        <label>
          To
          <input
            type="number"
            min="0"
            step="0.01"
            inputMode="decimal"
            placeholder="0.00"
            value={draftFilters.toAmount ?? ""}
            onChange={(event) =>
              handleAmountChange("toAmount", event.target.value)
            }
          />
        </label>
      </fieldset>

      <label className="expense-filters__currency">
        Currency
        <select
          value={draftFilters.currency ?? ""}
          onChange={(event) =>
            setDraftFilters((current) => ({
              ...current,
              currency: (event.target.value || undefined) as
                Currency | undefined,
            }))
          }
        >
          <option value="">All currencies</option>
          {currencies.map((currency) => (
            <option key={currency} value={currency}>
              {currency}
            </option>
          ))}
        </select>
      </label>

      <button type="button" onClick={handleApply}>
        Apply
      </button>
      <button
        className="expense-filters__clear-all"
        type="button"
        onClick={handleClearAll}
      >
        Clear all
      </button>
    </div>
  );
}
export default ExpenseFilters;
