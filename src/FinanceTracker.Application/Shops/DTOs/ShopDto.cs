namespace FinanceTracker.Application.Shops.DTOs;

public record ShopDto(Guid Id, string Name, Guid? RetailerId, string? RetailerName, string? Country, string? City);
