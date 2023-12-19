namespace Straddle.Payments.Infrastructure.Uow;

using MediatR;
using System.Threading.Tasks;

/// <summary>
/// Manages the unit of work the command runs under in the Mediator pipeline.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync(cancellationToken);
            TResponse response = await next();
            await _unitOfWork.CommitAsync(cancellationToken);

            return response;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}