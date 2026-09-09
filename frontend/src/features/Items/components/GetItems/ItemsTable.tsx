import type { GetCategoriesDto } from "../../../Categories/types/GetCategoriesDto";
import "./css/ItemsTable.css";
import type { UpdateItemData } from "../../types/data/ItemData";
import type { ItemValidationErrors } from "../../validation/itemValidation";
import type { ItemDto } from "../../types/dto/ItemDto";
import ItemRow from "./ItemRow";
import type { UnitDto } from "../../../shared/common/units";

export type ItemsTableProps = {
  items: ItemDto[];
  isLoading: boolean;
  error: Error | null;
  onUpdate: (item: ItemDto) => void;
  onDelete: (item: ItemDto) => void;
  categories: GetCategoriesDto[];
  units: UnitDto[];
  updateItemData: UpdateItemData | null;
  updateErrors: ItemValidationErrors;
  onUpdateNameChange: (value: string) => void;
  onUpdateCategoryChange: (value: string) => void;
  onUpdateUnitChange: (value: string) => void;
  onSave: () => void;
  onCancel: () => void;
};

function ItemsTable(props: ItemsTableProps) {
  if (props.isLoading) {
    return <div className="items-table">Loading...</div>;
  }

  if (props.error) {
    return <div className="items-table">Failed to load items.</div>;
  }

  if (props.items.length === 0) {
    return <div className="items-table">You don't have any items yet.</div>;
  }

  return (
    <div className="items-table">
      <div className="items-table__data">
        {props.items.length === 0 ? (
          <p className="items-table__no_items">You don't have any items yet.</p>
        ) : (
          props.items.map((item) => (
            <ItemRow
              key={item.id}
              item={item}
              onUpdate={props.onUpdate}
              onDelete={props.onDelete}
              isUpdating={props.updateItemData?.id === item.id}
              categories={props.categories}
              units={props.units}
              updateItemData={props.updateItemData}
              updateErrors={props.updateErrors}
              onUpdateNameChange={props.onUpdateNameChange}
              onUpdateCategoryChange={props.onUpdateCategoryChange}
              onUpdateUnitChange={props.onUpdateUnitChange}
              onSave={props.onSave}
              onCancel={props.onCancel}
            />
          ))
        )}
      </div>
    </div>
  );
}

export default ItemsTable;
