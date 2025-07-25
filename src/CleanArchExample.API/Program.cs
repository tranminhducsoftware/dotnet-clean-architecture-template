// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Infrastructure;
using CleanArchExample.Application;
using CleanArchExample.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using CleanArchExample.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CleanArchExample.Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using CleanArchExample.API.Middlewares;
// using OpenTelemetry.Instrumentation.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext
// builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("CleanArchExampleDB"));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add Application Layer (MediatR)
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration); // <- Gọi hàm mở rộng vừa tạo
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Thêm authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(builder.Environment.ApplicationName))
            .AddAspNetCoreInstrumentation()
            // .AddSqlClientInstrumentation()
            .AddConsoleExporter(); // Export ra console (dễ debug dev)
            // .AddJaegerExporter() // Nếu muốn export Jaeger
            // .AddZipkinExporter() // Nếu muốn export Zipkin
    });
    
var app = builder.Build();

app.MapControllers(); // <- để Controller được map route
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>(); // bắt lỗi toàn cục
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
