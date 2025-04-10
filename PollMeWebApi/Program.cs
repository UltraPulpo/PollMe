using PollMeWebApi.Interfaces;
using PollMeWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register PollService with the JSON file path
var dataFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "polls.json");
builder.Services.AddSingleton<IPollService>(new PollService(dataFilePath));

// Register swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("https://localhost:4200", "http://localhost:4200") // Replace with your Angular app's URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ensure CORS middleware runs before routing.
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseRouting(); // Routing middleware should come after CORS.

app.UseAuthorization();

app.MapControllers();

app.Run();
