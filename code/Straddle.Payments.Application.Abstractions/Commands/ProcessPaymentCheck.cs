﻿namespace Straddle.Payments.Application.Commands;

using Straddle.Application.Commands;

public record ProcessPaymentCheckRequest(Guid Id) : ICommandRequest<ProcessPaymentCheckResponse>;

public record ProcessPaymentCheckResponse();