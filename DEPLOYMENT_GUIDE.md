# Deployment Guide

Guide for deploying Web Lab Database Assignment to production.

## Pre-Deployment Checklist

- [ ] All tests passing
- [ ] Code reviewed
- [ ] Security vulnerabilities checked
- [ ] Performance optimized
- [ ] Database backups created
- [ ] Environment variables set
- [ ] HTTPS certificate configured
- [ ] Error logging enabled

## Database Deployment

### SQL Server Production Setup

```sql
-- Create production database with proper settings
CREATE DATABASE WebLabDB_Production
    ON
    PRIMARY (
        NAME = 'WebLabDB_Data',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\WebLabDB.mdf',
        SIZE = 100MB,
        FILEGROWTH = 10MB
    )
    LOG ON (
        NAME = 'WebLabDB_Log',
        FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\WebLabDB.ldf',
        SIZE = 50MB,
        FILEGROWTH = 10MB
    );

-- Enable backups
ALTER DATABASE WebLabDB_Production SET RECOVERY FULL;

-- Create database user with appropriate permissions
USE WebLabDB_Production;
CREATE LOGIN weblab_user WITH PASSWORD = 'StrongPassword123!';
CREATE USER weblab_user FOR LOGIN weblab_user;

-- Grant permissions
ALTER ROLE db_datareader ADD MEMBER weblab_user;
ALTER ROLE db_datawriter ADD MEMBER weblab_user;
```

## Backend Deployment

### Option 1: Azure App Service

```bash
# Create resource group
az group create --name WebLabRG --location eastus

# Create App Service Plan
az appservice plan create --name WebLabPlan --resource-group WebLabRG --sku B1

# Create Web App
az webapp create --name WebLabAPI --resource-group WebLabRG --plan WebLabPlan

# Configure connection string
az webapp config connection-string set --name WebLabAPI \
  --resource-group WebLabRG \
  --settings DefaultConnection="Server=tcp:...;Database=WebLabDB_Production;..." \
  --connection-string-type SQLServer

# Deploy code
dotnet publish -c Release
# Upload to App Service
```

### Option 2: IIS on Windows Server

```bash
# From Backend folder
dotnet publish -c Release -o "./bin/publish"

# Copy published files to IIS folder
# C:\inetpub\wwwroot\WebLabAPI\

# Configure IIS Application Pool
# - Create new pool for .NET Core
# - Set managed pipeline to "No Managed Code"
```

### Option 3: Docker Deployment

```dockerfile
# Create Dockerfile in Backend folder
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebLabAPI.csproj", "./"]
RUN dotnet restore "WebLabAPI.csproj"
COPY . .
RUN dotnet build "WebLabAPI.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .
EXPOSE 443
ENV ASPNETCORE_URLS=https://+:443
ENTRYPOINT ["dotnet", "WebLabAPI.dll"]
```

Build and run:
```bash
docker build -t weblab-api .
docker run -d -p 443:443 -e "DefaultConnection=..." weblab-api
```

## Frontend Deployment

### Option 1: Azure Static Web Apps

```bash
# Create Static Web App
az staticwebapp create --name WebLabFrontend \
  --resource-group WebLabRG \
  --source https://github.com/yourusername/WebLabDatabaseAssignment \
  --location eastus \
  --build-folder Frontend
```

### Option 2: GitHub Pages

1. Push to GitHub
2. Settings → Pages
3. Select main branch and /Frontend folder
4. Site published at https://yourusername.github.io/WebLabDatabaseAssignment

### Option 3: Netlify

```bash
npm install -g netlify-cli
netlify deploy --prod --dir=Frontend
```

## Environment Configuration

### Production appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:yourserver.database.windows.net,1433;Initial Catalog=WebLabDB_Production;Persist Security Info=False;User ID=weblab_user;Password=StrongPassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AllowedHosts": "yourdomain.com,www.yourdomain.com",
  "Cors": {
    "AllowedOrigins": ["https://yourdomain.com", "https://www.yourdomain.com"]
  }
}
```

### Frontend Configuration

Update API URL in `js/utils.js`:

```javascript
const API_BASE_URL = 'https://api.yourdomain.com/api';
```

## Security Best Practices

### 1. API Security

```csharp
// Require HTTPS
app.UseHttpsRedirection();

// Add security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    await next();
});

// Implement rate limiting
app.UseRateLimiter();
```

### 2. Database Security

- Use strong passwords
- Enable SQL Server encryption
- Implement row-level security
- Regular backups
- Audit logging enabled

### 3. Frontend Security

- Content Security Policy headers
- Sanitize user inputs
- HTTPS only
- Secure cookies

### 4. Authentication

Implement JWT tokens:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
```

## Monitoring & Logging

### Application Insights

```csharp
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();
var telemetryClient = app.Services.GetRequiredService<TelemetryClient>();
telemetryClient.TrackTrace("Application started");
```

### Logging

```csharp
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting application");
```

## Performance Optimization

### Caching

```csharp
builder.Services.AddMemoryCache();

app.UseResponseCaching();
```

### Database Indexing

```sql
CREATE INDEX IX_Students_Email ON Students(Email);
CREATE INDEX IX_Contacts_Email ON Contacts(Email);
CREATE INDEX IX_Products_Sku ON Products(Sku);
```

### CDN Configuration

- Serve static files from CDN
- Cache API responses (if applicable)
- Use compression (gzip)

## Disaster Recovery

### Backup Strategy

```sql
-- Daily backups
BACKUP DATABASE WebLabDB_Production TO DISK = 'C:\Backups\WebLabDB.bak'
WITH INIT, STATS = 10;

-- Backup logs every hour
BACKUP LOG WebLabDB_Production TO DISK = 'C:\Backups\WebLabDB_Log.bak'
WITH INIT;
```

### Recovery Process

```sql
-- Restore from backup
RESTORE DATABASE WebLabDB_Production FROM DISK = 'C:\Backups\WebLabDB.bak'
WITH REPLACE;
```

## Post-Deployment

- [ ] Verify all endpoints accessible
- [ ] Test all CRUD operations
- [ ] Check database connectivity
- [ ] Monitor error logs
- [ ] Performance testing
- [ ] Security testing
- [ ] User acceptance testing

## Support & Maintenance

- Monitor application performance
- Review logs regularly
- Apply security patches
- Update dependencies
- Backup verification
- Performance tuning
- Load testing before scaling

---

**Deployment Checklist Complete!** ✅

For questions, refer to documentation or contact your DevOps team.
