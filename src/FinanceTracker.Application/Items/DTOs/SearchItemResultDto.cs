using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Items.DTOs;

public record SearchItemResultDto(Guid Id, string Name, Guid CategoryId, string CategoryName, Unit unit);
