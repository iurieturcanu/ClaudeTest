FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy project files
COPY ["src/Frontends/PublicServiceRegister.Officer.Web/PublicServiceRegister.Officer.Web.csproj", "src/Frontends/PublicServiceRegister.Officer.Web/"]
COPY ["src/Shared/PublicServiceRegister.Contracts/PublicServiceRegister.Contracts.csproj", "src/Shared/PublicServiceRegister.Contracts/"]

# Restore dependencies
RUN dotnet restore "src/Frontends/PublicServiceRegister.Officer.Web/PublicServiceRegister.Officer.Web.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/src/Frontends/PublicServiceRegister.Officer.Web"
RUN dotnet build "PublicServiceRegister.Officer.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublicServiceRegister.Officer.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicServiceRegister.Officer.Web.dll"]
