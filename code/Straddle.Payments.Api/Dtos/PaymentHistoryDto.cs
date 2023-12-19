namespace Straddle.Payments.Api.Dtos;

public record PaymentHistoryDto(Guid Id, string Type, string User, DateTimeOffset CreatedAt);