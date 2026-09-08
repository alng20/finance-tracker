import { useEffect, useState } from "react";

export function useSearch<T>(
  search: string,
  selected: T | null,
  searchFunc: (search: string) => Promise<T[]>,
) {
  const [results, setResults] = useState<T[]>([]);

  useEffect(() => {
    if (selected || !search.trim()) {
      setResults([]);
      return;
    }

    const timer = setTimeout(async () => {
      const data = await searchFunc(search);
      setResults(data);
    }, 300);

    return () => clearTimeout(timer);
  }, [search, selected]);

  return {
    results,
    clearResults: () => setResults([]),
  };
}
