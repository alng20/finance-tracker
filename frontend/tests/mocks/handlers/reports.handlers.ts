import { http, HttpResponse } from "msw";

export const reportsHandlers = [
  http.get("api/reports/grouped", () => {
    return HttpResponse.json({
      currency: "NZD",
      groupingType: "Month",
      amountByPeriod: [
        {
          reportPeriod: { fromDate: "2026-08-01", toDate: "2026-08-31" },
          amount: 120,
        },
        {
          reportPeriod: { fromDate: "2026-09-01", toDate: "2026-09-30" },
          amount: 80,
        },
      ],
    });
  }),

  http.get("api/reports/by_category", () => {
    return HttpResponse.json({
      fromDate: null,
      toDate: null,
      totalAmount: 200,
      amounts: [
        {
          categoryId: "category-1",
          categoryName: "Food",
          totalAmount: 140,
        },
        {
          categoryId: "category-2",
          categoryName: "Tech",
          totalAmount: 40,
        },
      ],
      detailedAmount: 180,
      undetailedAmount: 20,
    });
  }),

  http.get("api/reports/by_shop", () => {
    return HttpResponse.json({
      fromDate: null,
      toDate: null,
      totalAmount: 200,
      amounts: [
        { shopId: "shop-1", shopName: "PAKnSAVE Shop", totalAmount: 150 },
        { shopId: null, shopName: "Unspecified", totalAmount: 50 },
      ],
    });
  }),

  http.get("api/reports/by_retailer", () => {
    return HttpResponse.json({
      fromDate: null,
      toDate: null,
      totalAmount: 200,
      amounts: [
        {
          retailerId: "retailer-1",
          retailerName: "PAKnSAVE Retailer",
          totalAmount: 150,
        },
        { retailerId: null, retailerName: "Unspecified", totalAmount: 50 },
      ],
    });
  }),
];
