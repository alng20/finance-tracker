import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { http, HttpResponse } from "msw";
import { describe, expect, it, vi } from "vitest";

import { server } from "../../../../../../tests/mocks/server";
import AddExpenseModal from "../AddExpenseModal";

describe("AddExpenseModal", () => {
  it("creates an expense when the user submits valid data", async () => {
    const user = userEvent.setup();

    const onClose = vi.fn();
    const onCreated = vi.fn();

    let requestBody: unknown;

    server.use(
      http.get("*/api/shops/search", ({ request }) => {
        const url = new URL(request.url);
        const searchString = url.searchParams.get("searchString");

        expect(searchString).toBe("PAKnSAVE");

        return HttpResponse.json([
          {
            id: "shop-1",
            name: "PAKnSAVE",
            retailerId: "retailer-1",
            retailerName: "PAKnSAVE",
            country: "New Zealand",
            city: "Wellington",
          },
        ]);
      }),

      http.post("*/api/expenses", async ({ request }) => {
        requestBody = await request.json();

        return HttpResponse.json({}, { status: 200 });
      }),
    );

    render(<AddExpenseModal onClose={onClose} onCreated={onCreated} />);

    await user.clear(screen.getByLabelText("Date"));
    await user.type(screen.getByLabelText("Date"), "2026-09-27");

    await user.type(screen.getByLabelText("Shop"), "PAKnSAVE");

    await user.click(
      await screen.findByRole("button", {
        name: "PAKnSAVE",
      }),
    );

    await user.type(screen.getByLabelText("Amount"), "123");

    await user.selectOptions(screen.getByLabelText("Currency"), "NZD");

    await user.click(
      screen.getByRole("button", {
        name: "Add",
      }),
    );

    expect(requestBody).toEqual({
      sharedGroupId: null,
      shopId: "shop-1",
      totalAmount: 123,
      currency: "NZD",
      expenseDate: "2026-09-27",
      details: [],
    });

    expect(onCreated).toHaveBeenCalledOnce();
    expect(onClose).toHaveBeenCalledOnce();
  });
});
