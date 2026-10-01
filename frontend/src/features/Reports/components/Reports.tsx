import "./css/Reports.css";

import { useState } from "react";

import { defaultCurrency } from "../../shared/common/consts";
import type { ReportGroupingType } from "../types/GetGroupedAmountResponse";
import type { ReportFiltersData } from "../types/ReportFiltersData";
import CategoryBreakdown from "./CategoryBreakdown";
import ReportFilters from "./ReportFilters";
import RetailerBreakdown from "./RetailerBreakdown";
import ShopBreakdown from "./ShopBreakdown";
import SpendingTrend from "./SpendingTrend";
import TotalAmountInfo from "./TotalAmountInfo";

function Reports() {
  const [filters, setFilters] = useState<ReportFiltersData>({
    currency: defaultCurrency,
  });
  const [groupingType, setGroupingType] = useState<ReportGroupingType>("Week");

  return (
    <div className="reports">
      <div className="reports__header">
        <p className="reports__eyebrow">Spending overview</p>
        <h1 className="reports__header_title">Reports</h1>
        <p className="reports__header_description">
          Understand where your money goes
        </p>
      </div>

      <ReportFilters filters={filters} onChange={setFilters} />

      <TotalAmountInfo filters={filters} />

      <SpendingTrend
        filters={filters}
        groupingType={groupingType}
        onGroupingTypeChange={setGroupingType}
      />

      <CategoryBreakdown filters={filters} />
      <ShopBreakdown filters={filters} />
      <RetailerBreakdown filters={filters} />
    </div>
  );
}

export default Reports;
