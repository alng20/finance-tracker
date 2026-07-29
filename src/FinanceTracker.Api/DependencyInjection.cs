using System.Text;
using FinanceTracker.Api.Exceptions;
using FinanceTracker.Api.Services;
using FinanceTracker.Application.Common.Interfaces.Services;
using FinanceTracker.Application.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FinanceTracker.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();
        services.AddHttpContextAccessor();

        services.AddSingleton<IExceptionResponseMapper, ExceptionResponseMapper>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        JwtOptions jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;

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
