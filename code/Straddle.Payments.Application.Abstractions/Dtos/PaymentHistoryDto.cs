namespace Straddle.Payments.Application.Dtos;

using System;

public record PaymentHistoryDto(Guid Id, string Type, string User, DateTimeOffset CreatedAt);