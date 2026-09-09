import { useCallback, useEffect, useState } from "react";

export function useSearch<InputType, OutputType = InputType>(
  search: string,
  selected: OutputType | null,
  searchFunc: (search: string) => Promise<InputType[]>,
  mapResult?: (items: InputType[]) => OutputType[],
) {
  const [results, setResults] = useState<OutputType[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);

  const doSearch = useCallback(() => {
    const trimmedName = search.trim();

    if (selected || !trimmedName) {
      setResults([]);
      setError(null);
      setIsLoading(false);
      return;
    }

    let isActive = true;

    setResults([]);
    setIsLoading(true);
    setError(null);

    const timer = window.setTimeout(() => {
      searchFunc(trimmedName)
        .then((data) => {
          if (isActive) {
            const mappedData = mapResult
              ? mapResult(data)
              : (data as unknown as OutputType[]);

            setResults(mappedData);
          }
        })
        .catch((err: unknown) => {
          if (isActive) {
            setError(err instanceof Error ? err : new Error("Search failed."));
            setResults([]);
          }
        })
        .finally(() => {
          if (isActive) {
            setIsLoading(false);
          }
        });
    }, 300);
    return () => {
      isActive = false;
      window.clearTimeout(timer);
    };
  }, [search, selected, searchFunc, mapResult]);

  useEffect(() => {
    const clear = doSearch();
    return clear;
  }, [doSearch]);

  return {
    results,
    clearResults: () => {
      setResults([]);
      setError(null);
      setIsLoading(false);
    },
    isLoading,
    error,
    doSearch,
  };
}
