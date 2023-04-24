#load ../data/*.cake

var mainRestoreTask = Task("restore");

projects.ForEach(project => {
    var task = Task(project.TaskName("restore")).Does(() => {
        DotNetRestore(project.Path.ToString(), new DotNetRestoreSettings {
            NoDependencies = true
        });
    });

    mainRestoreTask.IsDependentOn(task);
});
