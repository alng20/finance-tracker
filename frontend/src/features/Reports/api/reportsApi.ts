import { apiClient } from "../../../api/api";
import type { Currency } from "../../shared/types/Defs";
import type {
  GetGroupedAmountResponse,
  ReportGroupingType,
} from "../types/GetGroupedAmountResponse";

const API_REPORTS = "api/reports";

export async function getGroupedAmount(
  currency: Currency,
  groupingType: ReportGroupingType,
  fromDate?: string,
  toDate?: string,
): Promise<GetGroupedAmountResponse> {
  const params = new URLSearchParams({
    currency,
    groupingType,
    ...(fromDate && { fromDate }),
    ...(toDate && { toDate }),
  });

  return apiClient.get<GetGroupedAmountResponse>(
    `${API_REPORTS}/grouped?${params.toString()}`,
  );
}
