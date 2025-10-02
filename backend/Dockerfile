# -------- Stage 1: Build the app --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish the app to /app/publish
RUN dotnet publish -c Release -o /app/publish

# -------- Stage 2: Runtime image --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published files from build stage
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Environment variable for Supabase connection string
# This will be set on Render; default fallback just throws an error if missing
ENV SUPABASE_DB=""

# Start the app
ENTRYPOINT ["dotnet", "TheVintageKit.Backend.dll"]
