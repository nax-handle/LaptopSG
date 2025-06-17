using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using LaptopSG.Models;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllersWithViews();

// Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LaptopSGConnection")));

var app = builder.Build();

// Test database connection and log results
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("🔍 Testing database connection...");
        
        var connectionString = builder.Configuration.GetConnectionString("LaptopSGConnection");
        logger.LogInformation($"📝 Using connection string: {connectionString}");
        
        // Test connection
        var canConnect = await context.Database.CanConnectAsync();
        
        if (canConnect)
        {
            logger.LogInformation("✅ Database connection successful!");
            
            // Check if database exists
            var databaseExists = await context.Database.EnsureCreatedAsync();
            if (databaseExists)
            {
                logger.LogInformation("📊 Database created successfully!");
            }
            else
            {
                logger.LogInformation("📊 Database already exists.");
            }
            
            // Log database info
            logger.LogInformation($"🔧 Database Provider: {context.Database.ProviderName}");
            logger.LogInformation($"🗄️ Database Name: {context.Database.GetDbConnection().Database}");
        }
        else
        {
            logger.LogError("❌ Database connection failed!");
            
            // Try alternative connection strings
            var alternatives = new[]
            {
                "Server=localhost;Database=LapTopSG;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true",
                "Server=.\\SQLEXPRESS;Database=LapTopSG;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true",
                "Server=(localdb)\\MSSQLLocalDB;Database=LapTopSG;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true",
                "Server=localhost\\SQLEXPRESS;Database=LapTopSG;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
            };
            
            logger.LogInformation("🔄 Trying alternative connection strings...");
            
            foreach (var altConnectionString in alternatives)
            {
                try
                {
                    logger.LogInformation($"🧪 Testing: {altConnectionString}");
                    
                    var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();
                    optionsBuilder.UseSqlServer(altConnectionString);
                    
                    using var testContext = new StoreDbContext(optionsBuilder.Options);
                    var testCanConnect = await testContext.Database.CanConnectAsync();
                    
                    if (testCanConnect)
                    {
                        logger.LogInformation($"✅ Alternative connection successful: {altConnectionString}");
                        logger.LogWarning("💡 Consider updating your appsettings.json with this working connection string.");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"❌ Alternative failed: {ex.Message}");
                }
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Error testing database connection: {ErrorMessage}", ex.Message);
        logger.LogError("🔧 Connection String: {ConnectionString}", builder.Configuration.GetConnectionString("LaptopSGConnection"));
        
        // Log additional troubleshooting info
        logger.LogInformation("🛠️ Troubleshooting tips:");
        logger.LogInformation("   1. Ensure SQL Server is running");
        logger.LogInformation("   2. Check if SQL Server service is started");
        logger.LogInformation("   3. Verify Windows Authentication is enabled");
        logger.LogInformation("   4. Try SQL Server Management Studio with same credentials");
        logger.LogInformation("   5. Check Windows Firewall settings");
        
        // Check if it's a specific error type
        if (ex.Message.Contains("network-related") || ex.Message.Contains("server was not found"))
        {
            logger.LogWarning("🌐 Network/Server error detected. SQL Server might not be running or accessible.");
        }
        else if (ex.Message.Contains("login failed") || ex.Message.Contains("authentication"))
        {
            logger.LogWarning("🔐 Authentication error detected. Check Windows Authentication settings.");
        }
        else if (ex.Message.Contains("certificate") || ex.Message.Contains("SSL"))
        {
            logger.LogWarning("🔒 SSL/Certificate error detected. TrustServerCertificate=true should help.");
        }
    }
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();

