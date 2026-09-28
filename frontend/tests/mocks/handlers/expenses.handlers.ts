import { http, HttpResponse } from "msw";

export const expensesHandlers = [
  http.get("api/expenses", () => {
    return HttpResponse.json({
      page: 1,
      pageSize: 10,
      totalPages: 1,
      totalCount: 2,
      hasNextPage: false,
      hasPreviousPage: false,
      data: [
        {
          id: "expense-1",
          shopId: "shop-1",
          shopName: "PAKnSAVE",
          totalAmount: 10,
          currency: "NZD",
          expenseDate: "2026-09-27",
        },
        {
          id: "expense-2",
          shopId: "shop-2",
          shopName: "New World",
          totalAmount: 20,
          currency: "NZD",
          expenseDate: "2026-09-26",
        },
      ],
      metadata: {
        summaryAmount: 30,
        currency: "NZD",
      },
    });
  }),
];
