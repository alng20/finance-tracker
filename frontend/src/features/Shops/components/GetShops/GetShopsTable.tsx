import "./css/GetShopsTable.css";

import type { GetShopsDto } from "../../types/dto/GetShopsDto";
import GetShopsRow from "./GetShopsRow";

type GetShopsTableProps = {
  shops: GetShopsDto[];
  isLoading: boolean;
  error: Error | null;
  onUpdate: (shop: GetShopsDto) => void;
  onDelete: (shop: GetShopsDto) => void;
  updateShopData: {
    id: string;
    name: string;
    retailerId: string;
    country: string;
    city: string;
  } | null;
  updateErrors: Record<string, string>;
  onUpdateNameChange: (value: string) => void;
  onUpdateRetailerChange: (value: string) => void;
  onUpdateCountryChange: (value: string) => void;
  onUpdateCityChange: (value: string) => void;
  onSave: () => void;
  onCancel: () => void;
  retailers: { id: string; name: string }[];
};

function GetShopsTable(props: GetShopsTableProps) {
  if (props.isLoading) {
    return <div className="shops-table">Loading...</div>;
  }

  if (props.error) {
    return <div className="shops-table">Failed to load shops.</div>;
  }

  if (props.shops.length === 0) {
    return <div className="shops-table">You don't have any shops yet.</div>;
  }

  return (
    <div className="shops-table">
      <div className="shops-table__data">
        {props.shops.map((shop) => (
          <GetShopsRow
            key={shop.id}
            shop={shop}
            retailers={props.retailers}
            onUpdate={props.onUpdate}
            onDelete={props.onDelete}
            isUpdating={props.updateShopData?.id === shop.id}
            updateShopData={props.updateShopData}
            updateErrors={props.updateErrors}
            onUpdateNameChange={props.onUpdateNameChange}
            onUpdateRetailerChange={props.onUpdateRetailerChange}
            onUpdateCountryChange={props.onUpdateCountryChange}
            onUpdateCityChange={props.onUpdateCityChange}
            onSave={props.onSave}
            onCancel={props.onCancel}
          />
        ))}
      </div>
    </div>
  );
}

export default GetShopsTable;
