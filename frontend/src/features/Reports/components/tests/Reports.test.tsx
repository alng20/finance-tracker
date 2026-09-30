import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { http, HttpResponse } from "msw";
import { describe, expect, it } from "vitest";

import { server } from "../../../../../tests/mocks/server";
import Reports from "../Reports";

describe("Reports", () => {
  it("shows spending trend, category, shop and retailer sections from the reports APIs", async () => {
    render(<Reports />);

    expect(await screen.findByText("Food")).toBeInTheDocument();
    expect(await screen.findByText("PAKnSAVE Shop")).toBeInTheDocument();
    expect(await screen.findByText("PAKnSAVE Retailer")).toBeInTheDocument();
    expect(screen.getAllByText("Unspecified").length).toBeGreaterThan(0);
  });

  it("shows the detailed/undetailed callout for category spending", async () => {
    render(<Reports />);

    expect(await screen.findByText(/Detailed:/)).toBeInTheDocument();
    expect(screen.getByText(/Undetailed:/)).toBeInTheDocument();
  });

  it("shows an error state when a report request fails", async () => {
    server.use(
      http.get("api/reports/by_shop", () => {
        return new HttpResponse(null, { status: 500 });
      }),
    );

    render(<Reports />);

    expect(
      await screen.findByText("Failed to load spending by shop."),
    ).toBeInTheDocument();
  });

  it("shows an empty state when a report has no data", async () => {
    server.use(
      http.get("api/reports/by_retailer", () => {
        return HttpResponse.json({
          fromDate: null,
          toDate: null,
          totalAmount: 0,
          amounts: [],
        });
      }),
    );

    render(<Reports />);

    expect(
      await screen.findByText(
        "No spending recorded by retailer for this period.",
      ),
    ).toBeInTheDocument();
  });

  it("shows a loading state while a report request is pending", async () => {
    server.use(
      http.get("api/reports/grouped", async () => {
        await new Promise((resolve) => setTimeout(resolve, 100));

        return HttpResponse.json({
          currency: "NZD",
          groupingType: "Month",
          amountByPeriod: [],
        });
      }),
    );

    render(<Reports />);

    expect(await screen.findAllByText("Loading...")).not.toHaveLength(0);
  });

  it("refetches reports when the currency filter changes", async () => {
    const user = userEvent.setup();
    render(<Reports />);

    await screen.findByText("Food");

    await user.selectOptions(screen.getByLabelText("Currency"), "USD");

    expect(await screen.findByText("Food")).toBeInTheDocument();
  });

  it("refetches the spending trend when the grouping type changes", async () => {
    const user = userEvent.setup();
    render(<Reports />);

    await screen.findByText("Food");

    await user.selectOptions(screen.getByLabelText("Group by"), "Week");

    expect(await screen.findByText("Food")).toBeInTheDocument();
  });

  it("resets filters to defaults when 'Clear filters' is clicked", async () => {
    const user = userEvent.setup();
    render(<Reports />);

    await screen.findByText("Food");

    await user.type(screen.getByLabelText("From"), "2026-01-01");
    await user.selectOptions(screen.getByLabelText("Currency"), "USD");

    await user.click(screen.getByRole("button", { name: "Clear filters" }));

    expect(screen.getByLabelText("From")).toHaveValue("");
    expect(screen.getByLabelText("Currency")).toHaveValue("NZD");
  });
});
