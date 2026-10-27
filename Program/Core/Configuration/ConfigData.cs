namespace HCLI.Program.Core.Configuration
{
    internal class ConfigData
    {
        internal string UserName { get; set; }
        internal bool SetupComplete { get; set; }
        internal List<string> ModuleToLoadAtStart { get; set; }
    }
}
