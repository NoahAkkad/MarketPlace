# ASP.NET Core MVC Application ( MarketPlace )

## Introduction

This ASP.NET Core MVC application is a simple product management system with authentication features implemented using ASP.NET Core Identity. The application allows users to perform CRUD (Create, Read, Update, Delete) operations on products, and it includes a related model, Category, to categorize products. The application uses Entity Framework for database interactions and includes unit tests to ensure the functionality is working as expected.

## Steps to Run the Application

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to restore NuGet packages.
4. Set the `MarketPlace` project as the startup project.
5. Update the connection string in `appsettings.json` to point to your local database.
6. Run the application.

## Steps to Run Unit Tests

1. Open the solution in Visual Studio.
2. Build the solution to restore NuGet packages.
3. Set the `MyApp.Tests` project as the startup project.
4. Run the tests.

## Description of Tests

1. **ProductControllerTests**

    - **Index_ReturnsViewResultWithListOfProducts**: Ensures that the Index action in the ProductController returns a ViewResult containing a list of products.

    - **Create_ValidModel_RedirectsToIndex**: Verifies that creating a valid product model redirects to the Index page.

    - **Edit_ValidModel_RedirectsToIndex**: Checks if editing a valid product model redirects to the Index page.

    - **Delete_ValidModel_RedirectsToIndex**: Tests that deleting a valid product model redirects to the Index page.

    - **Create_InvalidModel_ReturnsViewResult**: Validates that creating an invalid product model returns a ViewResult.

    - **Create_ActionIsAuthorized**: Ensures that the Create action is authorized for an admin user.

    - **Edit_ActionIsAuthorized**: Checks if the Edit action is authorized for an admin user.

    - **Delete_ActionIsAuthorized**: Tests that the Delete action is authorized for an admin user.

    - **Create_ActionIsUnauthorized**: Validates that the Create action is unauthorized for a non-admin user.

    - **Edit_ActionIsUnauthorized**: Ensures that the Edit action is unauthorized for a non-admin user.

    - **Delete_ActionIsUnauthorized**: Checks if the Delete action is unauthorized for a non-admin user.

2. **CategoryControllerTests**

    - (Similar structure as ProductControllerTests)

<img width="1601" alt="Screenshot 2024-01-04 at 15 08 43" src="https://github.com/Distansakademin/inl-mningsuppgift-3-mvc-autentisering-NoahAkkad/assets/97226622/4eea24f8-4493-4d90-b96e-9bff64b50aa4">



## Reflection


This task provided valuable hands-on experience in implementing unit tests, integrating authentication features with ASP.NET Core Identity, and using Entity Framework for database interactions. I learned how to create and structure unit tests to ensure the functionality and security of the application.

Challenges were encountered in setting up the authentication and authorization tests, as it required careful consideration of user roles and permissions. The use of mock dependencies in unit tests was also a learning point, ensuring tests remain isolated and do not rely on external resources.

The skills gained in this task are highly transferable. Understanding unit testing, authentication, and authorization in ASP.NET Core is crucial for developing robust and secure applications. These skills will be beneficial in future projects, ensuring code reliability, security, and ease of maintenance. Overall, this assignment provided a comprehensive understanding of building a secure and well-tested ASP.NET Core MVC application.


# Noah Akkad


