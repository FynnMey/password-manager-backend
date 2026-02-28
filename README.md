## Features
* **Auth:** Login, JWT & Refresh Tokens.
* **User Management:** Verwaltung von Benutzerkonten.
* **Vault Storage:** Sicherer, verschlüsselter Datenspeicher.
* **Device Handling:** Verwaltung verknüpfter Geräte.
* **Rate-Limits:** Schutz vor Brute-Force und API-Missbrauch.

## Tech Stack
* **Runtime:** ASP.NET Core
* **ORM:** EF Core / Dapper
* **Database:** PostgreSQL / MySQL
* **Security:** JWT, Refresh Tokens, Encryption

## Project Structure
```text
src/
 ├── Api             # Controller, Middleware, Program.cs
 ├── Domain          # Entities, Interfaces, Exceptions
 ├── Application     # DTOs, Services, Logic, Commands/Queries
 ├── Infrastructure  # Persistence, DB Context, External Services
```