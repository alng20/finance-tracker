using FinanceTracker.Api.Exceptions;

namespace FinanceTracker.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddSingleton<IExceptionResponseMapper, ExceptionResponseMapper>();

        return services;
    }
}
