import { useEffect, useState, type SubmitEventHandler } from "react";
import "./css/Pagination.css";

type PaginationProps = {
  pageNumber: number;
  pagesTotalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  onPageChange: (pageNumber: number) => void;
};
function Pagination({
  pageNumber,
  pagesTotalCount,
  hasNextPage,
  hasPreviousPage,
  onPageChange,
}: PaginationProps) {
  const [pageInput, setPageInput] = useState(String(pageNumber));

  useEffect(() => {
    setPageInput(String(pageNumber));
  }, [pageNumber]);

  const handlePageSubmit: SubmitEventHandler<HTMLFormElement> = (event) => {
    event.preventDefault();

    const requestedPage = Number(pageInput);
    const targetPage = Number.isFinite(requestedPage)
      ? Math.min(Math.max(Math.trunc(requestedPage), 1), pagesTotalCount)
      : pageNumber;

    setPageInput(String(targetPage));
    if (targetPage !== pageNumber) {
      onPageChange(targetPage);
    }
  };

  return (
    <div className="pagination">
      <button
        type="button"
        aria-label="First page"
        title="First page"
        disabled={!hasPreviousPage}
        onClick={() => onPageChange(1)}
      >
        &laquo;
      </button>

      <button
        type="button"
        aria-label="Previous page"
        title="Previous page"
        disabled={!hasPreviousPage}
        onClick={() => onPageChange(pageNumber - 1)}
      >
        &lt;
      </button>

      <form className="pagination__jump" onSubmit={handlePageSubmit}>
        <label htmlFor="pagination-page">Page</label>
        <input
          id="pagination-page"
          type="number"
          min={1}
          max={pagesTotalCount}
          value={pageInput}
          aria-label="Page number"
          onChange={(event) => setPageInput(event.target.value)}
        />
        <span>of {pagesTotalCount}</span>
      </form>

      <button
        type="button"
        aria-label="Next page"
        title="Next page"
        disabled={!hasNextPage}
        onClick={() => onPageChange(pageNumber + 1)}
      >
        &gt;
      </button>

      <button
        type="button"
        aria-label="Last page"
        title="Last page"
        disabled={!hasNextPage}
        onClick={() => onPageChange(pagesTotalCount)}
      >
        &raquo;
      </button>
    </div>
  );
}

export default Pagination;
