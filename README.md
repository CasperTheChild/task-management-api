# Task Management API

## Overview
A backend service built with ASP.NET Core using Clean Architecture principles.  
The system supports task organization with assignments, tagging, comments, and secure user authentication.

This project focuses on structuring a scalable backend with clear separation of concerns and real-world features.

---

## Core Features
- JWT-based authentication and authorization
- TodoList and Task management
- Task assignment to users
- Tag system (many-to-many relationship)
- Task comments
- Background job processing with Hangfire (available in `feature/background-job` branch)

---

## Architecture
The project follows Clean Architecture:

- **Domain** — core entities and business rules  
- **Application** — services and use-case logic  
- **Infrastructure** — database, EF Core, Identity  
- **API** — controllers and endpoints  

Each layer is isolated to maintain maintainability and scalability.

---

## Tech Stack
- C# / ASP.NET Core  
- Entity Framework Core  
- SQL Server  
- ASP.NET Identity  
- JWT Authentication  
- Hangfire  

---

## Data Model
Main entities:
- TodoLists  
- Tasks  
- AssignedTasks  
- Comments  
- Tags  
- TaskTags  
- Users (Identity)

---

## Running the Project

1. Clone the repository
2. Configure `appsettings.json` (database connection)
3. Apply migrations: dotnet ef database update
4. Run: dotnet run


---

## API Overview

### Authentication
- `POST /api/Authentication/register`
- `POST /api/Authentication/login`

---

### Todo Lists
- `GET /api/TodoList`
- `GET /api/TodoList/{id}`
- `GET /api/TodoList/paged`
- `POST /api/TodoList`
- `PUT /api/TodoList/{id}`
- `PATCH /api/TodoList/{id}`
- `DELETE /api/TodoList/{id}`
- `GET /api/TodoList/preview`

---

### Tasks
- `GET /api/TodoList/{todoListId}/Tasks`
- `GET /api/TodoList/{todoListId}/Tasks/{id}`
- `GET /api/TodoList/{todoListId}/Tasks/paged`
- `POST /api/TodoList/{todoListId}/Tasks`
- `PUT /api/TodoList/{todoListId}/Tasks`
- `PATCH /api/TodoList/{todoListId}/Tasks/{id}`
- `DELETE /api/TodoList/{todoListId}/Tasks/{id}`

---

### Comments
- `GET /api/Comments`
- `GET /api/Comments/{commentId}`
- `GET /api/Comments/paged`
- `POST /api/Comments`
- `PUT /api/Comments`
- `PUT /api/Comments/status`

---

### Tags
- `GET /api/TodoList/{todoListId}/Tags/allTags`
- `GET /api/TodoList/{todoListId}/Tags/paged`
- `GET /api/TodoList/{todoListId}/Tags/Id`
- `GET /api/TodoList/{todoListId}/Tags/Name`
- `POST /api/TodoList/{todoListId}/Tags`
- `PUT /api/TodoList/{todoListId}/Tags`
- `DELETE /api/TodoList/{todoListId}/Tags`

---

### Task Assignments
- `GET /api/TodoList/TaskAssignments`
- `POST /api/TodoList/TaskAssignments/{taskId}/Users/{userId}`
- `DELETE /api/TodoList/TaskAssignments/{taskId}/Users/{userId}`

---

### Task Tags
- `GET /api/TaskTags/TaskId/{taskId}/paged`
- `GET /api/TaskTags/TagId/{tagId}/paged`
- `POST /api/TaskTags/Assign`
- `DELETE /api/TaskTags/Remove`

---

### Search
- `GET /api/Search`
- `GET /api/Search/paged`

---

### Users
- `GET /api/TodoList/Users/paged`
- `GET /api/TodoList/Users/exists/email/{email}`
- `GET /api/TodoList/Users/exists/id/{userId}`

---

## Notes
A minimal React frontend is included for basic interaction with the API, but the main focus of this project is backend development.

---

## Purpose
This project demonstrates:
- Clean Architecture implementation
- Backend system design with ASP.NET Core
- Working with relational data and EF Core
- Authentication and authorization workflows

---

## Author
Ubnigazhyp Dias
github.com/CasperTheChild
