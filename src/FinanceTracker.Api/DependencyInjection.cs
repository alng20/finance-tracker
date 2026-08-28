using System.Text;
using System.Text.Json.Serialization;
using FinanceTracker.Api.Common.Options;
using FinanceTracker.Api.Exceptions;
using FinanceTracker.Api.Services;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace FinanceTracker.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("FinanceTracker.Api");

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer(
                (document, context, cancellationToken) =>
                {
                    document.Components ??= new();

                    document.Components.SecuritySchemes ??=
                        new Dictionary<string, IOpenApiSecurityScheme>();

                    document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                    };

                    return Task.CompletedTask;
                }
            );
        });

        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        CorsOptions? frontendOptions = configuration
            .GetSection(CorsOptions.SectionName)
            .Get<CorsOptions>();
        if (frontendOptions == null)
        {
            logger.LogWarning("CORS options is absent");
        }

        RefreshTokenCookieOptions refreshTokenCookieOptions =
            configuration
                .GetSection(RefreshTokenCookieOptions.SectionName)
                .Get<RefreshTokenCookieOptions>()
            ?? throw new InvalidOperationException(
                "Refresh token cookies configuration is absent."
            );

        services.AddCors(options =>
        {
            options.AddPolicy(
                "Frontend",
                policy =>
                {
                    policy
                        .WithOrigins(frontendOptions!.AllowedHosts.ToArray())
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            );
        });

        services.AddHttpContextAccessor();

        services.AddSingleton<IExceptionResponseMapper, ExceptionResponseMapper>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ICurrentRequestService, CurrentRequestService>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<RefreshTokenOptions>(
            configuration.GetSection(RefreshTokenOptions.SectionName)
        );

        JwtOptions jwtOptions =
            configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWT configuration is absent.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey)
                    ),
                };
            });

        services.AddAuthorization();

        return services;
    }
}
