# GroupMembers Table

Stores information about a member of group

## Columns
| Column | Type | Nullable | Description |
|---|---|---|---|
| Id | UUID | No | Primary key |
| GroupId | UUID | No | Group id that member belongs |
| UserId | UUID | No | User id of member |
| Role | enum | No | Member role |
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
- Id

Foreign Keys:
- GroupId -> Groups.Id
- UserId -> Users.Id
- CreatedBy -> Users.Id
- UpdatedBy -> Users.Id


### Relationships

Groups (1) ---- (*) GroupMembers
Users (1) ---- (*) GroupMembers


### Business Rules

- 

