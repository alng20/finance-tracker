import { http, HttpResponse } from "msw";

export const purchasesHandlers = [
  http.get("api/purchases", () => {
    return HttpResponse.json({
      page: 1,
      pageSize: 10,
      totalPages: 1,
      totalCount: 0,
      hasNextPage: false,
      hasPreviousPage: false,
      data: [],
    });
  }),
  http.get("api/purchases/:id", () => {
    return HttpResponse.json({
      page: 1,
      pageSize: 5,
      totalPages: 1,
      totalCount: 0,
      hasNextPage: false,
      hasPreviousPage: false,
      data: [],
    });
  }),
];
