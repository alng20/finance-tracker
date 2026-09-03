export type UserRole = "User" | "Admin";

export type GetProfileResponse = {
  firstName: string;
  lastName: string;
  role: UserRole;
};
