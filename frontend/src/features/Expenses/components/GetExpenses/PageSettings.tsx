import { initialPageData, type PageData } from "../../types/PageData";
import "./css/ExpenseFilters.css";
import type { ExpenseFiltersData } from "../../types/ExpenseFiltersData";

export type ExpenseFiltersProps = {
  setPage: React.Dispatch<React.SetStateAction<PageData>>;
};

function PageSettings({ setPage }: ExpenseFiltersProps) {
  async function onClickApply() {
    const page: PageData = {
      pageNumber: 1,
      pageSize: 2,
    };
    setPage(page);
  }

  async function onClickClear() {
    setPage(initialPageData);
  }

  return (
    <section className="page_settings">
      <span>PAGE</span>
      <button onClick={onClickApply}>Apply</button>
      <button onClick={onClickClear}>Clear</button>
    </section>
  );
}

export default PageSettings;
