import { useState } from "react";

import { apiClient } from "../../../api/api";
import { useAuth } from "../hooks/useAuth";

function LoginForm() {
  const { login, logout } = useAuth();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  async function handleSubmit(event: React.SubmitEvent) {
    event.preventDefault();
    setError("");

    try {
      await login(email, password);
    } catch {
      setError("Invalid email or password");
    }
  }

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <h2>Login</h2>

        <div>
          <label htmlFor="email">Email</label>
          <input
            id="email"
            type="email"
            value={email}
            onChange={(event) => {
              setEmail(event.target.value);
            }}
          />
        </div>

        <div>
          <label htmlFor="password">Password</label>
          <input
            id="password"
            type="password"
            value={password}
            onChange={(event) => {
              setPassword(event.target.value);
            }}
          />
        </div>

        {error && <div>{error}</div>}

        <button type="submit">Login</button>

        <button
          type="button"
          onClick={() => {
            async function out() {
              try {
                await logout();
              } catch {
                setError("logout is failed");
              }
            }
            out();
          }}
        >
          Logout
        </button>

        <button
          type="button"
          onClick={() => {
            async function get() {
              try {
                const response = await apiClient.get("api/expenses");
                const data = await response.json();
                // TODO: Implement GET expenses
                console.log("expenses:", data);
              } catch {
                setError("Failed to get expenses");
              }
            }
            get();
          }}
        >
          Get expenses
        </button>
      </form>
    </div>
  );
}

export default LoginForm;
