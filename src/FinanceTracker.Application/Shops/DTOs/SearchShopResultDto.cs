namespace FinanceTracker.Application.Shops.DTOs;

public record SearchShopResultDto(Guid Id, string Name, Guid? RetailerId, string? RetailerName, string? Country, string? City);
