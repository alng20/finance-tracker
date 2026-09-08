import { useEffect, useState } from "react";
import type {
  GetItemsResponse,
  ItemDto,
} from "../../Expenses/types/GetItemsResponse";
import {
  initialPageRequest,
  type PageRequestData,
} from "../../Expenses/types/PageRequestData";
import useGetWithFilters from "../../shared/hooks/useGetWithFilters";
import {
  createItem,
  deleteItem,
  getItems,
  searchItems,
  updateItem,
} from "../api/itemsApi";
import ItemsTable from "./GetItems/ItemsTable";
import CreateItem from "./CreateItem/CreateItem";
import Pagination from "../../shared/components/Pagination/Pagintation";
import { useGet } from "../../Expenses/hooks/useGet";
import { getCategories } from "../../Categories/api/categoriesApi";
import type { GetCategoriesDto } from "../../Categories/types/GetCategoriesDto";
import type { SearchItemsDto } from "../types/SearchItemsDto";
import type { ItemFormErrors } from "./GetItems/ItemRow";
import "./Items.css";
import "./CreateItem/css/CreateItem.css";
import "../../Expenses/components/GetExpenses/css/ExpenseFilters.css";
import type { CreateItemData, UpdateItemData } from "../types/ItemData";
import type { ItemFiltersData } from "../types/ItemFiltersData";
import ItemFilters from "./GetItems/ItemFilters";

// TODO: Implement Units or move to separate file
const units = ["Undefined", "Piece", "G", "KG", "Milliliter", "Liter"].map(
  (unit) => ({ id: unit, name: unit }),
);

// TODO: Move to separate file
function validateItemForm({
  name,
  categoryId,
  unit,
}: CreateItemData | UpdateItemData): ItemFormErrors {
  const errors: ItemFormErrors = {};
  const trimmedName = name.trim();

  if (!trimmedName) {
    errors.name = "Name is required.";
  } else if (trimmedName.length > 100) {
    errors.name = "Name must be 100 characters or fewer.";
  }

  if (!categoryId) {
    errors.category = "Category is required.";
  }

  if (!unit) {
    errors.unit = "Unit is required.";
  }

  return errors;
}

function Items() {
  const [page, setPage] = useState<PageRequestData>(initialPageRequest);
  const [filters, setFilters] = useState<ItemFiltersData | null>(null);

  const [updateItemData, setUpdateItemData] = useState<UpdateItemData | null>(
    null,
  );
  const [updateErrors, setUpdateErrors] = useState<ItemFormErrors>({});

  const [isCreating, setIsCreating] = useState(false);
  const [createItemData, setCreateItemData] = useState<CreateItemData | null>(
    null,
  );
  const [createErrors, setCreateErrors] = useState<ItemFormErrors>({});

  const [isSearchLoading, setIsSearchLoading] = useState(false);
  const [search, setSearch] = useState("");
  const [searchResults, setSearchResults] = useState<SearchItemsDto[]>([]);
  const [searchError, setSearchError] = useState<Error | null>(null);

  const { results: categories } = useGet<GetCategoriesDto>(getCategories);

  function setItemData<T>(
    setter: React.Dispatch<React.SetStateAction<T | null>>,
    key: string,
  ): (value: string) => void {
    return (value: string) => {
      setter((current) => ({
        ...current!,
        [key]: value,
      }));
    };
  }

  // TODO: use useSearch or useSearchItems
  useEffect(() => {
    const searchValue = search.trim();

    if (!searchValue) {
      setSearchResults([]);
      setSearchError(null);
      setIsSearchLoading(false);
      return;
    }

    let isActive = true;
    setSearchResults([]);
    setIsSearchLoading(true);
    setSearchError(null);

    const timer = window.setTimeout(() => {
      searchItems(searchValue)
        .then((results) => {
          if (isActive) {
            setSearchResults(results);
          }
        })
        .catch((error: unknown) => {
          if (isActive) {
            setSearchError(
              error instanceof Error ? error : new Error("Search failed"),
            );
            setSearchResults([]);
          }
        })
        .finally(() => {
          if (isActive) {
            setIsSearchLoading(false);
          }
        });
    }, 300);

    return () => {
      isActive = false;
      window.clearTimeout(timer);
    };
  }, [search]);

  const {
    results: filteredItems,
    pagination,
    isLoading,
    error,
    refresh,
  } = useGetWithFilters<GetItemsResponse, ItemFiltersData, ItemDto>(
    page,
    filters,
    getItems,
  );

  // TODO: Fix category match, add categoryName to response
  const filteredSearchResults = searchResults.filter((item) => {
    const matchesCategory =
      !filters?.categories?.length ||
      filters.categories.some((category) => category.id === item.categoryId);
    const matchesUnit =
      !filters?.units?.length ||
      filters.units.some((unit) => unit.id === item.unit);

    return matchesCategory && matchesUnit;
  });

  // TODO: Make get/search items response paged
  const displayedItems: ItemDto[] = search.trim()
    ? filteredSearchResults.map((item) => ({
        ...item,
        categoryName:
          categories.find((category) => category.id === item.categoryId)
            ?.name ?? "Unknown category",
      }))
    : filteredItems;
  const itemCount = search.trim()
    ? filteredSearchResults.length
    : pagination.dataTotalCount;

  const onClickUpdateItem = (item: ItemDto) => {
    setIsCreating(false);
    setUpdateErrors({});
    setUpdateItemData({
      id: item.id,
      name: item.name,
      categoryId: item.categoryId,
      unit: item.unit,
    });
  };

  const onClickCreateItem = async () => {
    if (!createItemData) {
      return;
    }

    const errors = validateItemForm(createItemData);
    setCreateErrors(errors);

    if (Object.keys(errors).length > 0) {
      return;
    }

    await createItem({
      name: createItemData.name.trim(),
      categoryId: createItemData.categoryId,
      unit: createItemData.unit,
    });
    setIsCreating(false);
    setCreateItemData(null);
    setCreateErrors({});
    await refresh();
  };

  const onClickSaveUpdatedItem = async () => {
    if (!updateItemData) {
      return;
    }

    const errors = validateItemForm(updateItemData);
    setUpdateErrors(errors);

    if (Object.keys(errors).length > 0) {
      return;
    }

    await updateItem({
      id: updateItemData.id,
      name: updateItemData.name.trim(),
      categoryId: updateItemData.categoryId,
      unit: updateItemData.unit,
    });
    setUpdateItemData(null);
    await refresh();
  };

  const onClickDeleteItem = async (item: ItemDto) => {
    if (!window.confirm(`Delete item "${item.name}"?`)) {
      return;
    }

    await deleteItem(item.id);
    await refresh();
  };

  return (
    <div className="items">
      <div className="items-page__header">
        <div>
          <p className="items-page__eyebrow">Items management</p>
          <h1 className="items-page__title">Items</h1>
          <p className="items-page__description">
            Manage the items used in your expenses
          </p>
        </div>

        <div className="items-page__create_item_button">
          <button
            onClick={() => {
              setUpdateItemData(null);
              setIsCreating(true);
              setCreateErrors({});
            }}
            className="items-page__create_item_button_click"
          >
            Create Item
          </button>
        </div>
      </div>

      {isCreating && (
        <CreateItem
          categories={categories}
          units={units}
          createItemData={createItemData}
          errors={createErrors}
          onNameChange={setItemData(setCreateItemData, "name")}
          onCategoryChange={setItemData(setCreateItemData, "categoryId")}
          onUnitChange={setItemData(setCreateItemData, "unit")}
          onSubmit={onClickCreateItem}
          onCancel={() => setIsCreating(false)}
        />
      )}

      <section className="items-results-summary" aria-label="Items summary">
        <span>{search.trim() ? "Matching items" : "Total items"}</span>
        <strong>{itemCount}</strong>
      </section>

      <div className="items-controls">
        <ItemFilters
          categories={categories}
          units={units}
          filters={filters}
          onChange={setFilters}
          onPageReset={() =>
            setPage((current) => ({ ...current, pageNumber: 1 }))
          }
        />

        <div className="items-search">
          <label htmlFor="items-search-input">Search items</label>
          <input
            id="items-search-input"
            type="search"
            value={search}
            placeholder="Search by name"
            onChange={(event) => setSearch(event.target.value)}
          />
          {isSearchLoading && <p>Searching...</p>}
          {searchError && <p>Failed to search items.</p>}
          {!isSearchLoading &&
            search.trim() &&
            !searchError &&
            filteredSearchResults.length === 0 && <p>No items found.</p>}
        </div>
      </div>

      {!search.trim() && (
        <Pagination
          pageNumber={page.pageNumber}
          pagesTotalCount={pagination.pagesTotalCount}
          hasNextPage={pagination.hasNextPage}
          hasPreviousPage={pagination.hasPreviousPage}
          onPageChange={(newPage) =>
            setPage((current) => ({
              ...current,
              pageNumber: newPage,
            }))
          }
        />
      )}

      <ItemsTable
        items={displayedItems}
        isLoading={search.trim() ? isSearchLoading : isLoading}
        error={search.trim() ? searchError : error}
        onUpdate={onClickUpdateItem}
        onDelete={onClickDeleteItem}
        categories={categories}
        units={units}
        updateItemData={updateItemData}
        updateErrors={updateErrors}
        onUpdateNameChange={setItemData(setUpdateItemData, "name")}
        onUpdateCategoryChange={setItemData(setUpdateItemData, "categoryId")}
        onUpdateUnitChange={setItemData(setUpdateItemData, "unit")}
        onSave={onClickSaveUpdatedItem}
        onCancel={() => setUpdateItemData(null)}
      />
    </div>
  );
}

export default Items;
