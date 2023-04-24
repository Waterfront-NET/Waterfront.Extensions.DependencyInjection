#load build/data/args.cake
#load build/tasks/*.cake


projects.ForEach(project => {
    Information("Project {0} dependencies: [{1}]", project.Name, string.Join(", ", project.Dependencies().Select(dep => dep.Name)));
});

RunTarget(args.Target());
