#load ../data/*.cake

var packages = GetFiles(paths.Packages.Combine("*.nupkg").ToString());

Task("artifacts/push/nuget")
.WithCriteria(packages.Any())
.IsDependentOn("artifacts/push/nuget/setup-source")
.DoesForEach(packages, package => {
    Verbose("Pushing package {0} to nuget.org package registry", package.GetFilename());
    DotNetNuGetPush(package, new DotNetNuGetPushSettings {
        Source = "nuget.org",
        ApiKey = apikeys.Nuget
    });
});

Task("artifacts/push/nuget/setup-source")
.WithCriteria(!DotNetNuGetHasSource("nuget.org"))
.Does(() => {
    Information("Setting up nuget.org package source");
    DotNetNuGetAddSource("nuget.org", new DotNetNuGetSourceSettings {
        Source = "https://api.nuget.org/v3/index.json"
    });
});

Task("artifacts/push/github")
.WithCriteria(packages.Any())
.IsDependentOn("artifacts/push/github/setup-source")
.DoesForEach(packages, package => {
    Verbose("Pushing NuGet package {0} to github.com package registry", package.GetFilename());
    DotNetNuGetPush(package, new DotNetNuGetPushSettings {
        Source = "github.com",
        ApiKey = apikeys.Github
    });
});

Task("artifacts/push/github/setup-source").Does(() => {
    Information("Setting up github.com package registry...")
    DotNetNuGetAddSource("github.com", new DotNetNuGetSourceSettings {
        Source = "https://nuget.pkg.github.com/Waterfront-NET/index.json",
        UserName = "USERNAME",
        Password = apikeys.Github,
        StorePasswordInClearText = true
    });
}).WithCriteria(() => !DotNetNuGetHasSource("github.com"));
