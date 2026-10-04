import { render, screen, waitFor, within } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { http, HttpResponse } from "msw";
import { describe, expect, it } from "vitest";

import { server } from "../../../../../tests/mocks/server";
import Purchases from "../Purchases";

function pagedResponse<T>(data: T[], overrides: Record<string, unknown> = {}) {
  return {
    page: 1,
    pageSize: 10,
    totalPages: 1,
    totalCount: data.length,
    hasNextPage: false,
    hasPreviousPage: false,
    data,
    ...overrides,
  };
}

const milk = {
  id: "item-1",
  name: "Milk",
  categoryId: "category-1",
  categoryName: "Dairy",
  unit: "Liter",
  minPrice: {
    price: 2,
    currency: "NZD",
    date: "2026-01-10",
    shopId: "shop-1",
    shopName: "PAKnSAVE",
  },
  maxPrice: {
    price: 4,
    currency: "NZD",
    date: "2026-03-15",
    shopId: "shop-2",
    shopName: "New World",
  },
};

const bread = {
  ...milk,
  id: "item-2",
  name: "Bread",
  categoryName: "Bakery",
  unit: "Piece",
};

function mockPurchases(
  data: unknown[] = [milk, bread],
  overrides: Record<string, unknown> = {},
) {
  const requests: URL[] = [];

  server.use(
    http.get("api/categories", () =>
      HttpResponse.json([{ id: "category-1", name: "Dairy" }]),
    ),
    http.get("api/purchases", ({ request }) => {
      requests.push(new URL(request.url));
      return HttpResponse.json(pagedResponse(data, overrides));
    }),
  );

  return requests;
}

function mockPrices(
  prices: unknown[],
  overrides: Record<string, unknown> = {},
) {
  const requests: URL[] = [];

  server.use(
    http.get("api/purchases/:id", ({ request }) => {
      requests.push(new URL(request.url));
      return HttpResponse.json(
        pagedResponse(prices, { pageSize: 5, ...overrides }),
      );
    }),
  );

  return requests;
}

const price = (amount: number, date: string, shopName: string) => ({
  price: amount,
  currency: "NZD",
  date,
  shopId: null,
  shopName,
});

describe("Purchases", () => {
  it("shows item info with min and max price lines", async () => {
    const requests = mockPurchases();

    render(<Purchases />);

    expect(await screen.findByText("Milk")).toBeInTheDocument();
    expect(screen.getByText("Bread")).toBeInTheDocument();
    expect(screen.getByText("Liter")).toBeInTheDocument();
    expect(screen.getByText("Dairy")).toBeInTheDocument();

    const [minLine] = screen.getAllByLabelText("Min price");
    expect(within(minLine).getByText("PAKnSAVE")).toBeInTheDocument();
    expect(within(minLine).getByText(/\$2\.00/)).toBeInTheDocument();
    expect(within(minLine).getByText("10 Jan 2026")).toBeInTheDocument();

    const [maxLine] = screen.getAllByLabelText("Max price");
    expect(within(maxLine).getByText("New World")).toBeInTheDocument();
    expect(within(maxLine).getByText(/\$4\.00/)).toBeInTheDocument();
    expect(within(maxLine).getByText("15 Mar 2026")).toBeInTheDocument();

    expect(requests[0].searchParams.get("currency")).toBe("NZD");
    expect(requests[0].searchParams.get("page")).toBe("1");
  });

  it("shows error state", async () => {
    server.use(
      http.get("api/purchases", () => new HttpResponse(null, { status: 500 })),
    );

    render(<Purchases />);

    expect(
      await screen.findByText("Failed to load purchases."),
    ).toBeInTheDocument();
  });

  it("shows empty state", async () => {
    mockPurchases([], { totalPages: 0 });

    render(<Purchases />);

    expect(await screen.findByText("No purchases found.")).toBeInTheDocument();
  });

  it("shows loading state", async () => {
    server.use(
      http.get("api/purchases", async () => {
        await new Promise((resolve) => setTimeout(resolve, 100));
        return HttpResponse.json(pagedResponse([]));
      }),
    );

    render(<Purchases />);

    expect(await screen.findByText("Loading...")).toBeInTheDocument();
  });

  it("requests the next page when pagination is used", async () => {
    const user = userEvent.setup();
    const requests = mockPurchases([milk], {
      totalPages: 2,
      hasNextPage: true,
    });

    render(<Purchases />);
    await screen.findByText("Milk");

    await user.click(screen.getAllByRole("button", { name: "Next page" })[0]);

    await screen.findByText("Milk");
    expect(requests.at(-1)?.searchParams.get("page")).toBe("2");
  });

  it("searches without pressing Apply", async () => {
    const user = userEvent.setup();
    const requests = mockPurchases();

    render(<Purchases />);
    await screen.findByText("Milk");

    await user.type(screen.getByLabelText("Search"), "  milk ");

    await waitFor(() =>
      expect(requests.at(-1)?.searchParams.get("searchString")).toBe("milk"),
    );
    expect(requests.at(-1)?.searchParams.get("page")).toBe("1");
  });

  it("sends selected filters on Apply and resets them on clear", async () => {
    const user = userEvent.setup();
    const requests = mockPurchases();

    render(<Purchases />);
    await screen.findByText("Milk");

    await user.type(screen.getByLabelText("Search"), "milk");
    await user.type(screen.getByLabelText("From"), "2026-01-01");
    await user.type(screen.getByLabelText("To"), "2026-02-01");
    await user.click(screen.getByRole("button", { name: /Category/ }));
    await user.click(await screen.findByLabelText("Dairy"));
    await user.click(screen.getByRole("button", { name: "Apply" }));

    await waitFor(() => {
      const params = requests.at(-1)!.searchParams;
      expect(params.get("searchString")).toBe("milk");
      expect(params.get("fromDate")).toBe("2026-01-01");
      expect(params.get("toDate")).toBe("2026-02-01");
      expect(params.getAll("categoryIds")).toEqual(["category-1"]);
    });

    await user.click(screen.getByRole("button", { name: "Clear all" }));

    await waitFor(() => {
      const cleared = requests.at(-1)!.searchParams;
      expect(cleared.has("searchString")).toBe(false);
      expect(cleared.has("fromDate")).toBe(false);
      expect(cleared.has("categoryIds")).toBe(false);
    });
    expect(screen.getByLabelText("Search")).toHaveValue("");
  });
  it("does not apply a period where 'To' is not after 'From'", async () => {
    const user = userEvent.setup();
    const requests = mockPurchases();

    render(<Purchases />);
    await screen.findByText("Milk");
    const requestsCount = requests.length;

    await user.type(screen.getByLabelText("From"), "2026-02-01");
    await user.type(screen.getByLabelText("To"), "2026-01-01");

    expect(screen.getByRole("alert")).toBeInTheDocument();
    expect(screen.getByRole("button", { name: "Apply" })).toBeDisabled();
    expect(requests).toHaveLength(requestsCount);
  });

  it("opens prices history and requests it by item id", async () => {
    const user = userEvent.setup();
    mockPurchases([milk]);
    const priceRequests = mockPrices([
      price(3, "2026-03-01", "New World"),
      price(2, "2026-01-10", "PAKnSAVE"),
    ]);

    render(<Purchases />);
    await screen.findByText("Milk");

    await user.click(
      screen.getByRole("button", { name: "Show prices history of Milk" }),
    );

    const history = await screen.findByRole("region", {
      name: "Prices history of Milk",
    });
    expect(await within(history).findByText("1 Mar 2026")).toBeInTheDocument();
    expect(within(history).getByText(/\$3\.00/)).toBeInTheDocument();
    expect(within(history).getByText("PAKnSAVE")).toBeInTheDocument();

    expect(priceRequests[0].pathname).toBe("/api/purchases/item-1");
    expect(priceRequests[0].searchParams.get("sortType")).toBe("DateDesc");

    await user.click(
      screen.getByRole("button", { name: "Hide prices history of Milk" }),
    );
    expect(
      screen.queryByRole("region", { name: "Prices history of Milk" }),
    ).not.toBeInTheDocument();
  });

  it("sorts prices history by date and amount", async () => {
    const user = userEvent.setup();
    mockPurchases([milk]);
    const priceRequests = mockPrices([price(3, "2026-03-01", "New World")]);

    render(<Purchases />);
    await user.click(
      await screen.findByRole("button", { name: /Show prices history/ }),
    );
    const history = await screen.findByRole("region");

    const sortBy = async (name: string) => {
      await user.click(await within(history).findByRole("button", { name }));
    };
    const lastSort = () => priceRequests.at(-1)?.searchParams.get("sortType");

    await within(history).findByText("1 Mar 2026");

    await sortBy(/Date/);
    await within(history).findByText("1 Mar 2026");
    expect(lastSort()).toBe("DateAsc");

    await sortBy(/Amount/);
    await within(history).findByText("1 Mar 2026");
    expect(lastSort()).toBe("PriceAsc");

    await sortBy(/Amount/);
    await within(history).findByText("1 Mar 2026");
    expect(lastSort()).toBe("PriceDesc");

    expect(
      within(history).getByRole("columnheader", { name: /Amount/ }),
    ).toHaveAttribute("aria-sort", "descending");
  });

  it("paginates prices history independently", async () => {
    const user = userEvent.setup();
    const purchaseRequests = mockPurchases([milk]);
    const priceRequests = mockPrices([price(3, "2026-03-01", "New World")], {
      totalPages: 2,
      hasNextPage: true,
    });

    render(<Purchases />);
    await user.click(
      await screen.findByRole("button", { name: /Show prices history/ }),
    );
    const history = await screen.findByRole("region");
    await within(history).findByText("1 Mar 2026");

    await user.click(
      within(history).getByRole("button", { name: "Next page" }),
    );

    await within(history).findByText("1 Mar 2026");
    expect(priceRequests.at(-1)?.searchParams.get("page")).toBe("2");
    expect(purchaseRequests).toHaveLength(1);
  });

  it("passes the page period to prices history", async () => {
    const user = userEvent.setup();
    mockPurchases([milk]);
    const priceRequests = mockPrices([price(3, "2026-03-01", "New World")]);

    render(<Purchases />);
    await screen.findByText("Milk");

    await user.type(screen.getByLabelText("From"), "2026-01-01");
    await user.type(screen.getByLabelText("To"), "2026-04-01");
    await user.click(screen.getByRole("button", { name: "Apply" }));

    await user.click(
      await screen.findByRole("button", { name: /Show prices history/ }),
    );
    await screen.findByRole("region");

    await waitFor(() => expect(priceRequests.length).toBeGreaterThan(0));
    const params = priceRequests.at(-1)!.searchParams;
    expect(params.get("fromDate")).toBe("2026-01-01");
    expect(params.get("toDate")).toBe("2026-04-01");
  });

  it("shows error and empty states in prices history", async () => {
    const user = userEvent.setup();
    mockPurchases([milk, bread]);
    server.use(
      http.get("api/purchases/:id", ({ params }) =>
        params.id === "item-1"
          ? new HttpResponse(null, { status: 500 })
          : HttpResponse.json(pagedResponse([], { pageSize: 5 })),
      ),
    );

    render(<Purchases />);
    await user.click(
      await screen.findByRole("button", {
        name: "Show prices history of Milk",
      }),
    );
    expect(
      await screen.findByText("Failed to load prices history."),
    ).toBeInTheDocument();

    await user.click(
      screen.getByRole("button", { name: "Show prices history of Bread" }),
    );
    expect(
      await screen.findByText("No prices found for this period."),
    ).toBeInTheDocument();
  });
});
