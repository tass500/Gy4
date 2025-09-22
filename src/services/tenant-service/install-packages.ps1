# Install required packages for the Tenant Service

# Core EF packages
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

# Tools for EF Core
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0

# Swagger/OpenAPI
dotnet add package Swashbuckle.AspNetCore --version 6.5.0

# Logging
dotnet add package Serilog.AspNetCore --version 8.0.0

echo "All required packages have been installed successfully!"
