# SharedGroups Table

Stores information about a group of users

## Columns
| Column | Type | Nullable | Description |
|---|---|---|---|
| Id | UUID | No | Primary key |
| Name | varchar(128) | No | Group name |
| OwnerId | UUID | No | User id who is owner |
| CreatedAt | datetime | No | Creation timestamp |
| CreatedBy | UUID | No | User who created a record |
| UpdatedAt | datetime | Yes | Update timestamp |
| UpdatedBy | UUID | Yes | Last User who updated a record |
| isDeleted | bool | No | Is record deleted |

### Constraints

Primary Key:
- Id

Foreign Keys:
- OwnerId -> Users.Id
- CreatedBy -> Users.Id
- UpdatedBy -> Users.Id


### Relationships

Users (1) ---- (*) UserGroups


### Business Rules

- Group cannot exist without an owner
- Owner can transfere ownership
- Owner cannot leave a group without tranfering permission
- SharedGroup is deleted if owner is only user and leaves
- SharedGroup has only unique members

