import { useState } from "react";

import { getRetailers } from "../../Retailers/api/retailersApi";
import type { GetRetailersDto } from "../../Retailers/types/GetRetailersDto";
import { useGet } from "../../shared/hooks/useGet";
import useGetWithFilters from "../../shared/hooks/useGetWithFilters";
import { useSearch } from "../../shared/hooks/useSearch";
import {
  initialPageRequest,
  type PageRequestData,
} from "../../shared/types/PageRequestData";
import type { GetShopsDto } from "../types/dto/GetShopsDto";
import type { GetShopsResponse } from "../types/responses/GetShopsResponse";
import type { ShopFiltersData } from "../types/data/ShopFiltersData";
import {
  createShop,
  deleteShop,
  getShops,
  searchShops,
  updateShop,
} from "../api/shopsApi";
import CreateShop from "./CreateShop/CreateShop";
import GetShopsTable from "./GetShops/GetShopsTable";
import Pagination from "../../shared/components/Pagination/Pagintation";
import type { CreateShopData, UpdateShopData } from "../types/data/ShopData";
import {
  validateShop,
  type ShopValidationErrors,
} from "../validation/shopValidation";
import "./Shops.css";
import "../../Expenses/components/GetExpenses/css/ExpenseFilters.css";
import ShopFilters from "./GetShops/ShopFilters";
import type { SearchShopDto } from "../types/dto/SearchShopDto";
import type { ShopDto } from "../types/dto/ShopDto";

function Shops() {
  const [page, setPage] = useState<PageRequestData>(initialPageRequest);
  const [filters, setFilters] = useState<ShopFiltersData | null>(null);
  const [search, setSearch] = useState("");

  const [updateShopData, setUpdateShopData] = useState<UpdateShopData | null>(
    null,
  );
  const [updateErrors, setUpdateErrors] = useState<ShopValidationErrors>({});

  const [isCreating, setIsCreating] = useState(false);
  const [createShopData, setCreateShopData] = useState<CreateShopData | null>(
    null,
  );
  const [createErrors, setCreateErrors] = useState<ShopValidationErrors>({});
  const [createSubmitError, setCreateSubmitError] = useState<string | null>(
    null,
  );

  const {
    results: searchResults,
    isLoading: isSearchLoading,
    error: searchError,
    doSearch,
  } = useSearch<SearchShopDto>(search, null, searchShops);

  const { results: retailers } = useGet<GetRetailersDto>(getRetailers);

  const {
    results: shops,
    pagination,
    isLoading,
    error,
    refresh,
  } = useGetWithFilters<GetShopsResponse, ShopFiltersData, GetShopsDto>(
    page,
    filters,
    getShops,
  );

  const displayedShops: ShopDto[] = search.trim() ? searchResults : shops;
  const shopCount = search.trim()
    ? searchResults.length
    : pagination.dataTotalCount;

  function setShopData<T>(
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

  const onClickUpdateShop = (shop: ShopDto) => {
    setIsCreating(false);
    setUpdateErrors({});
    setUpdateShopData({
      id: shop.id,
      name: shop.name,
      retailerId: shop.retailerId ?? "",
      country: shop.country ?? "",
      city: shop.city ?? "",
    });
  };

  const onClickCreateShop = async () => {
    if (!createShopData) {
      return;
    }

    setCreateSubmitError(null);

    const errors = validateShop(createShopData);
    setCreateErrors(errors);

    if (Object.keys(errors).length > 0) {
      return;
    }

    try {
      await createShop({
        name: createShopData.name.trim(),
        retailerId: createShopData.retailerId || null,
        country: createShopData.country || null,
        city: createShopData.city || null,
      });
      setIsCreating(false);
      setCreateShopData(null);
      setCreateErrors({});
      setCreateSubmitError(null);
      await refresh();
      doSearch();
    } catch (error) {
      const message =
        error instanceof Error
          ? error.message
          : "Failed to create shop. Please try again.";

      setCreateSubmitError(message);
    }
  };

  const onClickSaveUpdatedShop = async () => {
    if (!updateShopData) {
      return;
    }

    const errors = validateShop(updateShopData);
    setUpdateErrors(errors);

    if (Object.keys(errors).length > 0) {
      return;
    }

    await updateShop({
      id: updateShopData.id,
      name: updateShopData.name.trim(),
      retailerId: updateShopData.retailerId || null,
      country: updateShopData.country || null,
      city: updateShopData.city || null,
    });
    setUpdateShopData(null);
    await refresh();
    doSearch();
  };

  const onClickDeleteShop = async (shop: ShopDto) => {
    if (!window.confirm(`Delete shop "${shop.name}"?`)) {
      return;
    }

    await deleteShop(shop.id);
    await refresh();
    doSearch();
  };

  return (
    <div className="shops">
      <div className="shops-page__header">
        <div>
          <p className="shops-page__eyebrow">Shops management</p>
          <h1 className="shops-page__title">Shops</h1>
          <p className="shops-page__description">
            Manage the shops used in your expenses
          </p>
        </div>

        <div className="shops-page__create_shop_button">
          <button
            onClick={() => {
              setUpdateShopData(null);
              setIsCreating(true);
              setCreateErrors({});
              setCreateSubmitError(null);
            }}
            className="shops-page__create_shop_button_click"
          >
            Create Shop
          </button>
        </div>
      </div>

      {isCreating && (
        <CreateShop
          retailers={retailers}
          createShopData={createShopData}
          errors={createErrors}
          submitError={createSubmitError}
          onNameChange={setShopData(setCreateShopData, "name")}
          onRetailerChange={setShopData(setCreateShopData, "retailerId")}
          onCountryChange={setShopData(setCreateShopData, "country")}
          onCityChange={setShopData(setCreateShopData, "city")}
          onSubmit={onClickCreateShop}
          onCancel={() => {
            setIsCreating(false);
            setCreateSubmitError(null);
          }}
        />
      )}

      <section className="shops-results-summary" aria-label="Shops summary">
        <span>{search.trim() ? "Matching shops" : "Total shops"}</span>
        <strong>{shopCount}</strong>
      </section>

      <div className="shops-controls">
        <ShopFilters
          retailers={retailers}
          filters={filters}
          onChange={setFilters}
          onPageReset={() =>
            setPage((current) => ({ ...current, pageNumber: 1 }))
          }
        />

        <div className="shops-search">
          <label htmlFor="shops-search-input">Search shops</label>
          <input
            id="shops-search-input"
            type="search"
            value={search}
            placeholder="Search by name"
            onChange={(event) => setSearch(event.target.value)}
          />
          {isSearchLoading && <p>Searching...</p>}
          {searchError && <p>Failed to search shops.</p>}
          {!isSearchLoading &&
            search.trim() &&
            !searchError &&
            searchResults.length === 0 && <p>No shops found.</p>}
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

      <GetShopsTable
        shops={displayedShops}
        isLoading={search.trim() ? isSearchLoading : isLoading}
        error={search.trim() ? searchError : error}
        onUpdate={onClickUpdateShop}
        onDelete={onClickDeleteShop}
        retailers={retailers}
        updateShopData={updateShopData}
        updateErrors={updateErrors}
        onUpdateNameChange={setShopData(setUpdateShopData, "name")}
        onUpdateRetailerChange={setShopData(setUpdateShopData, "retailerId")}
        onUpdateCountryChange={setShopData(setUpdateShopData, "country")}
        onUpdateCityChange={setShopData(setUpdateShopData, "city")}
        onSave={onClickSaveUpdatedShop}
        onCancel={() => setUpdateShopData(null)}
      />
    </div>
  );
}

export default Shops;
