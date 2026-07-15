# Users Table

Stores information about an user

## Columns
| Column | Type | Nullable | Description |
|---|---|---|---|
| Id | UUID | No | Primary key |
| FirstName | varchar(128) | No | User first name |
| LastName | varchar(128) | No | User last name |
| Email | varchar(128) | Yes | User email |
| Phone | varchar(128) | Yes | User phone |
| CreatedAt | datetime | Yes | Update timestamp |
| UpdatedAt | datetime | Yes | Update timestamp |
| isDeleted | bool | No | Is record deleted |

### Constraints

Primary Key:
- Id

Foreign Keys:
- CreatedBy -> Users.Id
- UpdatedBy -> Users.Id


### Relationships


### Business Rules

- 

