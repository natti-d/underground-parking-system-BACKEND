using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ParkingSpaces;
using ParkingSpaces.Authentication.Basic;
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

// extend the swagger functionality to append that authorization header to all calls
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

// basic authentication middleware
//builder.Services.AddAuthentication(options =>
//    {
//        //options.DefaultAuthenticateScheme = "BasicAuthentication";
//        options.DefaultChallengeScheme = "BasicAuthentication";
//    })
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthentication();
app.UseAuthorization();

// map all controllers in your application that are derived from `ControllerBase` or `Controller`.
app.MapControllers();

app.Run();
