export function makeUrlSearch(searchString: string): URLSearchParams {
  return new URLSearchParams({
    searchString,
  });
}
