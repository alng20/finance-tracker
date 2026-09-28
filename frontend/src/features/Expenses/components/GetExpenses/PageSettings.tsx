import "./css/PageSettings.css";

import {
  initialPageRequest,
  type PageRequestData,
} from "../../../shared/types/PageRequestData";

// TODO: Implement
export type ExpenseFiltersProps = {
  setPage: React.Dispatch<React.SetStateAction<PageRequestData>>;
};

function PageSettings({ setPage }: ExpenseFiltersProps) {
  async function onClickApply() {
    const page: PageRequestData = {
      pageNumber: 1,
      pageSize: 2,
    };
    setPage(page);
  }

  async function onClickClear() {
    setPage(initialPageRequest);
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
