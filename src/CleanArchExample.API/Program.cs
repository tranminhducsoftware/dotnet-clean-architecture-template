// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Infrastructure;
using CleanArchExample.Application;
using CleanArchExample.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CleanArchExample.Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using CleanArchExample.API.Middlewares;
using Serilog;
using CleanArchExample.API.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using CleanArchExample.Infrastructure.Health;
using HealthChecks.UI.Client;
// using EFCoreSecondLevelCacheInterceptor;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Production;

var builder = WebApplication.CreateBuilder(args);

// Đọc config từ nhiều nguồn (ưu tiên ENV > user-secrets > secret.json > appsettings)
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("secret.json", optional: true)                 // Chỉ local/dev mới có
    .AddUserSecrets<Program>(optional: true)                    // Dev nên dùng
    .AddEnvironmentVariables();                                 // ENV luôn ưu tiên nhất
// 👇 Serilog config
builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration)
          .Enrich.FromLogContext()
          //   .Enrich.WithMachineName()
          //   .Enrich.WithThreadId()
          .Enrich.WithProperty("App", "CleanArchExample");
});
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext
// Add Application Layer (MediatR)
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration); // <- Gọi hàm mở rộng vừa tạo
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(opt => { /* tắt auto 400 nếu muốn custom */ });
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
});

builder.Services.AddMemoryCache();
// Đăng ký Swagger service
builder.Services.AddEndpointsApiExplorer(); // Khuyến nghị thêm dòng này
builder.Services.AddSwaggerGen();
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
            .AddEntityFrameworkCoreInstrumentation()
            .AddSqlClientInstrumentation()
            .AddConsoleExporter(); // Export ra console (dễ debug dev)
                                   // .AddJaegerExporter() // Nếu muốn export Jaeger
                                   // .AddZipkinExporter() // Nếu muốn export Zipkin
    });

builder.Services.AddAppSecondLevelCache(builder.Configuration);

var app = builder.Build();

app.MapControllers(); // <- để Controller được map route
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>(); // bắt lỗi toàn cục
app.UseMiddleware<LogContextMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    Predicate = reg => reg.Tags.Contains("live"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/readyz", new HealthCheckOptions
{
    Predicate = reg => reg.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

var appHealth = app.Services.GetRequiredService<IAppHealthService>();
app.Lifetime.ApplicationStopping.Register(() =>
{
    appHealth.MarkAsNotReady();
    Thread.Sleep(5000); // Phù hợp với preStop sleep 5
});


app.UseHttpsRedirection();
app.Run();
