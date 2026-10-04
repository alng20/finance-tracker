import "./Purchases.css";

import { useEffect, useMemo, useState } from "react";

import Pagination from "../../shared/components/Pagination/Pagintation";
import useGetWithFilters from "../../shared/hooks/useGetWithFilters";
import {
  initialPageRequest,
  type PageRequestData,
} from "../../shared/types/PageRequestData";
import { getPurchases } from "../api/purchasesApi";
import type {
  GetPurchasesResponse,
  PurchaseDto,
} from "../types/GetPurchasesResponse";
import type { PurchaseFiltersData } from "../types/PurchaseFiltersData";
import PurchaseFilters from "./GetPurchases/PurchaseFilters";
import PurchasesTable from "./GetPurchases/PurchasesTable";

function Purchases() {
  const [page, setPage] = useState<PageRequestData>(initialPageRequest);
  const [appliedFilters, setAppliedFilters] =
    useState<PurchaseFiltersData | null>(null);
  const [search, setSearch] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");

  useEffect(() => {
    const timer = window.setTimeout(() => {
      setDebouncedSearch((current) => {
        const trimmed = search.trim();
        if (current !== trimmed) {
          setPage((currentPage) => ({ ...currentPage, pageNumber: 1 }));
        }
        return trimmed;
      });
    }, 300);

    return () => window.clearTimeout(timer);
  }, [search]);

  const filters = useMemo<PurchaseFiltersData | null>(() => {
    if (!appliedFilters && !debouncedSearch) {
      return null;
    }

    return { ...appliedFilters, searchString: debouncedSearch || undefined };
  }, [appliedFilters, debouncedSearch]);

  const {
    results: purchases,
    pagination,
    isLoading,
    error,
  } = useGetWithFilters<GetPurchasesResponse, PurchaseFiltersData, PurchaseDto>(
    page,
    filters,
    getPurchases,
  );

  const handleApplyFilters = (newFilters: PurchaseFiltersData | null) => {
    setPage((current) => ({ ...current, pageNumber: 1 }));
    setAppliedFilters(newFilters);
  };

  const handlePageChange = (newPage: number) =>
    setPage((current) => ({ ...current, pageNumber: newPage }));

  const paginationBlock = (
    <Pagination
      pageNumber={page.pageNumber}
      pagesTotalCount={pagination.pagesTotalCount}
      hasNextPage={pagination.hasNextPage}
      hasPreviousPage={pagination.hasPreviousPage}
      onPageChange={handlePageChange}
    />
  );

  return (
    <div className="purchases">
      <div className="purchases__header">
        <p className="purchases__eyebrow">Prices overview</p>
        <h1 className="purchases__title">Purchases</h1>
        <p className="purchases__description">Track your purchases prices</p>
      </div>

      <PurchaseFilters
        search={search}
        onSearchChange={setSearch}
        onApply={handleApplyFilters}
      />

      {paginationBlock}

      <PurchasesTable
        purchases={purchases}
        isLoading={isLoading}
        error={error}
        fromDate={filters?.fromDate}
        toDate={filters?.toDate}
      />

      {paginationBlock}
    </div>
  );
}

export default Purchases;
