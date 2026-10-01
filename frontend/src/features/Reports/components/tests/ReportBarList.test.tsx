import { render, screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";

import ReportBarList from "../ReportBarList";

describe("ReportBarList", () => {
  it("renders the empty message when there are no items", () => {
    render(
      <ReportBarList
        items={[]}
        amountFormatter={(amount) => `$${amount}`}
        emptyMessage="Nothing to show."
      />,
    );

    expect(screen.getByText("Nothing to show.")).toBeInTheDocument();
  });

  it("renders a row with the formatted amount for each item", () => {
    render(
      <ReportBarList
        items={[
          { key: "category-1", label: "Food", amount: 10 },
          { key: "category-2", label: "Tech", amount: 20 },
        ]}
        amountFormatter={(amount) => `$${amount}`}
        emptyMessage="Nothing to show."
      />,
    );

    expect(screen.getByText("Food")).toBeInTheDocument();
    expect(screen.getByText("$10")).toBeInTheDocument();
    expect(screen.getByText("Tech")).toBeInTheDocument();
    expect(screen.getByText("$20")).toBeInTheDocument();
  });
});
