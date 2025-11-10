## Set up Instructions 

### Prerequisties 
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Docker CLI]
- [dotnet-ef CLI tool](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (I used it to create the migrations)


### In order to run the application 

1. **Start PostgreSQL in Docker**
(PLEASE ENSURE THAT THE DOCKER DESKTOP APPLICATION IS RUNNING)
```bash docker compose up -d ```

If you're on Windows please try in CMD with Docker CLI installed:
```docker compose up -d```

2. **Running the application using Docker** 

You can start it up in Visual Studio (Community Edition) while having the project under UserManagent.Web

Using dotnet CLI on Windows:
```dotnet run --project UserManagement.web```

3. **Stop PostgreSQL** 
```docker compose down```
Will close the instance without losing data (I believe)


### Database 
- Database is using: PostgreSQL
- Connection from database to Application is using Connection string which can be found and altered in `appsettings.json`
- Migrations have already been completed can are contained within UserManagement.Data
- Inital Data has been provided by DataContext 

### Additional Packages 

- `Npgsql.EntityFrameworkCore.PostgreSQL`
    - Version used: 9.0.4
    - Used for: PostgreSQL provider for EF (Entity Framework)
- `Microsoft.EntityFrameworkCore.Design` 
    - Version used: 9.0.10
    - Used for: CLI tool which was used for migration of data form Datacontext to database

### Creating migrations or updating migrations 

- Migrations were made using the dotnet-ef CLI tool the following commands were used:

```
dotnet ef migrations add MIGRATION_NAME --project UserManagement.Data --startup-project UserManagement.Web
```
Customized for work space:
```
dotnet ef migrations add InitialCreate --project UserManagement.Data --startup-project UserManagement.Web
```

- To update a migration 
```
dotnet ef database update --project UserManagement.Data --startup-project UserManagement.Web
```


### Additional commands 

Observe containers/instances
```
docker ps
```
Logs:
```
docker logs postgres-db
```

## CI Pipelines

### Continuous Integration (CI)

The application uses GitHub Actions for Continuous Integration
Every push to main results in:
    - Code being built
    - Running unit tests
        - Tests results are published after as a report summary with the build
    
This can be seen in: `.github/workflows/dotnet.yml`

### Attempted Continuous Deployement (CD)

This is evidenced in `.github/workflows/cd.yml`

# User Management Technical Exercise

The exercise is an ASP.NET Core web application backed by Entity Framework Core, which faciliates management of some fictional users.
We recommend that you use [Visual Studio (Community Edition)](https://visualstudio.microsoft.com/downloads) or [Visual Studio Code](https://code.visualstudio.com/Download) to run and modify the application. 

**The application uses an in-memory database, so changes will not be persisted between executions.**

## The Exercise
Complete as many of the tasks below as you feel comfortable with. These are split into 4 levels of difficulty 
* **Standard** - Functionality that is common when working as a web developer
* **Advanced** - Slightly more technical tasks and problem solving
* **Expert** - Tasks with a higher level of problem solving and architecture needed
* **Platform** - Tasks with a focus on infrastructure and scaleability, rather than application development.

### 1. Filters Section (Standard)

The users page contains 3 buttons below the user listing - **Show All**, **Active Only** and **Non Active**. Show All has already been implemented. Implement the remaining buttons using the following logic:
* Active Only – This should show only users where their `IsActive` property is set to `true`
* Non Active – This should show only users where their `IsActive` property is set to `false`

### 2. User Model Properties (Standard)

Add a new property to the `User` class in the system called `DateOfBirth` which is to be used and displayed in relevant sections of the app.

### 3. Actions Section (Standard)

Create the code and UI flows for the following actions
* **Add** – A screen that allows you to create a new user and return to the list
* **View** - A screen that displays the information about a user
* **Edit** – A screen that allows you to edit a selected user from the list  
* **Delete** – A screen that allows you to delete a selected user from the list

Each of these screens should contain appropriate data validation, which is communicated to the end user.

### 4. Data Logging (Advanced)

Extend the system to capture log information regarding primary actions performed on each user in the app.
* In the **View** screen there should be a list of all actions that have been performed against that user. 
* There should be a new **Logs** page, containing a list of log entries across the application.
* In the Logs page, the user should be able to click into each entry to see more detail about it.
* In the Logs page, think about how you can provide a good user experience - even when there are many log entries.

### 5. Extend the Application (Expert)

Make a significant architectural change that improves the application.
Structurally, the user management application is very simple, and there are many ways it can be made more maintainable, scalable or testable.
Some ideas are:
* Re-implement the UI using a client side framework connecting to an API. Use of Blazor is preferred, but if you are more familiar with other frameworks, feel free to use them.
* Update the data access layer to support asynchronous operations.
* Implement authentication and login based on the users being stored.
* Implement bundling of static assets.
* Update the data access layer to use a real database, and implement database schema migrations.

### 6. Future-Proof the Application (Platform)

Add additional layers to the application that will ensure that it is scaleable with many users or developers. For example:
* Add CI pipelines to run tests and build the application.
* Add CD pipelines to deploy the application to cloud infrastructure.
* Add IaC to support easy deployment to new environments.
* Introduce a message bus and/or worker to handle long-running operations.

## Additional Notes

* Please feel free to change or refactor any code that has been supplied within the solution and think about clean maintainable code and architecture when extending the project.
* If any additional packages, tools or setup are required to run your completed version, please document these thoroughly.
