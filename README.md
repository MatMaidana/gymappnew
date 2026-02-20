# Gym Billing MVP

MVP SaaS para gimnasios pequeños de Argentina.

## Stack
- Backend: .NET 8 + ASP.NET Core Web API + EF Core + PostgreSQL
- Frontend: React + TypeScript + Vite

## Funcionalidades
- Control de alumnos
- Control de vencimiento de cuotas
- Registro de pagos
- Recordatorio manual por WhatsApp
- Listas: vencidos, por vencer y todos

## Backend
El backend está en `backend/GymBilling.Api` con una arquitectura clean-ish:
- `Domain`: entidades y enums
- `Application`: servicios y abstracciones
- `Infrastructure`: EF Core, repositorios
- `Api`: controllers y contratos

### Endpoints
- `GET /api/members?filter=overdue|dueSoon`
- `POST /api/members`
- `PUT /api/members/{id}`
- `DELETE /api/members/{id}` (soft delete)
- `POST /api/members/{id}/payments` (marca pago y avanza vencimiento +1 mes)

## Frontend
El frontend está en `frontend` y consume la API vía `fetch`.

Tabs:
- Vencidos
- Por vencer
- Todos

Acciones por alumno:
- Marcar pagado
- Abrir recordatorio por WhatsApp

## Variables
Frontend:
- `VITE_API_BASE_URL` (default `http://localhost:5000/api`)

Backend:
- `ConnectionStrings__DefaultConnection`
