using System.Text;
using System.Text.Json.Serialization;

using FinanceTracker.Api.Common.Configuration;
using FinanceTracker.Api.Common.Options;
using FinanceTracker.Api.Common.Options.Validators;
using FinanceTracker.Api.Exceptions;
using FinanceTracker.Api.Services;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Common.Options;
using FinanceTracker.Application.Common.Options.Validators;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
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

        services.AddSingleton<IValidateOptions<CorsPolicyOptions>, CorsPolicyOptionsValidator>();
        services
            .AddOptions<CorsPolicyOptions>()
            .Bind(configuration.GetSection(CorsPolicyOptions.SectionName))
            .ValidateOnStart();
        services.AddSingleton<IConfigureOptions<CorsOptions>, CorsPolicyConfiguration>();

        services.AddSingleton<
            IValidateOptions<RefreshTokenCookieOptions>,
            RefreshTokenCookieOptionsValidator
        >();
        services
            .AddOptions<RefreshTokenCookieOptions>()
            .Bind(configuration.GetSection(RefreshTokenCookieOptions.SectionName))
            .ValidateOnStart();

        services.AddCors();

        services.AddHttpContextAccessor();

        services.AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();
        services.AddSingleton<
            IValidateOptions<RefreshTokenOptions>,
            RefreshTokenOptionsValidator
        >();
        services.AddSingleton<IExceptionResponseMapper, ExceptionResponseMapper>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ICurrentRequestService, CurrentRequestService>();

        services
            .AddOptions<RefreshTokenOptions>()
            .Bind(configuration.GetSection(RefreshTokenOptions.SectionName))
            .ValidateOnStart();

        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateOnStart();
        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerConfiguration>();

        services.AddSingleton<
            IValidateOptions<AppFrwdHeadersOptions>,
            AppFrwdHeadersOptionsValidator
        >();
        services
            .AddOptions<AppFrwdHeadersOptions>()
            .Bind(configuration.GetSection(AppFrwdHeadersOptions.SectionName))
            .ValidateOnStart();
        services.AddSingleton<
            IConfigureOptions<ForwardedHeadersOptions>,
            ForwardedHeadersConfiguration
        >();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.AddAuthorization();

        return services;
    }
}
