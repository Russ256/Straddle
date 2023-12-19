namespace Straddle.Application.Commands;

using MediatR;

public interface ICommandRequest<TResponse> : IRequest<CommandResponse<TResponse>>
{
}