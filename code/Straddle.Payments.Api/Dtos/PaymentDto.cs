namespace Straddle.Payments.Api.Dtos;

public class PaymentDto()
{
    public Guid? Id { get; set; }
    public string? Status { get; set; }
    public decimal? Amount { get; set; }
    public string? FromAccount { get; set; }
    public string? ToAccount { get; set; }
    public string? Reference { get; set; }
    public DateOnly? Date { get; set; }
    public IEnumerable<PaymentHistoryDto>? Histoy { get; set; }
}
