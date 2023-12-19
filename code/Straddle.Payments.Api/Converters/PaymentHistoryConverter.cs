namespace Straddle.Payments.Api.Converters;

using Straddle.Payments.Api.Dtos;

public static class PaymentHistoryConverter
{
    public static PaymentHistoryDto ToDto(this Application.Dtos.PaymentHistoryDto value)
    {
        return new PaymentHistoryDto(value.Id, value.Type, value.User, value.CreatedAt);
    }

    public static IEnumerable<PaymentHistoryDto> ToDto(this IEnumerable<Application.Dtos.PaymentHistoryDto> value)
    {
        foreach (Application.Dtos.PaymentHistoryDto item in value)
        {
            yield return item.ToDto();
        }
    }
}