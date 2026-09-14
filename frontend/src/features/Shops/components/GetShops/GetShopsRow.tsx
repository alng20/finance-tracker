import "./css/GetShopsRow.css";

import type { GetShopsDto } from "../../types/dto/GetShopsDto";
import type { ShopValidationErrors } from "../../validation/shopValidation";

type GetShopsRowProps = {
  shop: GetShopsDto;
  retailers: { id: string; name: string }[];
  onUpdate: (shop: GetShopsDto) => void;
  onDelete: (shop: GetShopsDto) => void;
  isUpdating: boolean;
  updateShopData: {
    id: string;
    name: string;
    retailerId: string;
    country: string;
    city: string;
  } | null;
  updateErrors: ShopValidationErrors;
  onUpdateNameChange: (value: string) => void;
  onUpdateRetailerChange: (value: string) => void;
  onUpdateCountryChange: (value: string) => void;
  onUpdateCityChange: (value: string) => void;
  onSave: () => void;
  onCancel: () => void;
};

function GetShopsRow({
  shop,
  retailers,
  onUpdate,
  onDelete,
  isUpdating,
  updateShopData,
  updateErrors,
  onUpdateNameChange,
  onUpdateRetailerChange,
  onUpdateCountryChange,
  onUpdateCityChange,
  onSave,
  onCancel,
}: GetShopsRowProps) {
  return (
    <div className="shops-row">
      <div className="shop-row__name">
        <span className="shop-row__label">Name</span>
        {isUpdating ? (
          <>
            <input
              aria-invalid={Boolean(updateErrors.name)}
              value={updateShopData?.name}
              onChange={(event) => onUpdateNameChange(event.target.value)}
            />
            {updateErrors.name && (
              <small className="shop-row__error">{updateErrors.name}</small>
            )}
          </>
        ) : (
          <strong>{shop.name}</strong>
        )}
      </div>

      <div className="shop-row__retailer">
        <span className="shop-row__label">Retailer</span>
        {isUpdating ? (
          <>
            <select
              aria-invalid={Boolean(updateErrors.retailer)}
              value={updateShopData?.retailerId}
              onChange={(event) => onUpdateRetailerChange(event.target.value)}
            >
              <option value="">Select retailer</option>
              {retailers.map((retailer) => (
                <option key={retailer.id} value={retailer.id}>
                  {retailer.name}
                </option>
              ))}
            </select>
            {updateErrors.retailer && (
              <small className="shop-row__error">{updateErrors.retailer}</small>
            )}
          </>
        ) : (
          <strong>{shop.retailerName ?? "-"}</strong>
        )}
      </div>

      <div className="shop-row__country">
        <span className="shop-row__label">Country</span>
        {isUpdating ? (
          <>
            <select
              aria-invalid={Boolean(updateErrors.country)}
              value={updateShopData?.country}
              onChange={(event) => onUpdateCountryChange(event.target.value)}
            >
              <option value="">Select country</option>
              <option value="New Zealand">New Zealand</option>
            </select>
            {updateErrors.country && (
              <small className="shop-row__error">{updateErrors.country}</small>
            )}
          </>
        ) : (
          <strong>{shop.country ?? "-"}</strong>
        )}
      </div>

      <div className="shop-row__city">
        <span className="shop-row__label">City</span>
        {isUpdating ? (
          <>
            <select
              aria-invalid={Boolean(updateErrors.city)}
              value={updateShopData?.city}
              onChange={(event) => onUpdateCityChange(event.target.value)}
            >
              <option value="">Select city</option>
              <option value="Wellington">Wellington</option>
            </select>
            {updateErrors.city && (
              <small className="shop-row__error">{updateErrors.city}</small>
            )}
          </>
        ) : (
          <strong>{shop.city ?? "-"}</strong>
        )}
      </div>

      <div className="shop-row__actions">
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
            <button type="button" onClick={() => onUpdate(shop)}>
              Edit
            </button>
            <button type="button" onClick={() => onDelete(shop)}>
              Delete
            </button>
          </>
        )}
      </div>
    </div>
  );
}

export default GetShopsRow;
