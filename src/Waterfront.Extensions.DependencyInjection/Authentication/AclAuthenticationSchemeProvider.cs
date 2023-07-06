using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waterfront.Common.Authentication;
using Waterfront.Core.Authentication;

namespace Waterfront.Extensions.DependencyInjection.Authentication;

public class AclAuthenticationSchemeProvider : AclAuthenticationSchemeProviderBase
{
    private readonly IOptions<AclAuthenticationOptions> _options;

    public override IReadOnlyCollection<AclAuthenticationScheme> Schemes => _options.Value.Schemes;
    public override IReadOnlyDictionary<string, AclAuthenticationScheme> SchemeMap => _options.Value.SchemeMap;
    protected ILogger Logger { get; }

    public AclAuthenticationSchemeProvider(ILoggerFactory loggerFactory, IOptions<AclAuthenticationOptions> options)
    {
        Logger = loggerFactory.CreateLogger(GetType());
        _options = options;
    }
}
