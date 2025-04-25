using System.Data;
using System.Text;
using InternManagementAPI.Repositories;
using InternManagementAPI.Services;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Intern Management API", Version = "v1" });

    var jwtSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Description = "Enter 'Bearer' followed by your token.",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Id = "Bearer",
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddScoped<InternRepository>();
builder.Services.AddScoped<InternService>();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables(); // Adds environment variables as a fallback for production.

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // From appsettings.json or environment
            ValidAudience = builder.Configuration["Jwt:Audience"], // From appsettings.json or environment
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // From appsettings.json or environment
        };
    });


builder.Services.AddAuthorization();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("CONNECTIONSTRINGS_DEFAULTCONNECTION")));

builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddSingleton<JwtService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins(
                "https://intern-frontend-sooty.vercel.app",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Authorization"));
});


var app = builder.Build();
app.UseCors("AllowFrontend");




// ✅ Allow Swagger always (Dev + Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Intern Management API V1");
});

// ✅ Serve static files (important for Swagger UI JS/CSS)
app.UseStaticFiles();

// ✅ Enable CORS


app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
