using System.Data;
using System.Text;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RfContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
    // opts.UseNpgsql(builder.Configuration.GetConnectionStringFromEnvironment()));
builder.Services.AddScoped<IStudentsService, StudentsService>();
builder.Services.AddScoped<IStandsService, StandsService>();
builder.Services.AddScoped<IConnectionService, ConnectionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = false,
            // строка, представляющая издателя
            ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
            // будет ли валидироваться потребитель токена
            ValidateAudience = false,
            // установка потребителя токена
            ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value ?? "secretKey")),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy
            .WithOrigins(builder.Configuration.GetSection("DebugFrontendUrl").Value ?? "http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    }));

var app = builder.Build();

app.Lifetime.ApplicationStopping.Register(() =>
{
    app.Logger.LogInformation("Closing opened sessions...");
    using (IServiceScope scope = app.Services.CreateScope())
    {
        var connectionService = scope.ServiceProvider.GetRequiredService<IConnectionService>();
        connectionService.CloseAllOpened();
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("NgOrigins");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
