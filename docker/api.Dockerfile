FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app

COPY src/FinanceTracker.Api/FinanceTracker.Api.csproj src/FinanceTracker.Api/
COPY src/FinanceTracker.Application/FinanceTracker.Application.csproj src/FinanceTracker.Application/
COPY src/FinanceTracker.Domain/FinanceTracker.Domain.csproj src/FinanceTracker.Domain/
COPY src/FinanceTracker.Infrastructure/FinanceTracker.Infrastructure.csproj src/FinanceTracker.Infrastructure/

RUN dotnet restore src/FinanceTracker.Api/FinanceTracker.Api.csproj

COPY src/ src/

RUN dotnet publish src/FinanceTracker.Api/FinanceTracker.Api.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FinanceTracker.Api.dll"]
