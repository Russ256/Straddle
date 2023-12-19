namespace Straddle.Payments.Api.Security;

using Straddle.Payments.Domain.Services;

public class UserProvider : IUserProvider
{
    public string Name
    {
        get => "System";
    }
}