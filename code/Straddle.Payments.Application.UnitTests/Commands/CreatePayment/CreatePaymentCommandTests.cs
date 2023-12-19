namespace Straddle.Payments.Application.UnitTests.Commands.CreatePayment;

using Moq;
using Straddle.Application.Commands;
using Straddle.Payments.Application.Commands;
using Straddle.Payments.Domain.Data;
using Straddle.Payments.Domain.Messages;
using Straddle.Payments.Domain.Model;
using Straddle.Payments.Domain.Services;
using Straddle.UnitTests;

[TestClass]
public class CreatePaymentCommandTests : BaseUnitTest<CreatePaymentCommand>
{
    [TestMethod]
    public async Task Create_Payment_Success()
    {
        // Arrrange
        decimal amount = 100;
        string fromAccount = "fromAccount";
        string toAccount = "toAccount";
        string reference = "reference";
        DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
        DateTimeOffset dateTimeOffset = new(dateOnly, TimeOnly.MinValue, new TimeSpan());

        Payment? createdPayment = null;
        Mock<IPaymentUpdateRepository> mockPaymentUpdateRepository = Create<IPaymentUpdateRepository>();
        mockPaymentUpdateRepository.Setup(m => m.Add(It.IsAny<Payment>())).Callback<Payment>((payment) =>
        {
            createdPayment = payment;
        });

        Mock<IPublisher> mockPublisher = Create<IPublisher>();
        mockPublisher.Setup(m => m.Publish(It.IsAny<Guid>(), It.IsAny<object>(), dateTimeOffset)).Callback<Guid, object, DateTimeOffset?>((id, msg, dateTime) =>
        {
            id.Should().Be(createdPayment!.Id);
            msg.Should().BeOfType<PaymentRequest>();
            (msg as PaymentRequest)!.PaymentId.Should().Be(createdPayment.Id);
        });

        Mock<IPaymentHistoryWriter> mockPaymentHistoryWriter = Create<IPaymentHistoryWriter>();
        mockPaymentHistoryWriter.Setup(m => m.Write(It.IsAny<PaymentId>(), PaymentHistoryType.Created)).Callback<PaymentId, PaymentHistoryType>((id, type) =>
        {
            id.Should().Be(createdPayment!.Id);
            type.Should().Be(PaymentHistoryType.Created);
        });

        CreatePaymentRequest request = new(amount, fromAccount, toAccount, reference, dateOnly);
        CreatePaymentCommand sut = new(MockLogger.Object, mockPaymentUpdateRepository.Object, mockPublisher.Object, mockPaymentHistoryWriter.Object);

        // Act
        CommandResponse<CreatePaymentResponse> response = await sut.Handle(request, default);

        // Assert
        response.HasErrors.Should().BeFalse();

        response.Data!.Payment.Amount.Should().Be(amount);
        response.Data.Payment.FromAccount.Should().Be(fromAccount);
        response.Data.Payment.ToAccount.Should().Be(toAccount);
        response.Data.Payment.Reference.Should().Be(reference);
        response.Data.Payment.Date.Should().Be(dateOnly);

        createdPayment.Should().NotBeNull();
        createdPayment!.Amount.Should().Be(amount);
        createdPayment.FromAccount.Should().Be(fromAccount);
        createdPayment.ToAccount.Should().Be(toAccount);
        createdPayment.Reference.Should().Be(reference);
        createdPayment.Date.Should().Be(dateOnly);
        createdPayment.Status.Should().Be(PaymentStatus.Pending);

        mockPaymentUpdateRepository.VerifyAll();
        mockPublisher.VerifyAll();
        mockPaymentHistoryWriter.VerifyAll();
    }
}