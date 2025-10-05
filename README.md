
# 🧠 Admin Dashboard 

A small **Admin Dashboard** project to demonstrate full-stack development using **ASP.NET Core 8 Minimal API** and **React + Vite**.  

This project is a simplified version of a real admin panel, focusing on **REST API design, clean backend code, and a basic React frontend**.  

---

## 🔹 Project Overview

The Admin Dashboard allows you to:

- Authenticate as an admin 
- View a list of clients with their balances
- View recent payments 
- Check and update the token exchange rate

The frontend provides:

- A **login page** that stores a token in `localStorage`
- A **dashboard page** showing clients and a section to view/update the token rate

---

## 🚀 Technologies

- .NET 8 (ASP.NET Core Minimal API)  
- PostgreSQL  
- React + Vite
- Tailwind CSS 
- Docker / Docker Compose  
- Entity Framework Core  
- Identity  
- GitHub Actions (CI/CD)  

---
## 🖼️ Screenshot

<img width="400" height="400" alt="admin-dashboard" src="https://github.com/user-attachments/assets/190d150e-1610-4152-9781-3643671fbc98" />

---

## ✅ Login Credentials

#### 📦 Quick Start (Docker)
- Requires Docker installed  

- Clone the repository and navigate to the project folder:

```bash
git clone https://github.com/<your-username>/DashboardApp.git
cd DashboardApp
```
Run the application using Docker Compose:

```bash
cd AdminDashboardProject
docker compose up --build
```
#### 🚪 Application URLs
Backend API: http://localhost:5000/api

Frontend: http://localhost:5173

✅ Login
Open the frontend at http://localhost:5173 to see the login page.

Use the following credentials to log in:

```bash
Email: admin@mirra.dev
Password: admin123
```
---

## 🖥️ Dashboard Features
Once logged in, you can access the admin panel to:

- View a list of clients with their balances

- Add, edit, and delete clients

- View recent payments

- View and update current currency rates

- Manage users (future functionality)

- Use JWT authentication with token refresh

---


## ⚙️ Configuration
Environment variables are set in docker-compose.yml:

```bash
POSTGRES_USER: admin
POSTGRES_PASSWORD: admin123
POSTGRES_DB: dashboarddb

ConnectionStrings__DefaultConnection: Host=db;Database=dashboarddb;Username=admin;Password=admin123

JwtSettings__Key: MySuperSecretJwtKeyThatIsLongEnough123
JwtSettings__Issuer: AdminDashboardProject
JwtSettings__Audience: MyAppClients
JwtSettings__DurationInMinutes: 10
```
---
## 🧪 Testing

Backend tests run automatically in CI.

---
## 🔄 CI/CD
GitHub Actions configuration includes:

- Restoring dependencies and building the backend

- Running backend tests

- Installing frontend dependencies and building frontend

- The workflow file is located at: .github/workflows/ci.yml

---

## 📬 Postman Collection
A Postman collection is available for convenient API testing:

```bash
DashboardApp.postman_collection.json
```
The file is located in the root of the repository. Import it into Postman to quickly access ready-made requests for all main endpoints.
