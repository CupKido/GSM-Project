#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 3306
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GSM/GSM.csproj", "GSM/"]
RUN dotnet restore "GSM/GSM.csproj"
COPY . .
WORKDIR "/src/GSM"
RUN dotnet build "GSM.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GSM.csproj" -c Release -o /app/publish /p:UseAppHost=false

ENV MYSQL_USER=root \
    MYSQL_PASSWORD=root \
    MYSQL_DATABASE=gsmdb \
    MYSQL_HOST=localhost

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the entrypoint script into the container

# Set the entrypoint to the script
ENTRYPOINT ["dotnet", "GSM.dll"]