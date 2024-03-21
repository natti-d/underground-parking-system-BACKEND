using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ParkingSpaces;
using ParkingSpaces.Authentication.Basic;
using ParkingSpaces.Configuration;
using ParkingSpaces.Hubs;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register DbContext, instead of OnConfiguring methodS
// Pros: flexible and allows for centralized configuration
builder.Services.AddDbContext<ParkingSpacesDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// basic authentication middleware
//builder.Services.AddAuthentication("BasicAuthentication")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

// all services
Dependencies.DefineDependencies(builder);

// cors
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
    //.WithOrigins("http://localhost:5501", "http://localhost:7106");
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// basic auth
// extend the swagger functionality to append that authorization header to all calls
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
//    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "basic",
//        In = ParameterLocation.Header,
//        Description = "Basic Authorization header."
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "basic"
//                }
//            },
//            new string[] {}
//        }
//    });
//    c.MapType<TimeSpan>(() => new OpenApiSchema
//    {
//        Type = "string",
//        Example = new OpenApiString("00:00:00")
//    });
//});

// Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtAudience,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });
// Jwt configuration ends here

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
       {
         new OpenApiSecurityScheme
         {
           Reference = new OpenApiReference
           {
             Type = ReferenceType.SecurityScheme,
             Id = "Bearer"
           }
          },
          new string[] { }
        }
    });
    // make the timespan with format "00:00:00"
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


app.UseAuthentication();
app.UseAuthorization();

// map all controllers in your application that are derived from `ControllerBase` or `Controller`.
app.MapControllers();

app.Run();
