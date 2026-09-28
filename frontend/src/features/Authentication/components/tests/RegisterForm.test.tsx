import "@testing-library/jest-dom/vitest";

import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { StatusCodes } from "http-status-codes";
import { http, HttpResponse } from "msw";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { describe, expect, it } from "vitest";

import { server } from "../../../../../tests/mocks/server";
import { RegisterRequest } from "../../types/RegisterRequest";
import RegisterForm from "../RegisterForm";

describe("RegisterForm", () => {
  it("register OK", async () => {
    const user = userEvent.setup();

    let requestBody: unknown;

    server.use(
      http.post("*/api/auth/register", async ({ request }) => {
        requestBody = await request.json();

        return HttpResponse.json(
          {
            id: "user-1",
            firstName: "First",
            lastName: "Last",
            email: "user@ft.dev",
            role: "User",
          },
          { status: 200 },
        );
      }),
    );

    render(
      <MemoryRouter initialEntries={["/register"]}>
        <Routes>
          <Route path="/register" element={<RegisterForm />} />
          <Route path="/login" element={<div>Login page</div>} />
        </Routes>
      </MemoryRouter>,
    );

    await user.type(screen.getByLabelText("First Name"), "First");
    await user.type(screen.getByLabelText("Last Name"), "Last");
    await user.type(screen.getByLabelText("Email"), "user@ft.dev");
    await user.type(screen.getByLabelText("Password"), "password123");
    await user.type(screen.getByLabelText("Confirm Password"), "password123");

    await user.click(
      await screen.findByRole("button", {
        name: "Register",
      }),
    );

    expect(requestBody).toEqual({
      firstName: "First",
      lastName: "Last",
      email: "user@ft.dev",
      password: "password123",
      phone: null,
    });

    expect(
      await screen.findByRole("heading", {
        name: "Success",
      }),
    ).toBeInTheDocument();

    await waitFor(
      () => {
        expect(screen.getByText("Login page")).toBeInTheDocument();
      },
      { timeout: 3000 },
    );
  });

  it("register Conflict", async () => {
    const user = userEvent.setup();

    let requestBody: unknown;

    server.use(
      http.post("*/api/auth/register", async ({ request }) => {
        requestBody = await request.json();
        const email = (requestBody as RegisterRequest).email;
        return HttpResponse.json(
          {
            title: "Conflict",
            detail: `User with email ${email} already exists`,
            status: StatusCodes.CONFLICT,
          },
          {
            status: StatusCodes.CONFLICT,
          },
        );
      }),
    );

    render(
      <MemoryRouter initialEntries={["/register"]}>
        <Routes>
          <Route path="/register" element={<RegisterForm />} />
        </Routes>
      </MemoryRouter>,
    );

    await user.type(screen.getByLabelText("First Name"), "First");
    await user.type(screen.getByLabelText("Last Name"), "Last");
    await user.type(screen.getByLabelText("Email"), "user@ft.dev");
    await user.type(screen.getByLabelText("Password"), "password123");
    await user.type(screen.getByLabelText("Confirm Password"), "password123");

    await user.click(
      await screen.findByRole("button", {
        name: "Register",
      }),
    );

    expect(requestBody).toEqual({
      firstName: "First",
      lastName: "Last",
      email: "user@ft.dev",
      password: "password123",
      phone: null,
    });

    expect(
      await screen.findByText("User with email user@ft.dev already exists"),
    ).toBeInTheDocument();
  });

  it("register confirm password failed", async () => {
    const user = userEvent.setup();

    let requestSent = false;
    server.use(
      http.post("*/api/auth/register", () => {
        requestSent = true;

        return HttpResponse.json({});
      }),
    );

    render(
      <MemoryRouter initialEntries={["/register"]}>
        <Routes>
          <Route path="/register" element={<RegisterForm />} />
        </Routes>
      </MemoryRouter>,
    );

    await user.type(screen.getByLabelText("First Name"), "First");
    await user.type(screen.getByLabelText("Last Name"), "Last");
    await user.type(screen.getByLabelText("Email"), "user@ft.dev");
    await user.type(screen.getByLabelText("Password"), "password123");
    await user.type(screen.getByLabelText("Confirm Password"), "123");

    await user.click(
      await screen.findByRole("button", {
        name: "Register",
      }),
    );

    expect(requestSent).toBe(false);
    expect(
      await screen.getByText("Passwords do not match"),
    ).toBeInTheDocument();
  });

  it("register first name is empty space", async () => {
    const user = userEvent.setup();

    let requestSent = false;
    server.use(
      http.post("*/api/auth/register", () => {
        requestSent = true;

        return HttpResponse.json({});
      }),
    );

    render(
      <MemoryRouter initialEntries={["/register"]}>
        <Routes>
          <Route path="/register" element={<RegisterForm />} />
        </Routes>
      </MemoryRouter>,
    );

    await user.type(screen.getByLabelText("First Name"), " ");
    await user.type(screen.getByLabelText("Last Name"), "Last");
    await user.type(screen.getByLabelText("Email"), "user@ft.dev");
    await user.type(screen.getByLabelText("Password"), "password123");
    await user.type(screen.getByLabelText("Confirm Password"), "password123");

    await user.click(
      await screen.findByRole("button", {
        name: "Register",
      }),
    );

    expect(requestSent).toBe(false);
    expect(
      await screen.findByText("First Name is required."),
    ).toBeInTheDocument();
  });
});
