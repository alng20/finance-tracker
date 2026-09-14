using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Items.DTOs;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Common.Interfaces.Providers;

public interface IItemProvider
{
    Task<PagedResult<GetItemsResultDto>> GetAsync(
        int page,
        int pageSize,
        IReadOnlyCollection<Guid>? CategoryIds,
        IReadOnlyCollection<Unit>? Units,
        CancellationToken cancellationToken
    );
}
