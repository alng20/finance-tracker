# Shops Table

Stores information about a shop

## Columns
| Column | Type | Nullable | Description |
|---|---|---|---|
| Id | UUID | No | Primary key |
| RetailerId | varchar(128) | No | Retailer of shop |
| Address | varchar(500) | No | Shop address |
| CreatedAt | datetime | No | Creation timestamp |
| CreatedBy | UUID | No | User who created a record |
| UpdatedAt | datetime | Yes | Update timestamp |
| UpdatedBy | UUID | Yes | Last User who updated a record |
| isDeleted | bool | No | Is record deleted |

### Constraints

Primary Key:
- Id

Foreign Keys:
- RetailerId -> Retailers.Id
- CreatedBy -> Users.Id
- UpdatedBy -> Users.Id


### Relationships

Retailers (1) ---- (*) Shops

Users (1) ---- (*) Shops


### Business Rules

- 

