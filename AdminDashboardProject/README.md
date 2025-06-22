# 🧠 Admin Dashboard

Aдмин-приложение на **ASP.NET Core 8 + React + PostgreSQL**, поддерживает:

- JWT-аутентификацию (включая refresh-токены)
- Управление клиентами
- Платежи
- Обновление валютного курса
- CI/CD пайплайн через GitHub Actions
- Docker-окружение

---

## 🚀 Технологии

- .NET 8 (ASP.NET Core Minimal API)
- PostgreSQL
- React + Vite
- Docker / Docker Compose
- Entity Framework Core
- Identity
- GitHub Actions (CI/CD)

---


## ✅ Данные для входа:


#### 📦 Быстрый старт (Docker)
- Требуется установленный Docker

- Клонирование репозитория и переход в папку проекта:

```bash
git clone https://github.com/<твой-профиль>/DashboardApp.git
cd DashboardApp
```
Запуск приложения с помощью Docker Compose:

```bash
docker compose up --build
```

#### 🚪 Приложение будет доступно по адресам:


Backend API: http://localhost:5000/api

Frontend: http://localhost:5173


#### ✅  Вход в приложение
- При открытии frontend по адресу http://localhost:5173 отображается страница входа.

- Для авторизации используйте следующие данные:
```
Email: admin@mirra.dev
Пароль: admin123
```
---

## 🖥️ Функционал Dashboard
После успешного входа вы попадёте в админ-панель, где можно:

- Просматривать список клиентов с их балансами

- Добавлять, редактировать и удалять клиентов

- Просматривать последние платежи

- Просматривать и обновлять текущий валютный курс

- Управлять пользователями (в будущем расширение)

- Использовать JWT-аутентификацию с обновлением токенов (refresh tokens)
---

## ⚙️ Конфигурация
В docker-compose.yml заданы переменные окружения для:


```yaml
POSTGRES_USER: admin
POSTGRES_PASSWORD: admin123
POSTGRES_DB: dashboarddb

ConnectionStrings__DefaultConnection: Host=db;Database=dashboarddb;Username=admin;Password=admin123

JwtSettings__Key: MySuperSecretJwtKeyThatIsLongEnough123
JwtSettings__Issuer: AdminDashboardProject
JwtSettings__Audience: MyAppClients
JwtSettings__DurationInMinutes: 10
```
----
## 🧪 Тестирование
Тесты backend'а запускаются автоматически в CI. 

---
## 🔄 CI/CD
GitHub Actions конфигурация включает:

- Восстановление зависимостей и сборку backend

- Запуск тестов backend

- Установку зависимостей и сборку frontend

- Файл конфигурации находится по пути: .github/workflows/ci.yml

