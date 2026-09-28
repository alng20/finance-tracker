import type { RegistrationData } from "../types/RegistrationData";

export type RegisterValidationErrors = {
  firstName?: string;
  lastName?: string;
  email?: string;
  password?: string;
  phone?: string;
};

export function validateRegistrationData({
  firstName,
  lastName,
  email,
  password,
  phone,
}: RegistrationData): RegisterValidationErrors {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  const errors: RegisterValidationErrors = {};

  const trimmedFirstName = firstName.trim();
  if (!trimmedFirstName) {
    errors.firstName = "First Name is required.";
  } else if (trimmedFirstName.length > 100) {
    errors.firstName = "First Name must be 100 characters or fewer.";
  }

  const trimmedLastName = lastName.trim();
  if (!trimmedLastName) {
    errors.lastName = "Last Name is required.";
  } else if (trimmedLastName.length > 100) {
    errors.lastName = "Last Name must be 100 characters or fewer.";
  }

  const trimmedEmail = email.trim();
  if (!trimmedEmail) {
    errors.email = "Email is required.";
  } else if (trimmedEmail.length > 254) {
    errors.email = "Email must be 254 characters or fewer.";
  } else if (!emailRegex.test(trimmedEmail)) {
    errors.email = "Email must be a valid email address.";
  }

  if (!password) {
    errors.password = "Password is required.";
  } else if (password.length < 8) {
    errors.password = "Password must be 8 characters or more.";
  }

  if (phone && phone.length > 50) {
    errors.phone = "Phone must be 50 characters or fewer.";
  }

  return errors;
}
