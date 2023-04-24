#load ../data/*.cake

var mainBuildTask = Task("build");

projects.ForEach(project => {
    var task = Task(project.TaskName("build")).Does(() => {
        DotNetBuild(project.Path.ToString(), new DotNetBuildSettings {
            Configuration = args.Configuration()
        });

        if(args.Configuration() is "Release") {
            Information("Copying build output to artifacts folder");
        }
    }).IsDependentOn(project.TaskName("restore"));

    project.Dependencies().ForEach(dep => {
        task.IsDependentOn(dep.TaskName("build"));
    });

    mainBuildTask.IsDependentOn(task);
});
