using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load Env vars
DotNetEnv.Env.Load();
string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
if(connectionString == null) throw new InvalidOperationException("The required environment variable 'DB_CONNECTION_STRING' is not set.");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "jsondata/locations.json");
builder.Services.AddSingleton(new LocationsProvider(jsonFilePath));

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello world 🚀");


app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run("http://localhost:5000");

