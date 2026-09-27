import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { http, HttpResponse } from "msw";
import { describe, expect, it } from "vitest";

import { server } from "../../../../../tests/mocks/server";
import Expenses from "../Expenses";

describe("Expenses", () => {
  it("Show expenses by GET API", async () => {
    render(<Expenses />);

    const paknsave = await screen.findByText("PAKnSAVE");
    const newworld = await screen.findByText("New World");

    expect(paknsave).toBeInTheDocument();
    expect(newworld).toBeInTheDocument();
  });

  it("Show error state by GET API", async () => {
    server.use(
      http.get("*/api/expenses", () => {
        return new HttpResponse(null, { status: 500 });
      }),
    );

    render(<Expenses />);

    expect(
      await screen.findByText("Failed to load expenses."),
    ).toBeInTheDocument();
  });

  it("Show empty state by GET API", async () => {
    server.use(
      http.get("*/api/expenses", () => {
        return HttpResponse.json({
          page: 1,
          pageSize: 10,
          totalPages: 0,
          totalCount: 0,
          hasNextPage: false,
          hasPreviousPage: false,
          data: [],
          metadata: {
            summaryAmount: 0,
            currency: "NZD",
          },
        });
      }),
    );

    render(<Expenses />);

    expect(
      await screen.findByText("You don't have any expenses yet."),
    ).toBeInTheDocument();
  });

  it("Show loading state by GET API", async () => {
    server.use(
      http.get("*/api/expenses", async () => {
        await new Promise((resolve) => setTimeout(resolve, 100));

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
    );

    render(<Expenses />);

    expect(await screen.findByText("Loading...")).toBeInTheDocument();
  });

  it("open/close Add Expense modal when user clicks 'Add Expense'/'Cancel' button", async () => {
    const user = userEvent.setup();

    render(<Expenses />);

    const addExpenseButton = await screen.findByRole("button", {
      name: "Add Expense",
    });

    await user.click(addExpenseButton);
    expect(
      screen.getByRole("heading", { name: "New Expense" }),
    ).toBeInTheDocument();

    await user.click(screen.getByRole("button", { name: "Cancel" }));
    expect(
      screen.queryByRole("heading", { name: "New Expense" }),
    ).not.toBeInTheDocument();
  });
});
