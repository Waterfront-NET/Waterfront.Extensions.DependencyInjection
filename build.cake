#load build/data/*.cake
#load build/tasks/*.cake

Setup(ctx => {
    EnsureDirectoryExists(paths.Packages);
    EnsureDirectoryExists(paths.Libraries);


    projects.ForEach(project => {
        Verbose(
            "Project {0} dependencies: [{1}]",
            project.Name,
            string.Join(", ", project.Dependencies().Select(dep => dep.Name))
        );
    });

    Environment.SetEnvironmentVariable("SEMVER", version.SemVer);
    Environment.SetEnvironmentVariable("INFO_VER", version.InformationalVersion);
});



RunTarget(args.Target());
