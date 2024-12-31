using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Load Env vars
DotNetEnv.Env.Load();
string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
string? securityKey = Environment.GetEnvironmentVariable("SECURITY_KEY");
if (connectionString == null) throw new InvalidOperationException("The required environment variable 'DB_CONNECTION_STRING' is not set.");
if (securityKey == null) throw new InvalidOperationException("The required environment variable 'SECURITY_KEY' is not set.");

builder.Services.AddSingleton(securityKey);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register the TokenService
builder.Services.AddScoped<TokenService>();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKeyThatIs32BytesLongX")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Authentication failed: {Message}", context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Token validated successfully for user {UserId}", context.Principal?.Identity?.Name);
            return Task.CompletedTask;
        }
    };
});

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CargoHub API",
        Version = "v1"
    });

    // Add security definition for Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    // Add security requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddTransient<AddressProvider>();
builder.Services.AddTransient<ContactProvider>();
builder.Services.AddTransient<WarehousesProvider>();
builder.Services.AddTransient<ItemsProvider>();
builder.Services.AddTransient<LocationsProvider>();
builder.Services.AddTransient<ItemGroupProvider>();
builder.Services.AddTransient<OrderProvider>();
builder.Services.AddTransient<ShipmentProvider>();
builder.Services.AddTransient<SupplierProvider>();
builder.Services.AddTransient<TransferProvider>();
builder.Services.AddTransient<ClientsProvider>();
builder.Services.AddTransient<InventoriesProvider>();
builder.Services.AddTransient<ItemLinesProvider>();
builder.Services.AddTransient<ItemTypesProvider>();
builder.Services.AddTransient<DocksProvider>();

builder.Services.AddScoped<IValidator<Supplier>, SupplierValidator>();
builder.Services.AddScoped<IValidator<Location>, LocationValidator>();
builder.Services.AddScoped<IValidator<Item>, ItemValidator>();
builder.Services.AddScoped<IValidator<ItemGroup>, ItemGroupValidator>();
builder.Services.AddScoped<IValidator<Order>, OrderValidator>();
builder.Services.AddScoped<IValidator<Shipment>, ShipmentValidator>();
builder.Services.AddScoped<IValidator<Contact>, ContactValidator>();

builder.Services.AddScoped<IValidator<Address>, AddressValidator>();
builder.Services.AddScoped<IValidator<Transfer>, TransferValidator>();
builder.Services.AddScoped<IValidator<Inventory>, InventoryValidator>();
builder.Services.AddScoped<IValidator<ItemLine>, ItemLineValidator>();
builder.Services.AddScoped<IValidator<Warehouse>, WarehouseValidator>();
builder.Services.AddScoped<IValidator<ItemType>, ItemTypeValidator>();
builder.Services.AddScoped<IValidator<Address>, AddressValidator>();
builder.Services.AddScoped<IValidator<Client>, ClientValidator>();
builder.Services.AddScoped<IValidator<InventoryRequest>, InventoryRequestValidator>();
builder.Services.AddScoped<IValidator<Dock>, DockValidator>();



builder.Services.AddControllers().AddJsonOptions(options =>
 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "CargoHub API 🚚📦");


app.MapControllers();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CargoHub API");
    c.RoutePrefix = string.Empty;
});

app.Run("http://localhost:5000");

public partial class Program { }