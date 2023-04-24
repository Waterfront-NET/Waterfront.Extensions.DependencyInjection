#load paths.cake

using System.Xml;

static List<BuildProject> projects;

projects = new List<BuildProject> {
    new BuildProject {
        Name = "Waterfront.Extensions.DependencyInjection",
        Path = paths.Src.CombineWithFilePath("Waterfront.Extensions.DependencyInjection/Waterfront.Extensions.DependencyInjection.csproj")
    },
    new BuildProject {
        Name = "Waterfront.Extensions.DependencyInjection.Tests",
        Path = paths.Test.CombineWithFilePath("Waterfront.Extensions.DependencyInjection.Tests/Waterfront.Extensions.DependencyInjection.Tests.csproj")
    }
};


class BuildProject {
    private List<BuildProject> _dependencies;

    public string Name { get; init; }
    public FilePath Path { get; init; }
    public string ShortName => Name.Replace("Waterfront.", string.Empty).ToLowerInvariant();
    public DirectoryPath Directory => Path.GetDirectory();
    public bool IsTest() => Path.GetFilenameWithoutExtension().ToString().EndsWith(".Tests");
    public BuildProject() {
    }

    public string TaskName(string taskType) {
        return $":{ShortName}:{taskType}";
    }

    public List<BuildProject> Dependencies() {

        if(_dependencies == null) {
            _dependencies = new List<BuildProject>();
            var doc = new XmlDocument();
            doc.Load(Path.ToString());

            var projectReferenceNodes = doc.SelectNodes("//ProjectReference");
            if(projectReferenceNodes != null) {
                foreach(XmlNode node in projectReferenceNodes) {
                    XmlNode includeAttr = node.Attributes.GetNamedItem("Include");
                    if(includeAttr == null) {
                        continue;
                    }

                    string relPath = includeAttr.Value;
                    FilePath absPath = FilePath.FromString(relPath).MakeAbsolute(Directory);
                    BuildProject requiredProject = projects.Find(project => project.Path == absPath);

                    if(requiredProject == null) {
                        throw new Exception($"Dependency for project {Name} was not found: " + absPath.ToString());
                    }

                    _dependencies.Add(requiredProject);
                }
            }
        }


        return _dependencies;
    }
}
