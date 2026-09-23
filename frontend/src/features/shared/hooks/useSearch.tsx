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

  const trimmedName = search.trim();

  if ((selected || !trimmedName) && results.length > 0) {
    setResults([]);
    setError(null);
    setIsLoading(false);
  }

  const execSearch = useCallback(
    (text: string, isActive: { current: boolean }) => {
      setIsLoading(true);
      setError(null);

      searchFunc(text)
        .then((data) => {
          if (isActive.current) {
            const mappedData = mapResult
              ? mapResult(data)
              : (data as unknown as OutputType[]);
            setResults(mappedData);
          }
        })
        .catch((err: unknown) => {
          if (isActive.current) {
            setError(err instanceof Error ? err : new Error("Search failed."));
            setResults([]);
          }
        })
        .finally(() => {
          if (isActive.current) {
            setIsLoading(false);
          }
        });
    },
    [searchFunc, mapResult],
  );

  const doSearch = useCallback(() => {
    if (!selected && trimmedName) {
      execSearch(trimmedName, { current: true });
    }
  }, [trimmedName, selected, execSearch]);

  useEffect(() => {
    if (selected || !trimmedName) {
      return;
    }

    const isActive = { current: true };

    const timer = window.setTimeout(() => {
      execSearch(trimmedName, isActive);
    }, 300);

    return () => {
      isActive.current = false;
      window.clearTimeout(timer);
    };
  }, [trimmedName, selected, execSearch]);

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
