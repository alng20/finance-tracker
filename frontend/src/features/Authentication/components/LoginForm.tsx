import "./css/LoginForm.css";
import { useState } from "react";

import { useAuth } from "../hooks/useAuth";
import { useNavigate } from "react-router-dom";

function LoginForm() {
  const navigate = useNavigate();

  const { login } = useAuth();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  async function handleSubmit(event: React.SubmitEvent) {
    event.preventDefault();
    setError("");

    try {
      await login(email, password);
      navigate("/dashboard", { replace: true });
    } catch {
      setError("Invalid email or password");
    }
  }

  return (
    <div className="login-form-shell">
      <form className="login-form" onSubmit={handleSubmit}>
        <h2>Login</h2>
        <p className="login-form__hint">Welcome back.</p>

        <div className="login-form__field">
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

        <div className="login-form__field">
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

        {error && <div className="login-form__error">{error}</div>}

        <button className="login-form__submit" type="submit">
          Login
        </button>

        <button
          className="login-form__register"
          type="button"
          onClick={() => {
            navigate("/register");
          }}
        >
          Register
        </button>
      </form>
    </div>
  );
}

export default LoginForm;
