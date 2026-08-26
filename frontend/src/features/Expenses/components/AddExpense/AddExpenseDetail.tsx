import "./css/AddExpenseDetail.css";

import { useState, useEffect } from "react";
import { getCategories } from "../../../Categories/api/categoriesApi";
import { searchItems } from "../../../Items/api/searchItems";
import type { Category } from "../../../Categories/types/Category";
import type { Item } from "../../../Items/types/Item";
import type { ExpenseDetail } from "../../types/ExpenseDetail";
import { useSearchItems } from "./helpers/useSearchItems";

import { validateExpenseDetail } from "./validation/expenseValidation";
import type { ExpenseDetailValidationErrors } from "./validation/expenseValidation";

type AddExpenseDetailProps = {
  initialDetail?: ExpenseDetail;
  onSave: (detail: ExpenseDetail) => void;
  onCancel: () => void;
};

function AddExpenseDetail({
  initialDetail,
  onSave,
  onCancel,
}: AddExpenseDetailProps) {
  const [name, setName] = useState(initialDetail?.itemName ?? "");
  const [category, setCategory] = useState<Category | null>(null);
  const [quantity, setQuantity] = useState(
    initialDetail ? String(initialDetail.quantity) : "",
  );
  const [unit, setUnit] = useState(initialDetail?.unit ?? "");
  const [discount, setDiscount] = useState(
    initialDetail ? String(initialDetail.discount) : "",
  );
  const [price, setPrice] = useState(
    initialDetail ? String(initialDetail.price) : "",
  );

  const [categories, setCategories] = useState<Category[]>([]);

  const [selectedItem, setSelectedItem] = useState<Item | null>(null);
  const { items, clearItems } = useSearchItems(name, selectedItem);

  const [validationErrors, setValidationErrors] =
    useState<ExpenseDetailValidationErrors>({});

  useEffect(() => {
    async function loadCategories() {
      const data = await getCategories();
      setCategories(data);
      if (initialDetail) {
        const selectedCategory = data.find(
          (category) => category.name === initialDetail.categoryName,
        );

        setCategory(selectedCategory ?? null);
      }
    }

    loadCategories();
  }, []);

  {
    /* TODO: move to showEditedItem */
  }
  useEffect(() => {
    if (!initialDetail) {
      return;
    }

    async function loadInitialItem() {
      const data = await searchItems(initialDetail!.itemName);

      const item = data.find((item) => item.id === initialDetail!.itemId);

      if (item) {
        setSelectedItem(item);
      }
    }

    loadInitialItem();
  }, [initialDetail]);

  function saveDetail() {
    if (!selectedItem && !initialDetail) {
      return;
    }

    const detail: ExpenseDetail = {
      itemId: selectedItem?.id ?? initialDetail!.itemId,
      itemName: selectedItem?.name ?? initialDetail!.itemName,
      categoryName: category?.name ?? "Other",
      quantity: Number(quantity),
      unit: unit,
      discount: Number(discount),
      price: Number(price),
    };

    const errors = validateExpenseDetail(detail);

    if (Object.keys(errors).length > 0) {
      setValidationErrors(errors);
      return;
    } else {
      setValidationErrors({});
    }

    onSave(detail);
  }

  return (
    <div className="expense-detail-row-form">
      <div className="expense-detail-row-form__top">
        <div className="expense-detail-row-form__fields">
          <div className="detail-field detail-field--search">
            <input
              id="name"
              type="text"
              placeholder="name"
              value={name}
              onChange={(event) => {
                setName(event.target.value);
                setSelectedItem(null);
              }}
            />
            {validationErrors.itemId && (
              <span className="add-expense-detail__field_error">
                {validationErrors.itemId}
              </span>
            )}

            {/* TODO: move to selectSearchItem */}
            {items.length > 0 && (
              <div className="item-search__results">
                {items.map((item) => (
                  <button
                    key={item.id}
                    type="button"
                    onClick={() => {
                      setSelectedItem(item);
                      setName(item.name);

                      const selectedCategory = categories.find(
                        (category) => category.id === item.categoryId,
                      );

                      setCategory(selectedCategory ?? null);
                      clearItems();
                    }}
                  >
                    {item.name}
                  </button>
                ))}
              </div>
            )}

            {validationErrors.itemId && (
              <span className="add-expense-detail__field_error">
                {validationErrors.itemId}
              </span>
            )}
          </div>

          {/* TODO: move to selectCategory */}
          <div className="detail-field">
            <select
              name="category"
              value={category?.id ?? ""}
              onChange={(event) => {
                if (category && selectedItem) {
                  return;
                }
                const selectedCategory = categories.find(
                  (category) => category.id === event.target.value,
                );

                setCategory(selectedCategory ?? null);
              }}
            >
              <option value="" disabled>
                Category
              </option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              ))}
            </select>
            {validationErrors.categoryName && (
              <span className="add-expense-detail__field_error">
                {validationErrors.categoryName}
              </span>
            )}
          </div>

          <div className="detail-field">
            <input
              id="quantity"
              placeholder="Quantity"
              type="number"
              value={quantity}
              onChange={(event) => {
                setQuantity(event.target.value);
              }}
            />
            {validationErrors.quantity && (
              <span className="add-expense-detail__field_error">
                {validationErrors.quantity}
              </span>
            )}
          </div>
          <div className="detail-field">
            {/* TODO: move to selectUnit and use GET api/units */}
            <select
              name="unit"
              value={unit}
              onChange={(event) => {
                setUnit(event.target.value);
              }}
            >
              <option value="" disabled>
                Unit
              </option>
              <option value="KG">KG</option>
              <option value="piece">piece</option>
              <option value="G">G</option>
            </select>
            {validationErrors.unit && (
              <span className="add-expense-detail__field_error">
                {validationErrors.unit}
              </span>
            )}
          </div>

          <div className="detail-field">
            <input
              id="detail-discount"
              placeholder="Discount, %"
              type="number"
              value={discount}
              onChange={(event) => {
                setDiscount(event.target.value);
              }}
            />
            {validationErrors.discount && (
              <span className="add-expense-detail__field_error">
                {validationErrors.discount}
              </span>
            )}
          </div>
          <div className="detail-field">
            <input
              id="detail-price"
              placeholder="Price"
              type="number"
              value={price}
              onChange={(event) => {
                setPrice(event.target.value);
              }}
            />
            {validationErrors.price && (
              <span className="add-expense-detail__field_error">
                {validationErrors.price}
              </span>
            )}
          </div>
        </div>

        <button
          type="button"
          className="expense-detail-row-form__close"
          onClick={onCancel}
        >
          X
        </button>
      </div>

      <div className="expense-detail-row-form__actions">
        <button
          type="button"
          className="expense-detail-row-form__save"
          onClick={saveDetail}
        >
          Save detail
        </button>
      </div>
    </div>
  );
}

export default AddExpenseDetail;
