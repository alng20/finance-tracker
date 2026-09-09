import { useState } from "react";
import type { GetItemsResponse } from "../types/responses/GetItemsResponse";
import {
  initialPageRequest,
  type PageRequestData,
} from "../../shared/types/PageRequestData";
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
import { useGet } from "../../shared/hooks/useGet";
import { useSearch } from "../../shared/hooks/useSearch";
import { getCategories } from "../../Categories/api/categoriesApi";
import type { SearchItemsDto } from "../types/dto/SearchItemsDto";
import "./Items.css";
import "./CreateItem/css/CreateItem.css";
import "../../Expenses/components/GetExpenses/css/ExpenseFilters.css";
import type { CreateItemData, UpdateItemData } from "../types/data/ItemData";
import type { ItemFiltersData } from "../types/data/ItemFiltersData";
import ItemFilters from "./GetItems/ItemFilters";
import {
  validateItem,
  type ItemValidationErrors,
} from "../validation/itemValidation";
import type { ItemDto } from "../types/dto/ItemDto";
import type { GetItemsDto } from "../types/dto/GetItemsDto";
import { units } from "../../shared/common/units";
import type { GetCategoriesDto } from "../../Categories/types/GetCategoriesDto";

function Items() {
  const [page, setPage] = useState<PageRequestData>(initialPageRequest);
  const [filters, setFilters] = useState<ItemFiltersData | null>(null);
  const [search, setSearch] = useState("");

  const [updateItemData, setUpdateItemData] = useState<UpdateItemData | null>(
    null,
  );
  const [updateErrors, setUpdateErrors] = useState<ItemValidationErrors>({});

  const [isCreating, setIsCreating] = useState(false);
  const [createItemData, setCreateItemData] = useState<CreateItemData | null>(
    null,
  );
  const [createErrors, setCreateErrors] = useState<ItemValidationErrors>({});
  const [createSubmitError, setCreateSubmitError] = useState<string | null>(
    null,
  );

  const {
    results: searchResults,
    isLoading: isSearchLoading,
    error: searchError,
    doSearch,
  } = useSearch<SearchItemsDto>(search, null, searchItems);

  const { results: categories } = useGet<GetCategoriesDto>(getCategories);

  const itemsUnits = units; // TODO: Get units by request

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

  const {
    results: items,
    pagination,
    isLoading,
    error,
    refresh,
  } = useGetWithFilters<GetItemsResponse, ItemFiltersData, GetItemsDto>(
    page,
    filters,
    getItems,
  );

  // TODO: Make get/search items response paged
  const displayedItems: ItemDto[] = search.trim() ? searchResults : items;
  const itemCount = search.trim()
    ? searchResults.length
    : pagination.dataTotalCount;

  const onClickUpdateItem = (item: ItemDto) => {
    setIsCreating(false);
    setUpdateErrors({});
    setUpdateItemData({
      id: item.id,
      name: item.name,
      categoryId: item?.categoryId ?? "",
      unit: item.unit,
    });
  };

  const onClickCreateItem = async () => {
    if (!createItemData) {
      return;
    }

    setCreateSubmitError(null);

    const errors = validateItem(createItemData);
    setCreateErrors(errors);

    if (Object.keys(errors).length > 0) {
      return;
    }

    try {
      await createItem({
        name: createItemData.name.trim(),
        categoryId: createItemData.categoryId,
        unit: createItemData.unit,
      });
      setIsCreating(false);
      setCreateItemData(null);
      setCreateErrors({});
      setCreateSubmitError(null);
      await refresh();
      doSearch();
    } catch (error) {
      const message =
        error instanceof Error
          ? error.message
          : "Failed to create item. Please try again.";

      setCreateSubmitError(message);
    }
  };

  const onClickSaveUpdatedItem = async () => {
    if (!updateItemData) {
      return;
    }

    const errors = validateItem(updateItemData);
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
    doSearch();
  };

  const onClickDeleteItem = async (item: ItemDto) => {
    if (!window.confirm(`Delete item "${item.name}"?`)) {
      return;
    }

    await deleteItem(item.id);
    await refresh();
    doSearch();
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
              setCreateSubmitError(null);
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
          units={itemsUnits}
          createItemData={createItemData}
          errors={createErrors}
          submitError={createSubmitError}
          onNameChange={setItemData(setCreateItemData, "name")}
          onCategoryChange={setItemData(setCreateItemData, "categoryId")}
          onUnitChange={setItemData(setCreateItemData, "unit")}
          onSubmit={onClickCreateItem}
          onCancel={() => {
            setIsCreating(false);
            setCreateSubmitError(null);
          }}
        />
      )}

      <section className="items-results-summary" aria-label="Items summary">
        <span>{search.trim() ? "Matching items" : "Total items"}</span>
        <strong>{itemCount}</strong>
      </section>

      <div className="items-controls">
        <ItemFilters
          categories={categories}
          units={itemsUnits}
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
            searchResults.length === 0 && <p>No items found.</p>}
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
        units={itemsUnits}
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
