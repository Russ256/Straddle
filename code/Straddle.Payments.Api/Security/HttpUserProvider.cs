namespace Straddle.Payments.Api.Security;

using Straddle.Payments.Domain.Services;
using System.Security.Claims;

public class HttpUserProvider : IUserProvider
{
    private Guid? _id;
    private string? _name;

    public Guid Id
    {
        get => _id.Value;
    }

    public string Name
    {
        get => _name;
    }

    public HttpUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        string oid = httpContextAccessor.HttpContext.User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        if (!string.IsNullOrEmpty(oid))
        {
            _id = new Guid(oid);
        }

        _name = httpContextAccessor.HttpContext.User.FindFirstValue("preferred_username");
    }
}