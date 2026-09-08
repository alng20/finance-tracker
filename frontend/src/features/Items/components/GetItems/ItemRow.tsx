import type { ItemDto } from "../../../Expenses/types/GetItemsResponse";
import type { GetCategoriesDto } from "../../../Categories/types/GetCategoriesDto";
import "./css/ItemRow.css";
import type { UpdateItemData } from "../../types/ItemData";
import type { FilterData } from "../../../shared/types/FilterData";

export type ItemFormErrors = {
  name?: string;
  category?: string;
  unit?: string;
};

type ItemRowProps = {
  item: ItemDto;
  onUpdate: (item: ItemDto) => void;
  onDelete: (item: ItemDto) => void;
  isUpdating: boolean;
  categories: GetCategoriesDto[];
  units: FilterData[]; // TODO: fix it
  updateItemData: UpdateItemData | null;
  updateErrors: ItemFormErrors;
  onUpdateNameChange: (value: string) => void;
  onUpdateCategoryChange: (value: string) => void;
  onUpdateUnitChange: (value: string) => void;
  onSave: () => void;
  onCancel: () => void;
};

function ItemRow({
  item,
  onUpdate,
  onDelete,
  isUpdating,
  categories,
  units,
  updateItemData,
  updateErrors,
  onUpdateNameChange,
  onUpdateCategoryChange,
  onUpdateUnitChange,
  onSave,
  onCancel,
}: ItemRowProps) {
  return (
    <div className="items-row">
      <div className="item-row__name">
        <span className="item-row__label">Name</span>
        {isUpdating ? (
          <>
            <input
              aria-invalid={Boolean(updateErrors.name)}
              value={updateItemData?.name}
              onChange={(event) => onUpdateNameChange(event.target.value)}
            />
            {updateErrors.name && (
              <small className="item-row__error">{updateErrors.name}</small>
            )}
          </>
        ) : (
          <strong>{item.name}</strong>
        )}
      </div>
      <div className="item-row__category">
        <span className="item-row__label">Category</span>
        {isUpdating ? (
          <>
            <select
              aria-invalid={Boolean(updateErrors.category)}
              value={updateItemData?.categoryId}
              onChange={(event) => onUpdateCategoryChange(event.target.value)}
            >
              <option value="">Select category</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              ))}
            </select>
            {updateErrors.category && (
              <small className="item-row__error">{updateErrors.category}</small>
            )}
          </>
        ) : (
          <strong>{item.categoryName}</strong>
        )}
      </div>
      <div className="item-row__unit">
        <span className="item-row__label">Unit</span>
        {isUpdating ? (
          <>
            <select
              aria-invalid={Boolean(updateErrors.unit)}
              value={updateItemData?.unit}
              onChange={(event) => onUpdateUnitChange(event.target.value)}
            >
              <option value="">Select unit</option>
              {units.map((unit) => (
                <option key={unit.id} value={unit.id}>
                  {unit.name}
                </option>
              ))}
            </select>
            {updateErrors.unit && (
              <small className="item-row__error">{updateErrors.unit}</small>
            )}
          </>
        ) : (
          <strong>{item.unit}</strong>
        )}
      </div>
      <div className="item-row__actions">
        {isUpdating ? (
          <>
            <button type="button" onClick={onSave}>
              Save
            </button>
            <button type="button" onClick={onCancel}>
              Cancel
            </button>
          </>
        ) : (
          <>
            <button type="button" onClick={() => onUpdate(item)}>
              Edit
            </button>
            <button type="button" onClick={() => onDelete(item)}>
              Delete
            </button>
          </>
        )}
      </div>
    </div>
  );
}

export default ItemRow;
