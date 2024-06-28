# URL Shortener Service

This project is a simple URL Shortener Service built using ASP.NET Core, Entity Framework Core, and SQLite. 
It allows users to shorten long URLs, retrieve the original URLs from the shortened versions, and redirect to the original URLs.

## Features

- Shorten long URLs
- Retrieve the original URL from a shortened URL
- Redirect to the original URL using a shortened URL
- Very Simple frontend interface to interact with the service (if that helps!)

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Getting Started

Follow these instructions to get the project up and running on your local machine.

### 1. Clone the Repository

git clone https://github.com/jgraham2/UrlGeneratorService.git
cd UrlGeneratorService


### 2. Install Dependencies
Install the .NET dependencies:

dotnet restore

### 3. Build the Project

dotnet build

### 4. Run the Project (db will be created)
Start the ASP.NET Core project:

dotnet run

This will start the backend server at https://localhost:7281 - with Swagger UI launching.

### 5. Open the Frontend
Open your browser and navigate to the following URL to access the frontend interface:

Copy:
https://localhost:7281/index.html
