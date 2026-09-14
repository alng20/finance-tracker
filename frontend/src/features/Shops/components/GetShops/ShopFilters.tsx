import type { GetRetailersDto } from "../../../Retailers/types/GetRetailersDto";
import { SelectFilter } from "../../../shared/components/Filters/SelectFilter";
import type { FilterData } from "../../../shared/types/FilterData";
import type { ShopFiltersData } from "../../types/data/ShopFiltersData";
import "./css/ShopFilters.css";

type ShopFiltersProps = {
  retailers: GetRetailersDto[];
  filters: ShopFiltersData | null;
  onChange: (filters: ShopFiltersData | null) => void;
  onPageReset: () => void;
};

function ShopFilters({
  retailers,
  filters,
  onChange,
  onPageReset,
}: ShopFiltersProps) {
  const clearAllFilters = () => {
    onPageReset();
    onChange(null);
  };

  const updateRetailers = (selected: FilterData[]) => {
    onPageReset();
    onChange({ ...filters, retailers: selected });
  };

  const updateCountries = (selected: FilterData[]) => {
    onPageReset();
    onChange({ ...filters, countries: selected.map((item) => item.name) });
  };

  const updateCities = (selected: FilterData[]) => {
    onPageReset();
    onChange({ ...filters, cities: selected.map((item) => item.name) });
  };

  const removeRetailer = (retailerId: string) => {
    onPageReset();
    onChange({
      ...filters,
      retailers: filters?.retailers?.filter(
        (retailer) => retailer.id !== retailerId,
      ),
    });
  };

  const removeCountry = (countryName: string) => {
    onPageReset();
    onChange({
      ...filters,
      countries: filters?.countries?.filter(
        (country) => country !== countryName,
      ),
    });
  };

  const removeCity = (cityName: string) => {
    onPageReset();
    onChange({
      ...filters,
      cities: filters?.cities?.filter((city) => city !== cityName),
    });
  };

  // TODO: Create GET request or move to separate file
  const countryOptions: FilterData[] = [
    { id: "New Zealand", name: "New Zealand" },
  ];
  const cityOptions: FilterData[] = [{ id: "Wellington", name: "Wellington" }];

  return (
    <div className="shop-filters-content">
      <div className="shops-filters">
        <SelectFilter
          label="Retailer"
          options={retailers}
          selected={filters?.retailers ?? []}
          onChange={updateRetailers}
          onClear={() => updateRetailers([])}
        />

        <SelectFilter
          label="Country"
          options={countryOptions}
          selected={
            filters?.countries?.map((country) => ({
              id: country,
              name: country,
            })) ?? []
          }
          onChange={updateCountries}
          onClear={() => updateCountries([])}
        />

        <SelectFilter
          label="City"
          options={cityOptions}
          selected={
            filters?.cities?.map((city) => ({
              id: city,
              name: city,
            })) ?? []
          }
          onChange={updateCities}
          onClear={() => updateCities([])}
        />
      </div>

      {filters?.retailers?.length ||
      filters?.countries?.length ||
      filters?.cities?.length ? (
        <div className="shops-selected-filters" aria-label="Selected filters">
          <span>Selected filters:</span>
          {filters.retailers?.map((retailer) => (
            <button
              type="button"
              key={`retailer-${retailer.id}`}
              onClick={() => removeRetailer(retailer.id)}
              aria-label={`Remove retailer filter ${retailer.name}`}
            >
              <span className="shops-selected-filters__type">Retailer</span>
              <span>{retailer.name}</span>
              <span
                className="shops-selected-filters__remove"
                aria-hidden="true"
              >
                x
              </span>
            </button>
          ))}
          {filters.countries?.map((country) => (
            <button
              type="button"
              key={`country-${country}`}
              onClick={() => removeCountry(country)}
              aria-label={`Remove country filter ${country}`}
            >
              <span className="shops-selected-filters__type">Country</span>
              <span>{country}</span>
              <span
                className="shops-selected-filters__remove"
                aria-hidden="true"
              >
                x
              </span>
            </button>
          ))}
          {filters.cities?.map((city) => (
            <button
              type="button"
              key={`city-${city}`}
              onClick={() => removeCity(city)}
              aria-label={`Remove city filter ${city}`}
            >
              <span className="shops-selected-filters__type">City</span>
              <span>{city}</span>
              <span
                className="shops-selected-filters__remove"
                aria-hidden="true"
              >
                x
              </span>
            </button>
          ))}
          <button
            type="button"
            className="shops-selected-filters__clear-all"
            onClick={clearAllFilters}
          >
            Clear all
          </button>
        </div>
      ) : null}
    </div>
  );
}

export default ShopFilters;
