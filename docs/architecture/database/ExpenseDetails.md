# ExpenseDetails Table

Stores information about an individual element from expanse payment

## Columns
| Column | Type | Nullable | Description |
|---|---|---|---|
| Id | UUID | No | Primary key |
| ExpenseId | UUID | No | Expense that element belongs |
| ItemId | UUID | No | Item decribes an element |
| TotalPrice | decimal(18,2) | No | Total price |
| Discount | decimal(5, 2) | No | Discount percent |
| UnitPrice | decimal(18,2) | No | Price of 1 unit |
| UnitDiscountPrice | decimal(18,2) | Yes | Price of 1 unit with discount |
| Quantity | decimal(5, 2) | No | Quantity of element |
| Currency | enum | No | Price currency |
| CreatedAt | datetime | No | Creation timestamp |
| CreatedBy | UUID | No | User who created a record |
| UpdatedAt | datetime | Yes | Update timestamp |
| UpdatedBy | UUID | Yes | Last User who updated a record |
| isDeleted | bool | No | Is record deleted |

### Constraints

Primary Key:
- Id

Foreign Keys:
- ExpenseId -> Expenses.Id
- ItemId -> Items.Id
- CreatedBy -> Users.Id
- UpdatedBy -> Users.Id


### Relationships

Expenses (1) ---- (*) ExpenseDetails

Items (1) ---- (*) ExpenseDetails

Users (1) ---- (*) ExpenseDetails


### Business Rules

- UnitPrice stores price without discount and calculates from TotalPrice, Discount and Quantity
- TotalPrice stores price with discount if discount exists
- TotalPrice cannot be negative
- Quantity cannot be negative
- Discount range is from 0% to 100%
- TotalPrice, Quantity and Discount can be updated

