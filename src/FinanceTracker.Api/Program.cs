using FinanceTracker.Api;
using FinanceTracker.Api.Extensions;
using FinanceTracker.Infrastructure;
using FinanceTracker.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

await DatabaseInitializer.InitializeAsync(app.Services);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Run();
