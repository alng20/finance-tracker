import { apiClient } from "../../../api/api";
import type { GetRetailersDto } from "../types/GetRetailersDto";

const API_RETAILERS = "api/retailers";

export async function getRetailers(): Promise<GetRetailersDto[]> {
  return apiClient.get<GetRetailersDto[]>(`${API_RETAILERS}`);
}
