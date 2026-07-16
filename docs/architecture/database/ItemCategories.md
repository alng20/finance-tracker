# ExpenseCategories Table

Stores information about an expense category

## Columns
| Column | Type | Nullable | Description |
|---|---|---|---|
| Id | UUID | No | Primary key |
| Name | varchar(50) | No | Category Name |
| CreatedAt | datetime | No | Creation timestamp |
| CreatedBy | UUID | No | User who created a record |
| UpdatedAt | datetime | Yes | Update timestamp |
| UpdatedBy | UUID | Yes | Last User who updated a record |
| isDeleted | bool | No | Is record deleted |

### Constraints

Primary Key:
- Id

Foreign Keys:
- CreatedBy -> Users.Id
- UpdatedBy -> Users.Id


### Relationships

Users (1) ---- (*) ExpenseCategories


### Business Rules

- 

