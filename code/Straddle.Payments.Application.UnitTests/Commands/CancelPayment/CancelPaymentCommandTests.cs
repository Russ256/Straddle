namespace Straddle.Payments.Application.UnitTests.Commands.CancelPayment;

using Moq;
using Straddle.Application.Commands;
using Straddle.Payments.Application.Commands;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Model;
using Straddle.UnitTests;
using System.Linq.Expressions;

[TestClass]
public class CancelPaymentCommandTests : BaseUnitTest<CancelPaymentCommand>
{
    [TestMethod]
    [DataRow(PaymentStatus.Pending, PaymentStatus.Cancelled, PaymentHistoryType.Cancelled)]
    [DataRow(PaymentStatus.Processing, PaymentStatus.Processing, PaymentHistoryType.CancelFailed)]
    [DataRow(PaymentStatus.Cancelled, PaymentStatus.Cancelled, PaymentHistoryType.CancelFailed)]
    [DataRow(PaymentStatus.Completed, PaymentStatus.Completed, PaymentHistoryType.CancelFailed)]
    [DataRow(PaymentStatus.Errored, PaymentStatus.Errored, PaymentHistoryType.CancelFailed)]
    public async Task Cancel_Payment(PaymentStatus actualStatus, PaymentStatus expectedStatus, PaymentHistoryType expectedHistoryType)
    {
        // Arrrange
        Payment payment = new()
        {
            Id = PaymentId.New(),
            Status = actualStatus,
        };

        Mock<IPaymentUpdateRepository> mockPaymentUpdateRepository = Create<IPaymentUpdateRepository>();
        mockPaymentUpdateRepository.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Payment, bool>>>(), default)).ReturnsAsync(payment);

        Mock<IPaymentHistoryWriter> mockPaymentHistoryWriter = Create<IPaymentHistoryWriter>();
        mockPaymentHistoryWriter.Setup(m => m.Write(It.IsAny<PaymentId>(), expectedHistoryType)).Callback<PaymentId, PaymentHistoryType>((id, type) =>
        {
            id.Should().Be(payment.Id);
            type.Should().Be(expectedHistoryType);
        });

        CancelPaymentRequest request = new(payment.Id);
        CancelPaymentCommand sut = new(MockLogger.Object, mockPaymentUpdateRepository.Object, mockPaymentHistoryWriter.Object);

        // Act
        CommandResponse<CancelPaymentResponse> response = await sut.Handle(request, default);

        // Assert
        response.HasErrors.Should().BeFalse();
        payment.Status.Should().Be(expectedStatus);

        mockPaymentUpdateRepository.VerifyAll();
        mockPaymentHistoryWriter.VerifyAll();
    }
}