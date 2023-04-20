using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Waterfront.Common.Tokens.Definition;

namespace Waterfront.Extensions.DependencyInjection;

public partial class WaterfrontBuilder
{
    public IWaterfrontBuilder WithTokenDefinitionService<TService>(
        ServiceLifetime lifetime = ServiceLifetime.Scoped
    ) where TService : class, ITokenDefinitionService
    {
        Services.Replace(
            ServiceDescriptor.Describe(typeof(ITokenDefinitionService), typeof(TService), lifetime)
        );
        return this;
    }
}
