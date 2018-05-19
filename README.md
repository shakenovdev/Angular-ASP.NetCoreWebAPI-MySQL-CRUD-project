# IdeaShare Angular-WebAPI project

This repository contains Web App built on Angular 5 that interacts with WebAPI which has MySQL database

## Table of contents
1. [Demo](#demo)
2. [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Edit WebAPI config appsettings.json](#edit-webapi-config-appsettings.json)
    - [Code-first database migration](#code-first-database-migration)
    - [Run project](#run-project)
3. [Functionality Overview](#functionality-overview)
    - [Features](#features)
    - [WebAPI documentation](#webapi-documentation)
    - [Page navigation](#page-navigation)
4. [Built With](#built-with)
5. [Roadmap](#roadmap)
6. [Authors](#authors)
7. [License](#license)

## [Demo](http://ideashareapp.azurewebsites.net)

- SignUp email verification is limited to 100 emails per day due to free plan.
- Image uploading is restricted up to 500 KB
- Froala WYSIWYG is trial so the red alert stays above editor

*Reminder for Russia citizens: Microsoft Azure is banned in Russia (use VPN)*

## Getting Started

### Prerequisites

- Node.js
- Angular CLI
- .NET Core Framework

### Edit WebAPI config [appsettings.json](WebApi/appsettings.json)

- ConnectionString
- JWT SecretKey
- Email/SendGridAPIKey [how to create SendGrid?](https://docs.microsoft.com/en-us/azure/sendgrid-dotnet-how-to-send-email)

### Code-first database migration

Generate database `dotnet ef migrations add InitialMigration` then `dotnet ef database update`

### Run project

Run `ASPNETCORE_Environment=Development dotnet run` to build project.

## Functionality Overview

### Features

* Cross Platform
* CRUD operations
* Entity Framework Core MySQL
* JWT Authentication
* Swagger API documentation
* Responsive Design

### WebAPI documentation

Online API documentation is located on [/Swagger](http://ideashareapp.azurewebsites.net/swagger/)

### Page navigation

- Home
    - Home (/)
        - article list with infinite scrolling
        - the most popular tags
        - user leaderboard
    - Dependencies (/dependencies)
        - static page
        - information about used frameworks and third party libraries
    - Contacts (/contacts)
        - static page
- Auth
    - SignIn (/auth/signin)
        - store JWT token in localStorage
    - SignUp (/auth/signup)
        - Email verification
- Idea
    - Add new (/idea/new)
        - Auth guard (redirects if user is not logged)
        - edit list of tags
        - article editor WYSIWYG
    - Details (/idea/:id)
        - like/dislike button
        - raw html render
        - favorite button
        - edit/restore button
        - comments section
    - Search (/idea/search/:value)
        - search by value in title and article
- Profile
    - User info (/profile/:username)
        - information about user
        - list of favorited articles
        - list of created articles
    - Settings (/settings)
        - edit avatar
        - change username

## Built With

* **ASP.NET Core 2.0 WebAPI**
* **Angular 5**
* **MySQL**

## Roadmap

- [ ] Localization
- [ ] WebAPI.Tests
- [ ] Edit articles/comments
- [ ] Administrator dashboard

## Authors

* **Damir Shakenov** - [shakenovdev](https://github.com/shakenovdev)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details