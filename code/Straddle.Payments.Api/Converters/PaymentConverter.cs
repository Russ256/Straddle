namespace Straddle.Payments.Api.Converters;

using Straddle.Payments.Api.Dtos;

public static class PaymentConverter
{
    public static PaymentDto ToDto(this Application.Dtos.PaymentDto value)
    {
        return new PaymentDto()
        {
            Id = value.Id,
            Status = value.Status,
            Amount = value.Amount,
            FromAccount = value.FromAccount,
            ToAccount = value.ToAccount,
            Reference = value.Reference,
            Date = value.Date,
            Histoy = value.Histoy?.ToDto()
        };
    }
}