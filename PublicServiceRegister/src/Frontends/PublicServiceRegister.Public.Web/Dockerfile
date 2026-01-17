FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy project files
COPY ["src/Frontends/PublicServiceRegister.Public.Web/PublicServiceRegister.Public.Web.csproj", "src/Frontends/PublicServiceRegister.Public.Web/"]
COPY ["src/Shared/PublicServiceRegister.Contracts/PublicServiceRegister.Contracts.csproj", "src/Shared/PublicServiceRegister.Contracts/"]

# Restore dependencies
RUN dotnet restore "src/Frontends/PublicServiceRegister.Public.Web/PublicServiceRegister.Public.Web.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/src/Frontends/PublicServiceRegister.Public.Web"
RUN dotnet build "PublicServiceRegister.Public.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublicServiceRegister.Public.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicServiceRegister.Public.Web.dll"]
