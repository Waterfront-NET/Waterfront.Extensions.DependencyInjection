#load ../data/*.cake

var mainTestTask = Task("test");

projects.Where(p => p.IsTest()).ToList().ForEach(project => {
    var task = Task(project.TaskName("test")).Does(() => {
        DotNetTest(project.Path.ToString(), new DotNetTestSettings {
            Configuration = args.Configuration(),
            NoBuild = true
        });
    }).IsDependentOn(project.TaskName("build"));

    mainTestTask.IsDependentOn(task);
});
