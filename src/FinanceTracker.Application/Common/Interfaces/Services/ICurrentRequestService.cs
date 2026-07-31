namespace FinanceTracker.Application.Common.Interfaces.Services;

public interface ICurrentRequestService
{
    string IpAddress { get; }
    string UserAgent { get; }
}
