# Use the official .NET 6 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the project files and restore any dependencies
COPY ./backend/api/*.csproj ./api/
RUN dotnet restore ./api/*.csproj

# Copy the entire project and build the application
COPY ./backend/api/. ./api/
WORKDIR /app/api
RUN dotnet publish -c Release -o /out

# Use the official .NET 6 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Expose the port your application runs on (e.g., 80)
EXPOSE 80

# Set environment variables (optional defaults)
ENV MONGODB_CONNECTION_STRING=""
ENV MONGODB_DATABASE_NAME=""

# Set the entry point to run your application
ENTRYPOINT ["dotnet", "Company.Function.dll"]
