import "./css/PurchasesTable.css";

import type { PurchaseDto } from "../../types/GetPurchasesResponse";
import PurchaseRow from "./PurchaseRow";

type PurchasesTableProps = {
  purchases: PurchaseDto[];
  isLoading: boolean;
  error: Error | null;
  fromDate?: string;
  toDate?: string;
};

function PurchasesTable({
  purchases,
  isLoading,
  error,
  fromDate,
  toDate,
}: PurchasesTableProps) {
  if (isLoading) {
    return <div className="purchases-table">Loading...</div>;
  }

  if (error) {
    return <div className="purchases-table">Failed to load purchases.</div>;
  }

  if (purchases.length === 0) {
    return <div className="purchases-table">No purchases found.</div>;
  }

  return (
    <div className="purchases-table">
      <div className="purchases-table__data">
        {purchases.map((purchase) => (
          <PurchaseRow
            key={purchase.id}
            purchase={purchase}
            fromDate={fromDate}
            toDate={toDate}
          />
        ))}
      </div>
    </div>
  );
}

export default PurchasesTable;
