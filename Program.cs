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

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddSingleton<JwtService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:3000", "https://your-vercel-frontend.vercel.app") // Add your Vercel domain here too!
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("Authorization"));
});

var app = builder.Build();

// ✅ Allow Swagger always (Dev + Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Intern Management API V1");
});

// ✅ Serve static files (important for Swagger UI JS/CSS)
app.UseStaticFiles();

// ✅ Enable CORS
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
