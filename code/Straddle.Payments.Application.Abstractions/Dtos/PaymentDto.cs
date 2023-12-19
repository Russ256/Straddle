namespace Straddle.Payments.Application.Dtos;

using System;

public record PaymentDto(Guid Id, string Status, decimal Amount, string FromAccount, string ToAccount, string Reference, DateOnly Date, IEnumerable<PaymentHistoryDto>? Histoy = null);
