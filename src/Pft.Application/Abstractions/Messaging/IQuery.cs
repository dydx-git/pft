using MediatR;
using Pft.Domain.Abstractions;

namespace Pft.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
