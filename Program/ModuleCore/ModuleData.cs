using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.ModuleCore
{
    public class ModuleData
    {
        public string Name { get; private set; }
        public string ModuleCommand { get; private set; }
        public Action<ModuleModeUserInput> Execute { get; private set; }

        public ModuleData(string name, string command, Action<ModuleModeUserInput> execute)
        {
            Name = name;
            ModuleCommand = command;
            Execute = execute;
        }
    }
}
