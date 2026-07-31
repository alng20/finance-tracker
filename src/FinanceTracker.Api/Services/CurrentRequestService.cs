using FinanceTracker.Application.Common.Interfaces.Services;

public class CurrentRequestService : ICurrentRequestService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentRequestService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string IpAddress =>
        _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

    public string UserAgent =>
        _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString() ?? "Unknown";
}
