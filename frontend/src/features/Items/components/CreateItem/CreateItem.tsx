import "./css/CreateItem.css";

import type { GetCategoriesDto } from "../../../Categories/types/GetCategoriesDto";
import type { ItemFormErrors } from "../GetItems/ItemRow";
import type { CreateItemData } from "../../types/ItemData";
import type { FilterData } from "../../../shared/types/FilterData";

type CreateItemProps = {
  categories: GetCategoriesDto[];
  units: FilterData[]; // TODO: fix it
  createItemData: CreateItemData | null;
  errors: ItemFormErrors;
  onNameChange: (value: string) => void;
  onCategoryChange: (value: string) => void;
  onUnitChange: (value: string) => void;
  onSubmit: () => void;
  onCancel: () => void;
};

function CreateItem({
  categories,
  units,
  createItemData,
  errors,
  onNameChange,
  onCategoryChange,
  onUnitChange,
  onSubmit,
  onCancel,
}: CreateItemProps) {
  return (
    <form
      className="item-create-form"
      onSubmit={(event) => {
        event.preventDefault();
        onSubmit();
      }}
    >
      <input
        autoFocus
        aria-invalid={Boolean(errors.name)}
        placeholder="Item name"
        value={createItemData?.name}
        onChange={(event) => onNameChange(event.target.value)}
      />
      {errors.name && <p className="item-form-error">{errors.name}</p>}
      <select
        aria-invalid={Boolean(errors.category)}
        value={createItemData?.categoryId}
        onChange={(event) => onCategoryChange(event.target.value)}
      >
        <option value="">Select category</option>
        {categories.map((category) => (
          <option key={category.id} value={category.id}>
            {category.name}
          </option>
        ))}
      </select>
      {errors.category && <p className="item-form-error">{errors.category}</p>}
      <select
        aria-invalid={Boolean(errors.unit)}
        value={createItemData?.unit}
        onChange={(event) => onUnitChange(event.target.value)}
      >
        <option value="">Select unit</option>
        {units.map((itemUnit) => (
          <option key={itemUnit.id} value={itemUnit.id}>
            {itemUnit.name}
          </option>
        ))}
      </select>
      {errors.unit && <p className="item-form-error">{errors.unit}</p>}
      <button type="submit">Create</button>
      <button type="button" onClick={onCancel}>
        Cancel
      </button>
    </form>
  );
}

export default CreateItem;
