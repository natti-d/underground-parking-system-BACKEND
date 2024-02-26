using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ParkingSpaces;
using ParkingSpaces.Authentication.Basic;
using ParkingSpaces.Configuration;
using ParkingSpaces.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register DbContext, instead of OnConfiguring method
// Pros: flexible and allows for centralized configuration
builder.Services.AddDbContext<ParkingSpacesDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// basic authentication middleware
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

// all services
var dependencies = new Dependencies();
dependencies.DefineDependencies(builder);

// cors
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
    builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .WithOrigins("http://localhost:5500");
}));

// singalR
builder.Services.AddSignalR();

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
    c.MapType<TimeSpan>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("00:00:00")
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API V1");
    });
}

// cors
app.UseCors("CorsPolicy");

// singalR
// RouteTable.Routes.MapHubs(new HubConfiguration { EnableCrossDomain = true });

//app.UseSignalR(routes =>
//{
//    routes.MapHub<NotifyHub>("/notify");
//});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // Adjust the route and specify the name of your hub
    endpoints.MapHub<ParkSpaceAvailabilityHub>("/chat");
});



// map all controllers in your application that are derived from `ControllerBase` or `Controller`.
app.MapControllers();

app.Run();
