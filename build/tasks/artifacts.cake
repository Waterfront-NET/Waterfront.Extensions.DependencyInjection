#load ../data/*.cake


Task("artifacts/push/nuget/ensure-source").Does(() => {
    Information("Ensuring nuget.org package registry is configured");
    if(DotNetNuGetHasSource("nuget.org")) {
        Information("Source already exists");
        return;
    }

    Information("Configuring source...");
    DotNetNuGetAddSource("nuget.org", new DotNetNuGetSourceSettings {
        Source = "https://api.nuget.org/v3/index.json"
    });
});

Task("artifacts/push/nuget").Does(() => {
    Information("Pushing NuGet packages to nuget.org registry");
    var packages = GetFiles(paths.Packages.Combine("*.nupkg").ToString());
    Verbose("Found {0} packages: [{1}]", packages.Count, string.Join(", ", packages.Select(x => x.GetFilename())));

    if(string.IsNullOrEmpty(apikeys.Nuget)) {
        throw new Exception("Nuget api key not found. Environment variable NUGET_API_KEY has to be set in order for workflow to be able to push packages to nuget.org package registry");
    }

    packages.ToList().ForEach(package => {
        DotNetNuGetPush(package, new DotNetNuGetPushSettings {
            Source = "nuget.org",
            ApiKey = apikeys.Nuget
        });
    });
});
