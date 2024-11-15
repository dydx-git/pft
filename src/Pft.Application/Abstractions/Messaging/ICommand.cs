﻿using MediatR;
using Pft.Domain.Abstractions;

namespace Pft.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{
}
public interface IBaseCommand
{
}
