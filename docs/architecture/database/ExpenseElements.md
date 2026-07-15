# ExpenseElements Table

Stores information about an individual element from expanse payment

## Columns
| Column | Type | Nullable | Description |
|---|---|---|---|
| Id | UUID | No | Primary key |
| ExpenseId | UUID | No | Expense that element belongs |
| ItemId | UUID | No | Item decribes an element |
| ActualPrice | decimal(18,2) | No | Actual element price |
| Discount | decimal(5, 2) | Yes | Discount percent |
| TotalPrice | decimal(18,2) | No | Total element price with discount |
| Currency | enum | No | Price currency |
| Quantity | decimal(5, 2) | No | Quantity of element |
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
- ExpenseId -> Expenses.Id
- ItemId -> Items.Id
- CreatedBy -> Users.Id
- UpdatedBy -> Users.Id


### Relationships

Expenses (1) ---- (*) ExpenseElements

Items (1) ---- (*) Expenses

Users (1) ---- (*) ExpenseElements


### Business Rules

- ActualPrice stores price without discount.
- TotalPrice stores price with discount if discount exists.
- Discount range is from 0% to 100%, NULL is 0%

