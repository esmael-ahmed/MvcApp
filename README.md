# Employee Management System
Project Description
This is an MVC web application built using .NET that serves as an employee management system. It demonstrates a three-tier architecture with the following layers:
  - Business Logic Layer (BLL): Contains the Generic Repository.
  - Data Access Layer (DAL): Manages models, database context, and migrations.
  - Presentation Layer (PL): The MVC structure handling the user interface.
The application allows managing departments and employees with CRUD operations, authentication, authorization, file uploads, and more.

Features
  - 3-tier architecture: BLL, DAL, PL.
  - Department & Employee Management: CRUD operations for departments and employees with a one-to-many relationship between them.
  - Generic Repository: Implements business logic with a reusable repository pattern.
  - Client & Server-Side Validation: Validations using jQuery and built-in .NET validation features.
  - Search Bar: Enables searching for employees.
  - Authentication & Authorization: Uses Microsoft Identity for login, registration, and role management. Includes a forgot password feature.
  - File Uploads: Allows uploading employee images, and automatically deletes images from the server when an employee is removed.
  - User Interface: Responsive design using Bootstrap, FontAwesome, and partial views.
  - Data Mapping: Uses AutoMapper to map data between models and view models.
  - Data Passing: Utilizes ViewData, ViewBag, TempData, and view models to pass data between controllers and views.
  - Eager Loading: For optimizing queries when loading related data.

Usage
  - Department Management: Add, edit, delete, and view department information.
  - Employee Management: Add, edit, delete employees, upload their images, and search for employees by name.
  - Authentication & Authorization: Login, register, and reset password functionalities are implemented. Only authorized users can perform CRUD operations.
