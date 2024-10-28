var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();


string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data/warehouses.json");
builder.Services.AddSingleton(new WarehouseProvider(jsonFilePath));
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "Hello world 🚀");


app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run("http://localhost:5000");

