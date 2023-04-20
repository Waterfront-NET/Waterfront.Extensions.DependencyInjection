using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Waterfront.Common.Tokens.Encoding;

namespace Waterfront.Extensions.DependencyInjection;

public partial class WaterfrontBuilder
{
    public IWaterfrontBuilder WithTokenEncoder<TEncoder>(
        ServiceLifetime lifetime = ServiceLifetime.Singleton
    ) where TEncoder : class, ITokenEncoder
    {
        Services.Replace(
            ServiceDescriptor.Describe(typeof(ITokenEncoder), typeof(TEncoder), lifetime)
        );
        return this;
    }
}
