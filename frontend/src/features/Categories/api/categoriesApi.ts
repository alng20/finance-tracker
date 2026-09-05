import { apiClient } from "../../../api/api";
import type { GetCategoriesDto } from "../types/GetCategoriesDto";

const API_CATEGORIES = "api/categories";

export async function getCategories(): Promise<GetCategoriesDto[]> {
  return apiClient.get<GetCategoriesDto[]>(`${API_CATEGORIES}`);
}
