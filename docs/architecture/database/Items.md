# Items Table

Stores information about an item

## Columns
| Column | Type | Nullable | Description |
|---|---|---|---|
| Id | UUID | No | Primary key |
| Name | varchar(128) | No | Item name |
| CategoryId | UUID | No | Item category |
| Unit | enum | No | Unit of measurement |
| CreatedAt | datetime | No | Creation timestamp |
| CreatedBy | UUID | No | User who created a record |
| UpdatedAt | datetime | Yes | Update timestamp |
| UpdatedBy | UUID | Yes | Last User who updated a record |
| isDeleted | bool | No | Is record deleted |

### Constraints

Primary Key:
- Id

Foreign Keys:
- CategoryId -> ItemCategories.Id
- CreatedBy -> Users.Id
- UpdatedBy -> Users.Id


### Relationships

Categories (1) ---- (*) Items

Users (1) ---- (*) Items


### Business Rules

- 

