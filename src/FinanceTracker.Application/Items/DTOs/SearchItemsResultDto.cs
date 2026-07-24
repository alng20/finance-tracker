using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Items.DTOs;

public record SearchItemsResultDto(Guid Id, string Name, Guid? CategoryId, Unit unit);
