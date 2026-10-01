import "./css/ReportFilters.css";

import { defaultCurrency } from "../../shared/common/consts";
import { currencies, type Currency } from "../../shared/types/Currency";
import type { ReportFiltersData } from "../types/ReportFiltersData";

type ReportFiltersProps = {
  filters: ReportFiltersData;
  onChange: (filters: ReportFiltersData) => void;
};

function ReportFilters({ filters, onChange }: ReportFiltersProps) {
  const handleDateChange = (key: "fromDate" | "toDate", value: string) => {
    onChange({
      ...filters,
      [key]: value || undefined,
    });
  };

  const handleCurrencyChange = (value: Currency) => {
    onChange({
      ...filters,
      currency: value,
    });
  };

  const handleClearFilters = () => {
    onChange({ currency: defaultCurrency });
  };

  return (
    <div className="report-filters">
      <fieldset className="report-filters__group">
        <legend>Date range</legend>
        <label>
          From
          <input
            type="date"
            value={filters.fromDate ?? ""}
            onChange={(event) =>
              handleDateChange("fromDate", event.target.value)
            }
          />
        </label>
        <label>
          To
          <input
            type="date"
            value={filters.toDate ?? ""}
            onChange={(event) => handleDateChange("toDate", event.target.value)}
          />
        </label>
      </fieldset>

      <label className="report-filters__currency">
        Currency
        <select
          value={filters.currency}
          onChange={(event) =>
            handleCurrencyChange(event.target.value as Currency)
          }
        >
          {currencies.map((currency) => (
            <option key={currency} value={currency}>
              {currency}
            </option>
          ))}
        </select>
      </label>

      {/* TODO: Add format report (bars/text/pie) */}

      <button
        className="report-filters__clear"
        type="button"
        onClick={handleClearFilters}
      >
        Clear filters
      </button>
    </div>
  );
}

export default ReportFilters;
