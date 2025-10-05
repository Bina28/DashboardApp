
# 🧠 Admin Dashboard

Admin application built with **ASP.NET Core 8 + React + PostgreSQL**, featuring:

- JWT authentication (including refresh tokens)  
- Client management  
- Payments  
- Currency rate updates  
- CI/CD pipeline via GitHub Actions  
- Docker environment  

---

## 🚀 Technologies

- .NET 8 (ASP.NET Core Minimal API)  
- PostgreSQL  
- React + Vite  
- Docker / Docker Compose  
- Entity Framework Core  
- Identity  
- GitHub Actions (CI/CD)  

---
## 🖼️ Screenshot
![Admin Dashboard Screenshot](./images/admin-dashboard.png)  

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
