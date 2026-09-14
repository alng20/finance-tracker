import type { GetCategoriesDto } from "../../../Categories/types/GetCategoriesDto";
import type { UnitDto } from "../../../shared/common/units";
import { SelectFilter } from "../../../shared/components/Filters/SelectFilter";
import type { FilterData } from "../../../shared/types/FilterData";
import type { ItemFiltersData } from "../../types/data/ItemFiltersData";
import "./css/ItemFilters.css";

type ItemFiltersProps = {
  categories: GetCategoriesDto[];
  units: UnitDto[];
  filters: ItemFiltersData | null;
  onChange: (filters: ItemFiltersData | null) => void;
  onPageReset: () => void;
};

function ItemFilters({
  categories,
  units,
  filters,
  onChange,
  onPageReset,
}: ItemFiltersProps) {
  const clearAllFilters = () => {
    onPageReset();
    onChange(null);
  };

  const updateCategories = (selected: FilterData[]) => {
    onPageReset();
    onChange({ ...filters, categories: selected });
  };

  const updateUnits = (selected: FilterData[]) => {
    onPageReset();
    onChange({ ...filters, units: selected });
  };

  const removeCategory = (categoryId: string) => {
    onPageReset();
    onChange({
      ...filters,
      categories: filters?.categories?.filter(
        (category) => category.id !== categoryId,
      ),
    });
  };

  const removeUnit = (unitId: string) => {
    onPageReset();
    onChange({
      ...filters,
      units: filters?.units?.filter((unit) => unit.id !== unitId),
    });
  };

  return (
    <div className="item-filters-content">
      <div className="items-filters">
        <SelectFilter
          label="Category"
          options={categories}
          selected={filters?.categories ?? []}
          onChange={updateCategories}
          onClear={() => updateCategories([])}
        />
        <SelectFilter
          label="Unit"
          options={units}
          selected={filters?.units ?? []}
          onChange={updateUnits}
          onClear={() => updateUnits([])}
        />
      </div>

      {filters?.categories?.length || filters?.units?.length ? (
        <div className="items-selected-filters" aria-label="Selected filters">
          <span>Selected filters:</span>
          {filters.categories?.map((category) => (
            <button
              type="button"
              key={`category-${category.id}`}
              onClick={() => removeCategory(category.id)}
              aria-label={`Remove category filter ${category.name}`}
            >
              <span className="items-selected-filters__type">Category</span>
              <span>{category.name}</span>
              <span
                className="items-selected-filters__remove"
                aria-hidden="true"
              >
                x
              </span>
            </button>
          ))}
          {filters.units?.map((unit) => (
            <button
              type="button"
              key={`unit-${unit.id}`}
              onClick={() => removeUnit(unit.id)}
              aria-label={`Remove unit filter ${unit.name}`}
            >
              <span className="items-selected-filters__type">Unit</span>
              <span>{unit.name}</span>
              <span
                className="items-selected-filters__remove"
                aria-hidden="true"
              >
                x
              </span>
            </button>
          ))}
          <button
            type="button"
            className="items-selected-filters__clear-all"
            onClick={clearAllFilters}
          >
            Clear all
          </button>
        </div>
      ) : null}
    </div>
  );
}

export default ItemFilters;
