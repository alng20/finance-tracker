# SharedGroupMembers Table

Stores information about a member of group

## Columns
| Column | Type | Nullable | Description |
|---|---|---|---|
| GroupId | UUID | No | Group id that member belongs |
| UserId | UUID | No | User id of member |
| CanRead | bool | No | if user can read expenses |
| CanWrite | bool | No | if user can write expenses |
| CanAdd | bool | No | if user can add expenses |
| CanDelete | bool | No | if user can delete expenses |
| JoinedAt | datetime | No | Join timestamp |
| JoinedBy | UUID | No | User who joined a member |
| UpdatedAt | datetime | Yes | Update timestamp |
| UpdatedBy | UUID | Yes | Last User who updated a record |
| isDeleted | bool | No | Is record deleted |

### Constraints

Primary Key:
- GroupId + UserId

Foreign Keys:
- GroupId -> Groups.Id
- UserId -> Users.Id
- CreatedBy -> Users.Id
- UpdatedBy -> Users.Id


### Relationships

Groups (1) ---- (*) SharedGroupMembers

Users (1) ---- (*) SharedGroupMembers


### Business Rules

- Member's permissions can be updated
- Shared group's owner has all permissions

