import { useCallback, useEffect, useRef, useState } from "react";

import type { PagedResponse } from "../../Expenses/types/PagedResponse";
import type { PageRequestData } from "../../Expenses/types/PageRequestData";
import type { PaginationData } from "../../Expenses/types/PaginationData";

function useGetWithFilters<
  ResponseType extends PagedResponse<DtoType>,
  FiltersType,
  DtoType,
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

    setIsLoading(true);
    setError(null);

    getter(page, filters, { signal: abortCtrlRef.current?.signal })
      .then((response: ResponseType) => {
        setResults(response.data);
        setPagination({
          pagesTotalCount: response.totalPages,
          dataTotalCount: response.totalCount,
          hasNextPage: response.hasNextPage,
          hasPreviousPage: response.hasPreviousPage,
        });
      })
      .catch(setError)
      .finally(() => setIsLoading(false));
  }, [filters, page]);

  useEffect(() => {
    refresh();
    return () => {
      abortCtrlRef.current?.abort();
    };
  }, [refresh]);

  return {
    results,
    pagination,
    isLoading,
    error,
    refresh,
  };
}

export default useGetWithFilters;
