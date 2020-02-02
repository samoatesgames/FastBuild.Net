namespace FastBuild.Net
{
    public class FastBuildStartInfo
    {
        public string FastBuildExecutableLocation { get; set; }
        public string FastBuildBffLocation { get; set; }

        public bool GenerateReport { get; set; }
        public bool Verbose { get; set; }

        public bool ShowCommands { get; set; }
        public bool ShowDependencies { get; set; }
    }
}
