import { useEffect, useState } from "react";

import type { SearchItemsDto } from "../../../../Items/types/SearchItemsDto";
import { searchItems } from "../../../../Items/api/itemsApi";

export function useSearchItems(
  searchName: string,
  selectedItem: SearchItemsDto | null,
) {
  const [items, setItems] = useState<SearchItemsDto[]>([]);

  useEffect(() => {
    if (selectedItem || !searchName.trim()) {
      setItems([]);
      return;
    }

    const timer = setTimeout(async () => {
      const data = await searchItems(searchName);
      setItems(data);
    }, 300);

    return () => clearTimeout(timer);
  }, [searchName, selectedItem]);

  return {
    items,
    clearItems: () => setItems([]),
  };
}
