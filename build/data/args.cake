var args = new BuildArguments { Context = Context };

class BuildArguments {
    public ICakeContext Context { get; init; }

    public string Configuration() => Context.Argument("configuration", Context.Argument("c", "Debug"));
    public string Target() => Context.Argument("target", Context.Argument("t", "build"));
    public bool NoCopyArtifacts => Context.HasArgument("no-copy-artifacts");
    public bool NoBuild => Context.HasArgument("no-build");
}
