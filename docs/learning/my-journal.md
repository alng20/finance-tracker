# **My learning journal**

newest to oldest

## Tuesday 14.07.26



## Monday 13.07.26

### Modern .NET Ecosystem
.NET is a development platform created by Microsoft. It includes the runtime (CLR), a large standard library, development tools and languages such as C#. It allows developers to build web, desktop, mobile and cloud applications.\
.NET\
├── Runtime (CLR)\
├── Base Class Library (BCL)\
├── SDK\
├── Compiler (Roslyn)\
└── Languages (C#, F#, VB)

IL -- Intermediate language\
CLR -- Common Language Runtime (Garbage Collection, Exception Handling, Thread Pool, Security, Reflection, Assembly Loading)\
JIT -- Just-In-Time (Concept of runtime compiler)

.cs -> Roslyn Compiler (out IL: .dll) -> CLR -> JIT -> Machine code -> CPU\
**BUILD**: .cs -> Roslyn -> IL\
**RUN**: IL -> CLR -> JIT -> Machine code\

### Project and Solution
**Project** is a part of solution (build unit), it is compiled into .exe or .dll file, consists of classes, logic. 
**Solution** is a workspace that groups related projects into a single development environment. It makes it easier to build, navigate, debug and manage the entire application.
We have separation projects because we need to separate responsibilities\
4 projects == 4 resposibilities


### ASP.NET Core Overview
It is a framework


### Clean Architecture
React -> HTTP Request -> ASP.NET Core -> Middleware -> Controller -> Application -> Domain -> Infrastructure -> PostgreSQL

**React** sends HTTP Request\
**HTTP Request** comes into ASP.NET Core (Controllers and etc.)\
**Middleware Pipeline** (chain of request handlers: Request -> Logging -> Authentication -> Authorization -> Exception Handling -> Controller)\
**Controller** receive HTTP Request, check Model, call Application Layer, returns HTTP Response\
**Application Layer** has business logic of my project\
**Domain** is only about business, describe entities and etc.\
**Infrastructure** connects with Database and send request to it\
**Database** creates new info, return info, answer to infra\
Then HTTP Response goes through reversed path

Infrastructure is independent module\
API -> Infra -> DB\
Worker -> Infra -> DB\
Infrastructure is part of system, not API\
API is external layer

Why is Domain the most important layer in Clean Architecture\
Because the Domain layer contains the core business rules of the application. It should be independent from external frameworks, databases and delivery mechanisms. This allows the business logic to remain stable while technical details can change.\

API -> Application -> Domain\
Infrastructure -> Application -> Domain\
Dependency Injection Rule\
Application-Interfaces-IExpenseRepository\
Infrastructure-Persistence-PostgresExpenseRepository\
