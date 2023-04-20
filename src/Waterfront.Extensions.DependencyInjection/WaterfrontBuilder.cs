using Microsoft.Extensions.DependencyInjection;

namespace Waterfront.Extensions.DependencyInjection;

public partial class WaterfrontBuilder : IWaterfrontBuilder
{
    public IServiceCollection Services { get; }

    public WaterfrontBuilder(IServiceCollection services)
    {
        Services = services;
        services.AddOptions();
        ConfigureTokens(tokens => tokens.SetIssuer("Waterfront").SetLifetime(60))
        .WithDefaultTokenEncoder()
        .WithDefaultTokenDefinitionService();
    }
}
