using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Items.DTOs;

public record GetItemsResultDto(Guid Id, string Name, Guid? CategoryId, string? CategoryName, Unit unit);
