namespace HCLI.Program.Base
{
    internal interface ICommandModule
    {
        string Name { get; set; }
        string ModuleCommand { get; set; }
        Dictionary<string, Action<string[]>> SubCommands { get; set; }
        void Execute(string[] args);
    }
}
