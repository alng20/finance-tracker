# Delete Behavior for relationships

## General Rules

- Financial records (`Expense`, `ExpenseDetail`) should not be physically deleted accidentally.
- Soft Delete is preferred for business entities where historical data is important.
- Cascade Delete is used only for child entities that cannot exist independently.
- Restrict is preferred for relationships involving historical or reporting data.
- SetNull is used only for optional relationships where the dependent entity can exist without the principal entity.

# Delete Behavior Strategy

| Principal Entity | Dependent Entity | Relationship | Delete Behavior | Soft Delete | Reason |
|---|---|---|---|---|---|
| User | Expense | User owns financial history | Restrict | Yes | Expenses contain important financial data and must not be deleted accidentally. User deletion is handled through Soft Delete. |
| SharedGroup | Expense | Group can contain shared expenses | Restrict | Yes | Expenses should preserve history even if a group is archived or deleted. |
| Shop | Expense | Shop is referenced by historical expenses | Restrict | Yes | Purchase history should keep information about where the expense was made. |
| Expense | ExpenseDetail | Details belong only to an expense | Cascade | No | ExpenseDetail has no independent meaning without Expense. Removing an expense removes its details. |
| Item | ExpenseDetail | Item is referenced by historical purchases | Restrict | Yes | Deleting an item must not remove historical purchase details and prices. |
| Retailer | Shop | Retailer owns shops | Restrict | Yes | Historical shop information should be preserved. |
| User | SharedGroup | User creates and owns groups | Restrict | Yes | Ownership changes are business operations. Groups should not be deleted automatically with users. |
| User | SharedGroupMember | User membership in groups | Restrict | Yes | Membership history may be required for audit and reporting. |
| SharedGroup | SharedGroupMember | Members belong to a group | Cascade | No | Membership records cannot exist without a group. |
| Category | Item | SetNull | Item can optionally have a category | No | Category is a classification entity. Removing a category should only detach items from it, not hide the category through Soft Delete. |

## Soft Delete Strategy

Soft Delete is used for entities where historical information has business value.

Entities with Soft Delete:
- User
- SharedGroup
- SharedGroupMember
- Shop
- Retailer
- Item
- Category

Soft Delete implementation:
- Entity contains `DeletedAt` nullable property.
- Deleted entities are excluded from normal queries using EF Core `HasQueryFilter`.
- Deleted records can be accessed using `IgnoreQueryFilters()` when required for administration or auditing.
