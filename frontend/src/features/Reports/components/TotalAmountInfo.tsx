import "./css/TotalAmountInfo.css";

import { formatAmount } from "../../shared/common/utils";
import * as reportsApi from "../api/reportsApi";
import useReportAmount from "../hooks/useReportAmount";
import type { ReportFiltersData } from "../types/ReportFiltersData";

type TotalAmountInfoProps = {
  filters: ReportFiltersData;
};

function TotalAmountInfo({ filters }: TotalAmountInfoProps) {
  const { result, isLoading, error } = useReportAmount(
    filters,
    reportsApi.getTotalAmount,
  );

  return (
    <section className="total-amount-info" aria-label="Total amount">
      <h2>Total Amount</h2>

      {isLoading && <div>Loading...</div>}
      {error && <div>Failed to load total amount.</div>}
      {!isLoading && !error && (
        <div className="total-amount-info_data">
          {formatAmount(result?.totalAmount ?? 0, filters.currency)}
        </div>
      )}
    </section>
  );
}

export default TotalAmountInfo;
