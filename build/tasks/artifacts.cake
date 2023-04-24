#load ../data/*.cake

var mainPackagesTask = Task("artifacts/pkg");

projects.Where(p => !p.IsTest()).ToList().ForEach(project => {
    var task = Task(project.TaskName("artifacts/pkg")).Does(() => {
        var packageFiles = GetFiles(project.Directory.Combine("bin/Release/*.{nupkg,snupkg}").ToString());

        packageFiles.ToList().ForEach(file => CopyFileToDirectory(file, paths.Packages));
    }).IsDependentOn(project.TaskName("pack"))
    .WithCriteria(args.Configuration() is "Release", "Configuration is not 'Release'");

    mainPackagesTask.IsDependentOn(task);
});
