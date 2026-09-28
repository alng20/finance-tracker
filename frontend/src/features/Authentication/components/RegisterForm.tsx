import "./css/RegisterPage.css";

import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { register } from "../api/authApi";
import type { RegisterRequest } from "../types/RegisterRequest";
import type { RegistrationData } from "../types/RegistrationData";
import {
  type RegisterValidationErrors,
  validateRegistrationData,
} from "../validation/RegisterValidation";

function RegisterForm() {
  const navigate = useNavigate();

  const [data, setData] = useState<RegistrationData>({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    phone: null,
  });

  const [confirmPassword, setConfirmPassword] = useState("");
  const [errors, setErrors] = useState<RegisterValidationErrors>({});
  const [registerError, setRegisterError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  function setDataField(key: keyof RegistrationData, value: string) {
    setData((current) => ({
      ...current,
      [key]: value,
    }));
  }

  function isConfirmPasswordValid(): boolean {
    return confirmPassword === "" || confirmPassword === data.password;
  }

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setRegisterError("");

    if (confirmPassword !== data.password) {
      return;
    }

    const validationErrors = validateRegistrationData(data);
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }
    setErrors({});

    try {
      await register({
        firstName: data.firstName.trim(),
        lastName: data.lastName.trim(),
        email: data.email.trim(),
        password: data.password,
        phone: data.phone?.trim() || null,
      } as RegisterRequest);

      setSuccessMessage("Registration successful! Redirecting to login...");
      setTimeout(() => {
        navigate("/login", { replace: true });
      }, 2000);
    } catch (error) {
      const message =
        error instanceof Error
          ? error.message
          : "Failed to register. Please try again.";

      setRegisterError(message);
    }
  }

  if (successMessage) {
    return (
      <div className="register-form-shell">
        <div className="register-form register-form__success">
          <h2>Success</h2>
          <p className="register-form__success-message">{successMessage}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="register-form-shell">
      <form className="register-form" onSubmit={handleSubmit}>
        <h2>Register</h2>
        <p className="register-form__hint">Create your account.</p>

        <div className="register-form__row">
          <div className="register-form__field">
            <label htmlFor="firstName">First Name</label>
            <input
              id="firstName"
              type="text"
              value={data.firstName}
              onChange={(event) =>
                setDataField("firstName", event.target.value)
              }
              required
            />
            {errors.firstName && (
              <span className="register-form__field-error">
                {errors.firstName}
              </span>
            )}
          </div>

          <div className="register-form__field">
            <label htmlFor="lastName">Last Name</label>
            <input
              id="lastName"
              type="text"
              value={data.lastName}
              onChange={(event) => setDataField("lastName", event.target.value)}
              required
            />
            {errors.lastName && (
              <span className="register-form__field-error">
                {errors.lastName}
              </span>
            )}
          </div>
        </div>

        <div className="register-form__field">
          <label htmlFor="email">Email</label>
          <input
            id="email"
            type="email"
            value={data.email}
            onChange={(event) => setDataField("email", event.target.value)}
            required
          />
          {errors.email && (
            <span className="register-form__field-error">{errors.email}</span>
          )}
        </div>

        <div className="register-form__row">
          <div className="register-form__field">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              value={data.password}
              onChange={(event) => setDataField("password", event.target.value)}
              required
            />
            {errors.password && (
              <span className="register-form__field-error">
                {errors.password}
              </span>
            )}
          </div>

          <div className="register-form__field">
            <label htmlFor="confirm-password">Confirm Password</label>
            <input
              id="confirm-password"
              type="password"
              value={confirmPassword}
              onChange={(event) => setConfirmPassword(event.target.value)}
              required
            />
            {!isConfirmPasswordValid() && (
              <span className="register-form__field-error">
                Passwords do not match
              </span>
            )}
          </div>
        </div>

        <div className="register-form__field">
          <label htmlFor="phone">Phone</label>
          <input
            id="phone"
            type="text"
            value={data.phone ?? ""}
            onChange={(event) => setDataField("phone", event.target.value)}
          />
          {errors.phone && (
            <span className="register-form__field-error">{errors.phone}</span>
          )}
        </div>

        {registerError && (
          <div className="register-form__error">{registerError}</div>
        )}

        <div className="register-form__actions">
          <button className="register-form__submit" type="submit">
            Register
          </button>

          <button
            className="register-form__cancel"
            type="button"
            onClick={() => navigate(-1)}
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  );
}

export default RegisterForm;
