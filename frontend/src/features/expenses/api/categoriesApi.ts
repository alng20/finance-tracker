import type { Category } from "../types/Category";

const API_URL = "http://localhost:5067/api/categories";

export async function getCategories(): Promise<Category[]> {
  const response = await fetch(API_URL);

  if (!response.ok) {
    throw new Error("Failed to fetch categories");
  }

  const data = await response.json();
  return data;
}
