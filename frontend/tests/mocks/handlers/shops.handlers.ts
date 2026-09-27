import { http, HttpResponse } from "msw";

export const shopsHandlers = [
  http.get("api/shops", () => {
    return HttpResponse.json({
      page: 1,
      pageSize: 50,
      totalPages: 1,
      totalCount: 0,
      hasNextPage: false,
      hasPreviousPage: false,
      data: [],
    });
  }),
];
