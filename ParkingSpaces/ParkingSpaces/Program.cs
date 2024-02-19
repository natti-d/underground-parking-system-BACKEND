using Microsoft.EntityFrameworkCore;
using ParkingSpaces;
using ParkingSpaces.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register DbContext, instead of OnConfiguring method
// Pros: flexible and allows for centralized configuration
builder.Services.AddDbContext<ParkingSpacesDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// all services
var dependencies = new Dependencies();
dependencies.DefineDependencies(builder);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// map all controllers in your application that are derived from `ControllerBase` or `Controller`.
app.MapControllers();

app.Run();
