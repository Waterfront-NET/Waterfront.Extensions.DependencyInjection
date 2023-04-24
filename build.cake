#load build/data/args.cake
#load build/data/paths.cake
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
});



RunTarget(args.Target());
