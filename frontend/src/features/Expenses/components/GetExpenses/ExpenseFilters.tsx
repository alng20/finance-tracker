import "./css/ExpenseFilters.css";

import { useState } from "react";

import { getCategories } from "../../../Categories/api/categoriesApi";
import type { GetCategoriesDto } from "../../../Categories/types/GetCategoriesDto";
import { searchItems } from "../../../Items/api/itemsApi";
import type { SearchItemsDto } from "../../../Items/types/dto/SearchItemsDto";
import { getRetailers } from "../../../Retailers/api/retailersApi";
import type { GetRetailersDto } from "../../../Retailers/types/GetRetailersDto";
import { SelectFilter } from "../../../shared/components/Filters/SelectFilter";
import { useGet } from "../../../shared/hooks/useGet";
import useGetWithFilters from "../../../shared/hooks/useGetWithFilters";
import { useSearch } from "../../../shared/hooks/useSearch";
import { currencies, type Currency } from "../../../shared/types/Currency";
import type { FilterData } from "../../../shared/types/FilterData";
import { getShops } from "../../../Shops/api/shopsApi";
import type { ShopFiltersData } from "../../../Shops/types/data/ShopFiltersData";
import type { GetShopsDto } from "../../../Shops/types/dto/GetShopsDto";
import type { GetShopsResponse } from "../../../Shops/types/responses/GetShopsResponse";
import type { ExpenseFiltersData } from "../../types/ExpenseFiltersData";
import {
  type ExpensesSortType,
  expensesSortTypes,
} from "../../types/ExpensesSortType";

type ExpenseFiltersProps = {
  onApply: (filters: ExpenseFiltersData | null) => void;
};

export function ExpenseFilters({ onApply }: ExpenseFiltersProps) {
  const [draftFilters, setDraftFilters] = useState<ExpenseFiltersData>({});
  const [clearAllKey, setClearAllKey] = useState(0);

  const [itemSearch, setItemSearch] = useState("");
  const { results: items } = useSearch<SearchItemsDto, FilterData>(
    itemSearch,
    null,
    searchItems,
    (searchResults) =>
      searchResults.map((item) => ({
        id: item.id,
        name: item.name,
      })),
  );

  const { results: shops } = useGetWithFilters<
    GetShopsResponse,
    ShopFiltersData,
    GetShopsDto
  >({ pageNumber: 1, pageSize: 50 }, null, getShops);
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
    <div className="expense-filters" key={clearAllKey}>
      <SelectFilter
        label="Shop"
        options={shops}
        selected={draftFilters.shops ?? []}
        onChange={createFilterChangeHandler("shops")}
        onClear={createFilterClearHandler("shops")}
      />

      <SelectFilter
        label="Category"
        options={categories}
        selected={draftFilters.categories ?? []}
        onChange={createFilterChangeHandler("categories")}
        onClear={createFilterClearHandler("categories")}
      />

      <SelectFilter
        label="Item"
        options={items}
        selected={draftFilters.items ?? []}
        onChange={createFilterChangeHandler("items")}
        searchable
        onSearch={setItemSearch}
        onClear={createFilterClearHandler("items")}
      />

      <SelectFilter
        label="Retailer"
        options={retailers}
        selected={draftFilters.retailers ?? []}
        onChange={createFilterChangeHandler("retailers")}
        onClear={createFilterClearHandler("retailers")}
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

      {/* TODO: Add to .css */}
      <label className="expense-filters__sort_type">
        Sort by
        <select
          value={draftFilters.sortType ?? ""}
          onChange={(event) =>
            setDraftFilters((current) => ({
              ...current,
              sortType: (event.target.value || undefined) as
                ExpensesSortType | undefined,
            }))
          }
        >
          {expensesSortTypes.map((sortType) => (
            <option key={sortType} value={sortType}>
              {sortType}
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
