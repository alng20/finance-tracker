import { useCallback, useEffect, useState } from "react";
// import type { FilterOption } from "../components/GetExpenses/SelectFilter";
import * as itemsApi from "../../Items/api/itemsApi";
import type { FilterData } from "../../shared/types/FilterData";

export function useSearchItems(search: string) {
  const [items, setItems] = useState<FilterData[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);

  function clearItems() {
    setItems([]);
  }

  const searchItems = useCallback(async () => {
    if (!search.trim()) {
      setItems([]);
      return;
    }

    setIsLoading(true);
    setError(null);

    const timer = setTimeout(async () => {
      itemsApi
        .searchItems(search)
        .then((items) => {
          const options: FilterData[] = items.map((item) => ({
            id: item.id,
            name: item.name,
          }));

          setItems(options);
        })
        .catch(setError)
        .finally(() => setIsLoading(false));
    }, 300);
    return () => clearTimeout(timer);
  }, [search]);

  useEffect(() => {
    searchItems();
  }, [searchItems]);

  return {
    items,
    clearItems,
    isLoading,
    error,
  };
}
