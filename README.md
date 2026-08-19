# TodoList Backend

## Overview

A backend service built with ASP.NET Core following Clean Architecture principles.

The system provides task and TodoList management with task assignments, tagging, comments, notifications, and secure user authentication.

The project focuses on building a maintainable and scalable backend with clear separation of concerns and practical real-world features.

---

## Core Features

* JWT-based authentication and authorization
* TodoList and task management
* Task assignment to users
* Tag system with many-to-many relationships
* Task comments
* Comment notifications
* Background job processing with Hangfire (available in the `feature/background-job` branch)

---

## Architecture

The project follows Clean Architecture principles:

* **Domain** — core entities and business rules
* **Application** — application services and use-case logic
* **Infrastructure** — database access, EF Core, and Identity
* **WebApi** — controllers and API endpoints

Dependencies are structured to keep business logic independent from infrastructure concerns.

---

## Tech Stack

* C#
* ASP.NET Core
* Entity Framework Core
* SQL Server
* ASP.NET Identity
* JWT Authentication
* Hangfire

---

## Data Model

Main entities include:

* TodoLists
* Tasks
* AssignedTasks
* Comments
* Tags
* TaskTags
* Users (ASP.NET Identity)

---

## Running the Project

1. Clone the repository.
2. Configure the database connection in `appsettings.json`.
3. Apply the database migrations:

```bash
dotnet ef database update
```

4. Run the application:

```bash
dotnet run
```

---

## API Overview

### Authentication

* `POST /api/Authentication/register`
* `POST /api/Authentication/login`

### Todo Lists

* `GET /api/TodoList`
* `GET /api/TodoList/{id}`
* `GET /api/TodoList/paged`
* `POST /api/TodoList`
* `PUT /api/TodoList/{id}`
* `PATCH /api/TodoList/{id}`
* `DELETE /api/TodoList/{id}`
* `GET /api/TodoList/preview`

### Tasks

* `GET /api/TodoList/{todoListId}/Tasks`
* `GET /api/TodoList/{todoListId}/Tasks/{id}`
* `GET /api/TodoList/{todoListId}/Tasks/paged`
* `POST /api/TodoList/{todoListId}/Tasks`
* `PUT /api/TodoList/{todoListId}/Tasks`
* `PATCH /api/TodoList/{todoListId}/Tasks/{id}`
* `DELETE /api/TodoList/{todoListId}/Tasks/{id}`

### Comments

* `GET /api/Comments`
* `GET /api/Comments/{commentId}`
* `GET /api/Comments/paged`
* `POST /api/Comments`
* `PUT /api/Comments`
* `PUT /api/Comments/status`

### Tags

* `GET /api/TodoList/{todoListId}/Tags/allTags`
* `GET /api/TodoList/{todoListId}/Tags/paged`
* `GET /api/TodoList/{todoListId}/Tags/Id`
* `GET /api/TodoList/{todoListId}/Tags/Name`
* `POST /api/TodoList/{todoListId}/Tags`
* `PUT /api/TodoList/{todoListId}/Tags`
* `DELETE /api/TodoList/{todoListId}/Tags`

### Task Assignments

* `GET /api/TodoList/TaskAssignments`
* `POST /api/TodoList/TaskAssignments/{taskId}/Users/{userId}`
* `DELETE /api/TodoList/TaskAssignments/{taskId}/Users/{userId}`

### Task Tags

* `GET /api/TaskTags/TaskId/{taskId}/paged`
* `GET /api/TaskTags/TagId/{tagId}/paged`
* `POST /api/TaskTags/Assign`
* `DELETE /api/TaskTags/Remove`

### Search

* `GET /api/Search`
* `GET /api/Search/paged`

### Users

* `GET /api/TodoList/Users/paged`
* `GET /api/TodoList/Users/exists/email/{email}`
* `GET /api/TodoList/Users/exists/id/{userId}`

---

## Notes

A minimal React frontend is included for basic interaction with the API. The main focus of the project, however, is backend development and architecture.

---

## Purpose

This project demonstrates:

* Clean Architecture implementation
* Backend development with ASP.NET Core
* Relational data modeling with Entity Framework Core
* Authentication and authorization workflows
* Service-based application design
* Background processing and notification handling

---

## Author

**Ubnigazhyp Dias** — [GitHub](https://github.com/CasperTheChild)
