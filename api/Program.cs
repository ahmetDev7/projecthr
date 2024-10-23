var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data");
builder.Services.AddTransient<ItemsProvider>();
builder.Services.AddSingleton(new LocationsProvider($"{jsonFilePath}/locations.json"));

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello world 🚀");


app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run("http://localhost:5000");

