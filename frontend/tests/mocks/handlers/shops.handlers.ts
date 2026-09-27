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
      data: [
        {
          id: "shop-1",
          name: "PAKnSAVE",
          retailerId: "retailer-1",
          retailerName: "PAKnSAVE",
          country: "New Zealand",
          city: "Wellington",
        },
      ],
    });
  }),
];
