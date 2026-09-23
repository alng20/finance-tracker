import "./css/Pagination.css";

import { type SubmitEventHandler } from "react";

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
  const handlePageSubmit: SubmitEventHandler<HTMLFormElement> = (event) => {
    event.preventDefault();

    const formData = new FormData(event.currentTarget);
    const requestedPage = Number(formData.get("page-input"));
    const targetPage = Number.isFinite(requestedPage)
      ? Math.min(Math.max(Math.trunc(requestedPage), 1), pagesTotalCount)
      : pageNumber;

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
          name="page-input"
          min={1}
          max={pagesTotalCount}
          key={pageNumber}
          defaultValue={pageNumber}
          aria-label="Page number"
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
