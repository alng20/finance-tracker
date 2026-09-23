import { useCallback, useEffect, useRef, useState } from "react";

import type { PagedResponse } from "../types/PagedResponse";
import type { PageRequestData } from "../types/PageRequestData";
import type { PaginationData } from "../types/PaginationData";

function useGetWithFilters<
  ResponseType extends PagedResponse<DtoType, MetadataType>,
  FiltersType,
  DtoType,
  MetadataType = undefined,
>(
  page: PageRequestData,
  filters: FiltersType | null,
  getter: (
    page: PageRequestData,
    filters: FiltersType | null,
    reqInit: RequestInit,
  ) => Promise<ResponseType>,
) {
  const [results, setResults] = useState<DtoType[]>([]);
  const [metadata, setMetadata] = useState<MetadataType | null>(null);
  const [pagination, setPagination] = useState<PaginationData>({
    pagesTotalCount: 0,
    dataTotalCount: 0,
    hasNextPage: false,
    hasPreviousPage: false,
  });
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  const abortCtrlRef = useRef<AbortController | null>(null);

  const refresh = useCallback(async () => {
    abortCtrlRef.current?.abort();

    const controller = new AbortController();
    abortCtrlRef.current = controller;

    getter(page, filters, { signal: controller.signal })
      .then((response: ResponseType) => {
        if (!controller.signal.aborted) {
          setIsLoading(false);
        }
        setResults(response.data);
        setMetadata(response.metadata ?? null);
        setPagination({
          pagesTotalCount: response.totalPages,
          dataTotalCount: response.totalCount,
          hasNextPage: response.hasNextPage,
          hasPreviousPage: response.hasPreviousPage,
        });
      })
      .catch((err) => {
        if (err.name !== "AbortError") {
          setError(err);
        }
        if (!controller.signal.aborted) {
          setIsLoading(false);
        }
      });
  }, [filters, page, getter]);

  useEffect(() => {
    refresh();
    const currentAbortCtrl = abortCtrlRef.current;
    return () => {
      currentAbortCtrl?.abort();
    };
  }, [refresh]);

  return {
    results,
    metadata,
    pagination,
    isLoading,
    error,
    refresh,
  };
}

export default useGetWithFilters;
