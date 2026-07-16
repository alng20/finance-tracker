# Expenses Table

Stores payment information by user

## Columns
| Column | Type | Nullable | Description |
|---|---|---|---|
| Id | UUID | No | Primary key |
| UserId | UUID | No | User who did a payment |
| SharedGroupId | UUID | Yes | Shared group id |
| ShopId | UUID | Yes | Store where payment was made |
| Date | datetime | No | Date of payment |
| TotalAmount | decimal(18,2) | No | Total expense amount |
| Currency | enum | No | Expense currency |
| Notes | varchar(500) | Yes | Additional information |
| CreatedAt | datetime | No | Creation timestamp |
| CreatedBy | UUID | No | User who created a record |
| UpdatedAt | datetime | Yes | Update timestamp |
| UpdatedBy | UUID | Yes | Last User who updated a record |
| isDeleted | bool | No | Is record deleted |

### Constraints

Primary Key:
- Id

Foreign Keys:
- UserId -> Users.Id
- SharedGroupId -> SharedGroups.Id
- ShopId -> Shops.Id
- CreatedBy -> Users.Id
- UpdatedBy -> Users.Id


### Relationships

Users (1) ---- (*) Expenses

SharedGroups (1) ---- (*) Expenses

Shops (1) ---- (*) Expenses


### Business Rules

- TotalAmount stores the final payment amount.
- TotalAmount cannot be negative
- Expense has to have an user who does a payment
- Expense can be added to a shared group
- Expense can be deleted from a shared group
