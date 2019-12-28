FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Superdigital.Api/Superdigital.Api.csproj", "src/Superdigital.Api/"]
COPY ["src/Superdigital.Infra.Data/Superdigital.Infra.Data.csproj", "src/Superdigital.Infra.Data/"]
COPY ["src/Superdigital.Domain/Superdigital.Domain.csproj", "src/Superdigital.Domain/"]
COPY ["src/Superdigital.Infra.CrossCutting.Extension/Superdigital.Infra.CrossCutting.Extension.csproj", "src/Superdigital.Infra.CrossCutting.Extension/"]
COPY ["src/Superdigital.Application/Superdigital.Application.csproj", "src/Superdigital.Application/"]
RUN dotnet restore "src/Superdigital.Api/Superdigital.Api.csproj"
COPY . .
WORKDIR "/src/src/Superdigital.Api"
RUN dotnet build "Superdigital.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Superdigital.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Superdigital.Api.dll"]
