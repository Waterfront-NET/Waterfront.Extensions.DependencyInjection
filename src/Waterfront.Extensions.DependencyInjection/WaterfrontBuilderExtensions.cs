using Waterfront.Core.Tokens.Definition;
using Waterfront.Core.Tokens.Encoding;
using Waterfront.Core.Tokens.Signing.CertificateProviders.Files;

namespace Waterfront.Extensions.DependencyInjection;

public static class WaterfrontBuilderExtensions
{
    public static IWaterfrontBuilder WithDefaultTokenEncoder(this IWaterfrontBuilder self)
    {
        return self.WithTokenEncoder<TokenEncoder>();
    }

    public static IWaterfrontBuilder WithDefaultTokenDefinitionService(this IWaterfrontBuilder self)
    {
        return self.WithTokenDefinitionService<TokenDefinitionService>();
    }

    public static IWaterfrontBuilder WithFileSigningCertificateProvider(
        this IWaterfrontBuilder self,
        Action<FileSigningCertificateProviderOptions> configureOptions
    )
    {
        return self
        .WithSigningCertificateProvider<FileSigningCertificateProvider,
            FileSigningCertificateProviderOptions>(configureOptions);
    }

    public static IWaterfrontBuilder WithFileSigningCertificateProvider(
        this IWaterfrontBuilder self,
        string certificatePath,
        string privateKeyPath
    )
    {
        return self.WithFileSigningCertificateProvider(
            opt => {
                opt.CertificatePath = certificatePath;
                opt.PrivateKeyPath  = privateKeyPath;
            }
        );
    }
}
