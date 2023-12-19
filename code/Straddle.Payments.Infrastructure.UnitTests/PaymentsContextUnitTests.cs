namespace Straddle.Payments.Infrastructure.UnitTests;

using Microsoft.EntityFrameworkCore;
using Moq;
using Straddle.Payments.Domain.Model;
using Straddle.Payments.Domain.Services;

[TestClass]
public class PaymentsContextUnitTests
{
    [TestMethod]
    [Description("Checks the validity of the EF Model")]
    public async Task Test_Model_IsValid()
    {
        DbContextOptions<PaymentsContext> options = new DbContextOptionsBuilder<PaymentsContext>()
                .UseSqlServer(@"Server=.;Database=Straddle.Payments;Trusted_Connection=True;TrustServerCertificate=True")
                .Options;

        Mock<IUserProvider> mockUserProvider = new Mock<IUserProvider>();

        PaymentsContext sut = new PaymentsContext(options, mockUserProvider.Object);

        Payment? payment = await sut.Payments.FirstOrDefaultAsync();
        PaymentHistory? paymentHistory = await sut.PaymentHistory.FirstOrDefaultAsync();
    }
}