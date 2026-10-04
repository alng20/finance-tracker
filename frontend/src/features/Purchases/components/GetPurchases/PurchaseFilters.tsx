import "../../../Expenses/components/GetExpenses/css/ExpenseFilters.css";
import "./css/PurchaseFilters.css";

import { useState } from "react";

import { getCategories } from "../../../Categories/api/categoriesApi";
import type { GetCategoriesDto } from "../../../Categories/types/GetCategoriesDto";
import { SelectFilter } from "../../../shared/components/Filters/SelectFilter";
import { useGet } from "../../../shared/hooks/useGet";
import type { FilterData } from "../../../shared/types/FilterData";
import type { PurchaseFiltersData } from "../../types/PurchaseFiltersData";

type PurchaseFiltersProps = {
  search: string;
  onSearchChange: (search: string) => void;
  onApply: (filters: PurchaseFiltersData | null) => void;
};

function PurchaseFilters({
  search,
  onSearchChange,
  onApply,
}: PurchaseFiltersProps) {
  const [draftFilters, setDraftFilters] = useState<PurchaseFiltersData>({});
  const [clearAllKey, setClearAllKey] = useState(0);

  const { results: categories } = useGet<GetCategoriesDto>(getCategories);

  const hasInvalidPeriod =
    !!draftFilters.fromDate &&
    !!draftFilters.toDate &&
    draftFilters.toDate <= draftFilters.fromDate;

  const handleCategoriesChange = (value: FilterData[]) => {
    setDraftFilters((current) => ({ ...current, categories: value }));
  };

  const handleDateChange = (key: "fromDate" | "toDate", value: string) => {
    setDraftFilters((current) => ({ ...current, [key]: value || undefined }));
  };

  const handleApply = () => {
    if (hasInvalidPeriod) {
      return;
    }

    onApply(draftFilters);
  };

  const handleClearAll = () => {
    setDraftFilters({});
    setClearAllKey((current) => current + 1);
    onSearchChange("");
    onApply(null);
  };

  return (
    <form
      className="expense-filters purchase-filters"
      onSubmit={(event) => {
        event.preventDefault();
        handleApply();
      }}
    >
      <div className="purchase-filters__row" key={clearAllKey}>
        <fieldset className="expense-filters__group">
          <legend>Date</legend>
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
              onChange={(event) =>
                handleDateChange("toDate", event.target.value)
              }
            />
          </label>
          {hasInvalidPeriod && (
            <p className="purchase-filters__error" role="alert">
              "To" date must be after "From" date.
            </p>
          )}
        </fieldset>

        <SelectFilter
          label="Category"
          options={categories}
          selected={draftFilters.categories ?? []}
          onChange={handleCategoriesChange}
          onClear={() => handleCategoriesChange([])}
        />

        <button type="submit" disabled={hasInvalidPeriod}>
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

      <label className="purchase-filters__search">
        Search
        <input
          type="search"
          placeholder="Search by name"
          value={search}
          onChange={(event) => onSearchChange(event.target.value)}
        />
      </label>
    </form>
  );
}

export default PurchaseFilters;
