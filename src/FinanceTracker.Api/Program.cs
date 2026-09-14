using FinanceTracker.Api;
using FinanceTracker.Api.Extensions;
using FinanceTracker.Infrastructure;
using FinanceTracker.Infrastructure.Persistence;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

app.UseExceptionMiddleware();
app.UseHttpsRedirection();

await DatabaseInitializer.InitializeAsync(app.Services);

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.Logger.LogInformation("Finance Tracker API is starting in development mode");
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Run();
