import "./css/AddExpenseDetail.css";

import { useState, useEffect } from "react";
import { getCategories } from "../../api/categoriesApi";
import { searchItems } from "../../api/searchItems";
import type { Category } from "../../types/Category";
import type { Item } from "../../types/Item";

export type ExpenseDetail = {
  itemId: string;
  itemName: string;
  categoryName: string;
  quantity: number;
  unit: string;
  discount: number;
  price: number;
};

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
  function findCategoryByName(categoryName: string | null) {
    if (!categoryName) {
      return null;
    }
    const selectedCategory = categories.find(
      (category) => category.name === categoryName,
    );
    return selectedCategory ?? null;
  }

  const [searchName, setSearchName] = useState(initialDetail?.itemName ?? "");
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
  const [items, setItems] = useState<Item[]>([]);
  const [selectedItem, setSelectedItem] = useState<Item | null>(null);

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

  useEffect(() => {
    if (selectedItem) {
      setItems([]);
      return;
    }

    if (!searchName.trim()) {
      setItems([]);
      return;
    }

    const timer = setTimeout(() => {
      async function loadItems() {
        const data = await searchItems(searchName);

        setItems(data);
      }

      loadItems();
    }, 300);

    return () => {
      clearTimeout(timer);
    };
  }, [searchName, selectedItem]);

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

    const item: ExpenseDetail = {
      itemId: selectedItem?.id ?? initialDetail!.itemId,
      itemName: selectedItem?.name ?? initialDetail!.itemName,
      categoryName: category?.name ?? "Other",
      quantity: Number(quantity),
      unit: unit,
      discount: Number(discount),
      price: Number(price),
    };

    onSave(item);
  }
  const [category, setCategory] = useState<Category | null>(
    findCategoryByName(initialDetail?.categoryName ?? null),
  );

  return (
    <div className="expense-detail-row-form">
      <div className="expense-detail-row-form__top">
        <div className="expense-detail-row-form__fields">
          <input
            id="name"
            type="text"
            placeholder="Name"
            value={searchName}
            onChange={(event) => {
              setSearchName(event.target.value);
              setSelectedItem(null);
            }}
          />

          {items.length > 0 && (
            <div className="item-search__results">
              {items.map((item) => (
                <button
                  key={item.id}
                  type="button"
                  onClick={() => {
                    setSelectedItem(item);
                    setSearchName(item.name);

                    const selectedCategory = categories.find(
                      (category) => category.id === item.categoryId,
                    );
                    setCategory(selectedCategory ?? null);

                    setItems([]);
                  }}
                >
                  {item.name}
                </button>
              ))}
            </div>
          )}
          <select
            name="category"
            defaultValue=""
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

          <input
            id="quantity"
            placeholder="Quantity"
            type="number"
            value={quantity}
            onChange={(event) => {
              setQuantity(event.target.value);
            }}
          />
          <select
            name="unit"
            defaultValue=""
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
          <input
            id="detail-discount"
            placeholder="Discount, %"
            type="number"
            value={discount}
            onChange={(event) => {
              setDiscount(event.target.value);
            }}
          />
          <input
            id="detail-price"
            placeholder="Price"
            type="number"
            value={price}
            onChange={(event) => {
              setPrice(event.target.value);
            }}
          />
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
