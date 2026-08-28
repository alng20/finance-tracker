import { useEffect, useState } from "react";

import { searchItems } from "../../../../Items/api/searchItems";
import type { Item } from "../../../../Items/types/Item";

export function useSearchItems(searchName: string, selectedItem: Item | null) {
  const [items, setItems] = useState<Item[]>([]);

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
