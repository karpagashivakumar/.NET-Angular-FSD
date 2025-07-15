# .NET+Angular FSD
# 💼 Asset Management System

A full-stack enterprise-grade **Asset Management System** developed using:

- 🌐 ASP.NET Core Web API (Backend)
- 🖥 ASP.NET MVC (Admin UI)
- 🅰️ Angular (Employee UI)
- 🔐 JWT Authentication & Role-based Authorization
- 🧠 Entity Framework Core with Code-First & Repository Pattern
- ✅ NUnit for Unit Testing
- 📊 SonarQube for Code Quality Analysis
- 🗃️ MS SQL Server (Production DB), SQLite In-Memory (Testing)

---

## 📚 Project Overview

This application helps organizations manage their assets efficiently by providing:

- Asset request, allocation, and auditing workflows
- Service and maintenance request handling
- Role-based views and features for Admin and Employees
- Responsive, professional UI design with Bootstrap and animations
- Secure login with JWT-based token authentication

---

## 🧩 Technologies Used

| Layer        | Technology                             |
|--------------|----------------------------------------|
| Backend      | ASP.NET Core Web API                   |
| Database     | MS SQL Server + EF Core + Migrations   |
| Admin UI     | ASP.NET MVC (Bootstrap, Razor Views)   |
| Employee UI  | Angular 16 (Standalone Components)     |
| Auth         | JWT, Session Storage                   |
| Testing      | NUnit + In-Memory SQLite               |
| Styling      | Bootstrap 5, Animate.css, Icons        |
| DevOps       | SonarQube (Code Quality & Coverage)    |

---

## 🔐 Authentication & Authorization

- Role-based login system:
  - **Admin:** via ASP.NET MVC UI
  - **Employee:** via Angular UI
- JWT tokens stored securely in session/local storage
- Role-based redirection and route protection implemented on both client apps

---

## 🧱 Architecture

Solution/
│
├── AssetDAL/ # Data Layer
│ ├── Models/ # Entity Classes
│ └── DataAccess/ # Repositories & Interfaces
│
├── AssetServiceLayer/ # ASP.NET Core Web API
│ ├── Controllers/ # RESTful Endpoints
│ ├── Middleware/ # JWT & Error Handling
│ ├── DTOs/ # Data Transfer Objects
│ └── Program.cs, Startup.cs # App Configuration
│
├── AssetAdminUI/ # ASP.NET MVC Admin Panel
│ ├── Controllers/
│ ├── Views/
│ ├── Models/
│ └── wwwroot/ (Bootstrap, CSS)
│
├── Asset_Management/ # Angular Employee UI
│ ├── core/ # Services & Models
│ ├── modules/ # Feature Modules (auth, user, etc.)
│ └── shared/ # Reusable UI components (toast, loader)
│
├── AssetHubTest/ # NUnit Unit Testing Project
│ └── AssetRepositoryTests.cs

yaml
Copy
Edit

---

## 🌟 Features

### 🛠 Admin (ASP.NET MVC)
- Manage users (activate/deactivate, profile, password)
- Manage assets (CRUD with images)
- Allocate assets to employees
- Handle asset/service requests
- Perform audits and view reports
- View dashboards with toasts & dark mode

### 👨‍💼 Employee (Angular)
- Submit asset requests
- View allocated assets
- Raise service tickets
- View audit status
- Modern, mobile-responsive UI with animations

---

## 🧪 Testing

- Unit tested using **NUnit**
- In-memory **SQLite** used to prevent affecting actual data
- Coverage for repository methods: Create, Read, Update, Delete

---

## 🛡️ Code Quality

- Integrated with **SonarQube**:
  - Code smells
  - Coverage % reporting
  - Maintainability, bugs, and security hotspots

---

## 🔧 Setup Instructions

### 🖥 Backend (API)

1. Navigate to `AssetServiceLayer/`
2. Restore packages:
   ```bash
   dotnet restore
Run migrations and update database:

bash
Copy
Edit
dotnet ef database update
Launch the API:

bash
Copy
Edit
dotnet run
API Base URL: http://localhost:5116/

🎩 Admin UI (ASP.NET MVC)
Set AssetAdminUI as Startup Project

Build and run:

bash
Copy
Edit
Ctrl + F5
Admin Login: http://localhost:<port>/Login

🅰️ Angular (Employee UI)
Navigate to Asset_Management/

Install dependencies:

bash
Copy
Edit
npm install
Run the app:

bash
Copy
Edit
ng serve --open
🔐 Sample Credentials
Role	Username	Password
Admin	admin@asset.com	Admin@123
Employee	john@asset.com	Emp@1234

🎯 Future Enhancements
Email Notifications for request updates

PDF report generation for audits

Admin dashboard charts using Chart.js

Advanced filtering & search

🙌 Acknowledgments
Special thanks to:

My trainer(Mr. Soyeb Ghachi) for continuous guidance 💡

Open-source libraries like Bootstrap, Animate.css, and ngx-toastr 🎉
