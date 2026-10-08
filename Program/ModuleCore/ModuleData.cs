using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;
using HCLI.Program.ModuleCore.Abstractions;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.ModuleCore
{
    public class ModuleData
    {
        public string Name { get; private set; }
        public string ModuleCommand { get; private set; }
        public List<ICommand> SubCommands { get; private set; }
        public ModuleBase Module { get; private set; }

        public ModuleData(string name, string command, ModuleBase module, List<ICommand> subcommands)
        {
            Name = name;
            ModuleCommand = command;
            Module = module;
            SubCommands = subcommands;
        }
    }
}
