#load ../data/*.cake

var mainPackTask = Task("pack");

projects.Where(p => !p.IsTest()).ToList().ForEach(project => {
    var task = Task(project.TaskName("pack")).Does(() => {
        DotNetPack(project.Path.ToString(), new DotNetPackSettings {
            Configuration = args.Configuration(),
            NoBuild = true
        });

        if(args.Configuration() is "Release" && !args.NoCopyArtifacts) {
            Information("Copying project {0} v{1} NuGet packages to artifacts folder", project.Name, version.SemVer);

            var pattern = $"{project.Name}.{version.SemVer}." + "{nupkg,snupkg}";
            var files = GetFiles(project.Directory.Combine("bin/Release/" + pattern).ToString());
            files.ToList().ForEach(package => {
                Verbose("Copying package {0} to target folder {1}", package, paths.Packages);
                CopyFileToDirectory(package, paths.Packages);
            });
        }
    }).IsDependentOn(project.TaskName("build"));

    mainPackTask.IsDependentOn(task);
});
