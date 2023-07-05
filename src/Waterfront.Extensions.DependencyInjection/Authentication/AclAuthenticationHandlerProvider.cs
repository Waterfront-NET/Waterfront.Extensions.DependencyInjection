using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waterfront.Common.Authentication;

namespace Waterfront.Extensions.DependencyInjection.Authentication;

// TODO: Implement caching

public class AclAuthenticationHandlerProvider : IAclAuthenticationHandlerProvider
{
    protected ILogger Logger { get; }
    protected IServiceProvider ServiceProvider { get; }

    public AclAuthenticationHandlerProvider(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        Logger = loggerFactory.CreateLogger(GetType());
        ServiceProvider = serviceProvider;
    }

    public Task<IAclAuthenticationHandler> GetHandlerAsync(AclAuthenticationScheme scheme)
    {
        object handler = ActivatorUtilities.CreateInstance(ServiceProvider, scheme.HandlerType);
        return Task.FromResult((IAclAuthenticationHandler)handler);
    }
}
