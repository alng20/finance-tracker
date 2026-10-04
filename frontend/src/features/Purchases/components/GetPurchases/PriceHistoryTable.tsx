import "./css/PriceHistoryTable.css";

import { useCallback, useMemo, useState } from "react";

import { formatAmount, formatDate } from "../../../shared/common/utils";
import Pagination from "../../../shared/components/Pagination/Pagintation";
import useGetWithFilters from "../../../shared/hooks/useGetWithFilters";
import type { PageRequestData } from "../../../shared/types/PageRequestData";
import { getItemPrices, type ItemPricesFilters } from "../../api/purchasesApi";
import type {
  GetItemPricesResponse,
  PriceHistoryDto,
} from "../../types/GetItemPricesResponse";
import {
  defaultPricesSortType,
  type PricesSortType,
} from "../../types/PricesSortType";

const historyPage: PageRequestData = { pageNumber: 1, pageSize: 5 };

type PriceHistoryTableProps = {
  itemId: string;
  itemName: string;
  fromDate?: string;
  toDate?: string;
};

function nextSortType(
  current: PricesSortType,
  field: "Date" | "Price",
): PricesSortType {
  if (field === "Date") {
    return current === "DateDesc" ? "DateAsc" : "DateDesc";
  }

  return current === "PriceAsc" ? "PriceDesc" : "PriceAsc";
}

function getAriaSort(
  sortType: PricesSortType,
  field: "Date" | "Price",
): "ascending" | "descending" | "none" {
  if (!sortType.startsWith(field)) {
    return "none";
  }

  return sortType.endsWith("Asc") ? "ascending" : "descending";
}

function getSortIndicator(sortType: PricesSortType, field: "Date" | "Price") {
  const ariaSort = getAriaSort(sortType, field);

  if (ariaSort === "none") {
    return "";
  }

  return ariaSort === "ascending" ? " ▲" : " ▼";
}

function PriceHistoryTable({
  itemId,
  itemName,
  fromDate,
  toDate,
}: PriceHistoryTableProps) {
  const [page, setPage] = useState<PageRequestData>(historyPage);
  const [sortType, setSortType] = useState<PricesSortType>(
    defaultPricesSortType,
  );

  const filters = useMemo<ItemPricesFilters>(
    () => ({ fromDate, toDate, sortType }),
    [fromDate, toDate, sortType],
  );

  const getter = useCallback(
    (
      pageRequest: PageRequestData,
      itemFilters: ItemPricesFilters | null,
      reqInit: RequestInit,
    ) => getItemPrices(itemId, pageRequest, itemFilters, reqInit),
    [itemId],
  );

  const [prevFilters, setPrevFilters] = useState(filters);
  if (prevFilters !== filters) {
    setPrevFilters(filters);
    setPage((current) => ({ ...current, pageNumber: 1 }));
  }

  const {
    results: prices,
    pagination,
    isLoading,
    error,
  } = useGetWithFilters<
    GetItemPricesResponse,
    ItemPricesFilters,
    PriceHistoryDto
  >(page, filters, getter);

  const handleSort = (field: "Date" | "Price") => {
    setSortType((current) => nextSortType(current, field));
  };

  return (
    <div
      className="price-history-table"
      role="region"
      aria-label={`Prices history of ${itemName}`}
    >
      {error && (
        <p className="price-history-table__message">
          Failed to load prices history.
        </p>
      )}

      {!error && isLoading && (
        <p className="price-history-table__message">Loading...</p>
      )}

      {!error && !isLoading && prices.length === 0 && (
        <p className="price-history-table__message">
          No prices found for this period.
        </p>
      )}

      {!error && !isLoading && prices.length > 0 && (
        <div className="price-history-table__number">
          Purchases number: {pagination.dataTotalCount}
        </div>
      )}

      {!error && !isLoading && prices.length > 0 && (
        <table className="price-history-table__table">
          <thead>
            <tr>
              <th scope="col" aria-sort={getAriaSort(sortType, "Date")}>
                <button type="button" onClick={() => handleSort("Date")}>
                  Date{getSortIndicator(sortType, "Date")}
                </button>
              </th>
              <th scope="col">Shop</th>
              <th scope="col" aria-sort={getAriaSort(sortType, "Price")}>
                <button type="button" onClick={() => handleSort("Price")}>
                  Amount{getSortIndicator(sortType, "Price")}
                </button>
              </th>
            </tr>
          </thead>
          <tbody>
            {prices.map((price, index) => (
              <tr key={`${price.date}-${price.shopId ?? "none"}-${index}`}>
                <td>{formatDate(price.date)}</td>
                <td>{price.shopName}</td>
                <td>{formatAmount(price.price, price.currency)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {!error && pagination.pagesTotalCount > 1 && (
        <Pagination
          pageNumber={page.pageNumber}
          pagesTotalCount={pagination.pagesTotalCount}
          hasNextPage={pagination.hasNextPage}
          hasPreviousPage={pagination.hasPreviousPage}
          onPageChange={(newPage) =>
            setPage((current) => ({ ...current, pageNumber: newPage }))
          }
        />
      )}
    </div>
  );
}

export default PriceHistoryTable;
