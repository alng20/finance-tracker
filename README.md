# Finance Tracker

Personal and family finance tracking application.

## Tech Stack

Backend:
- .NET 10
- ASP.NET Core
- FluentValidation
- Vertical Slice Architecture (Application layer)
- CQRS
- MediatR
- Repository Pattern
- Docker

Frontend:
- React
- TypeScript
- Vertical Slice Architecture
- Vitest
- Jest
- Docker

Database:
- PostgreSQL


## Simple usage

1. Download the latest release
2. Extract files
3. Configure your `.env` file from `.env.example`
4. Use scripts to deploy and stop application

Make scripts executable if they are not already executable
```bash
chmod +x deploy_app stop_app
```

Deploy and start application
```bash
./deploy_app docker-compose.deploy.yml .env
```

Add `pull` to pull images
```bash
./deploy_app docker-compose.deploy.yml .env pull
```

Add `recreate` to recreate the containers
```bash
./deploy_app docker-compose.deploy.yml .env recreate
```


Stop application
```bash
./stop_app docker-compose.deploy.yml .env
```

Add `rmi` to remove images
```bash
./stop_app docker-compose.deploy.yml .env rmi
```

### Browser access

Application is available at `http://localhost:5173` by default

Application is available at `https://localhost:5443` if local TLS certificates are configured

Use your ports if you change `HTTP_PORT`/`HTTS_PORT` environment variables
