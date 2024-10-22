using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "jsondata/locations.json");
builder.Services.AddSingleton(new LocationsProvider(jsonFilePath));

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello world 🚀");


app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run("http://localhost:5000");

