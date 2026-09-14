import "./css/CreateShop.css";

import type { GetRetailersDto } from "../../../Retailers/types/GetRetailersDto";
import type { CreateShopData } from "../../types/data/ShopData";
import type { ShopValidationErrors } from "../../validation/shopValidation";

type CreateShopProps = {
  retailers: GetRetailersDto[];
  createShopData: CreateShopData | null;
  errors: ShopValidationErrors;
  submitError: string | null;
  onNameChange: (value: string) => void;
  onRetailerChange: (value: string) => void;
  onCountryChange: (value: string) => void;
  onCityChange: (value: string) => void;
  onSubmit: () => void;
  onCancel: () => void;
};

function CreateShop({
  retailers,
  createShopData,
  errors,
  submitError,
  onNameChange,
  onRetailerChange,
  onCountryChange,
  onCityChange,
  onSubmit,
  onCancel,
}: CreateShopProps) {
  return (
    <form
      className="shop-create-form"
      onSubmit={(event) => {
        event.preventDefault();
        onSubmit();
      }}
    >
      {submitError && (
        <p className="shop-form-error shop-form-error--global">{submitError}</p>
      )}
      <div className="shop-form-field">
        <input
          autoFocus
          aria-invalid={Boolean(errors.name)}
          placeholder="Shop name"
          value={createShopData?.name}
          onChange={(event) => onNameChange(event.target.value)}
        />
        {errors.name && <p className="shop-form-error">{errors.name}</p>}
      </div>
      <div className="shop-form-field">
        <select
          aria-invalid={Boolean(errors.retailer)}
          value={createShopData?.retailerId}
          onChange={(event) => onRetailerChange(event.target.value)}
        >
          <option value="">Select retailer</option>
          {retailers.map((retailer) => (
            <option key={retailer.id} value={retailer.id}>
              {retailer.name}
            </option>
          ))}
        </select>
        {errors.retailer && (
          <p className="shop-form-error">{errors.retailer}</p>
        )}
      </div>
      <div className="shop-form-field">
        <select
          aria-invalid={Boolean(errors.country)}
          value={createShopData?.country}
          onChange={(event) => onCountryChange(event.target.value)}
        >
          {/* TODO: Make it possible to write */}
          <option value="">Select country</option>
          <option value="New Zealand">New Zealand</option>
        </select>
        {errors.country && <p className="shop-form-error">{errors.country}</p>}
      </div>
      <div className="shop-form-field">
        <select
          aria-invalid={Boolean(errors.city)}
          value={createShopData?.city}
          onChange={(event) => onCityChange(event.target.value)}
        >
          <option value="">Select city</option>
          <option value="Wellington">Wellington</option>
        </select>
        {errors.city && <p className="shop-form-error">{errors.city}</p>}
      </div>
      <button type="submit">Create</button>
      <button type="button" onClick={onCancel}>
        Cancel
      </button>
    </form>
  );
}

export default CreateShop;
