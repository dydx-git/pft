﻿using MediatR;
using Microsoft.Extensions.Logging;
using Pft.Domain.Abstractions;
using Serilog.Context;

namespace Pft.Application.Abstractions.Behaviors;

/// <summary>
/// Loggings, for the commands pipeline only
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;

        try
        {
            logger.LogInformation("Executing request {Request}", name);

            var result = await next();

            if (result.IsSuccess)
            {
                logger.LogInformation("Request {Request} processed successfully", name);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Request {Request} processed with error", name);
                }
            }            

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Request {Request} processing failed", name);

            throw;
        }
    }
}