import { useEffect, useState } from "react";
import type { Shop } from "../../../../Shops/types/Shop";
import { searchShops } from "../../../../Shops/api/searchShops";

export function useSearchShop(searchName: string, shop: Shop | null) {
  const [shops, setShops] = useState<Shop[]>([]);

  useEffect(() => {
    if (!searchName.trim() || shop) {
      setShops([]);
      return;
    }

    const timer = setTimeout(async () => {
      const data = await searchShops(searchName);
      setShops(data);
    }, 300);

    return () => clearTimeout(timer);
  }, [searchName, shop]);

  return {
    shops,
    clearShops: () => setShops([]),
  };
}
