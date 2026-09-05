import { useCallback, useEffect, useState } from "react";

export function useGet<T>(getter: () => Promise<T[]>) {
  const [results, setResults] = useState<T[]>([]);

  const getResults = useCallback(async () => {
    async function get() {
      const data = await getter();
      setResults(data);
    }
    get();
  }, []);

  useEffect(() => {
    getResults();
  }, [getResults]);

  return {
    results,
    clearResults: () => setResults([]),
  };
}
