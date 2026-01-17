FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy project files
COPY ["src/Core/PublicServiceRegister.Api/PublicServiceRegister.Api.csproj", "src/Core/PublicServiceRegister.Api/"]
COPY ["src/Core/PublicServiceRegister.Application/PublicServiceRegister.Application.csproj", "src/Core/PublicServiceRegister.Application/"]
COPY ["src/Core/PublicServiceRegister.Domain/PublicServiceRegister.Domain.csproj", "src/Core/PublicServiceRegister.Domain/"]
COPY ["src/Core/PublicServiceRegister.Infrastructure/PublicServiceRegister.Infrastructure.csproj", "src/Core/PublicServiceRegister.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/Core/PublicServiceRegister.Api/PublicServiceRegister.Api.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/src/Core/PublicServiceRegister.Api"
RUN dotnet build "PublicServiceRegister.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublicServiceRegister.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicServiceRegister.Api.dll"]
