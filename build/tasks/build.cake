#load ../data/*.cake

var mainBuildTask = Task("build");

projects.ForEach(project => {
    var task = Task(project.TaskName("build")).Does(() => {
        DotNetBuild(project.Path.ToString(), new DotNetBuildSettings {
            Configuration = args.Configuration(),
            NoRestore = true,
            NoDependencies = true
        });

        if(args.Configuration() is "Release" && !args.NoCopyArtifacts && !project.IsTest()) {
            Information("Compressing build artifacts for project {0} v{1}", project.Name, version.SemVer);

            var sourceDir = project.Directory.Combine("bin/Release/net6.0");
            var archiveFilename = $"{project.Name}.{version.SemVer}.zip";
            var targetArchive = paths.Libraries.CombineWithFilePath(archiveFilename);

            Verbose("Compressing directory {0} to target archive {1}", sourceDir, targetArchive);

            Zip(sourceDir, targetArchive);
        }
    }).IsDependentOn(project.TaskName("restore"))
      .WithCriteria(!args.NoBuild);

    project.Dependencies().ForEach(dep => {
        task.IsDependentOn(dep.TaskName("build"));
    });

    mainBuildTask.IsDependentOn(task);
});
