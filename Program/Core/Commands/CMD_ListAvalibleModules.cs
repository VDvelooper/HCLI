using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;
using HCLI.Program.ModuleCore;

namespace HCLI.Program.Core.Commands
{
    public class CMD_ListAvalibleModules : ICommand
    {
        public string Syntax { get; }
        public string Description { get; }

        public CMD_ListAvalibleModules(string syntax, string description)
        {
            Syntax = syntax;
            Description = description;
        }

        public void Execute(UserInput userInput)
        {
            Console.WriteLine();
            foreach (ModuleData module in Runtime.ModuleRegistry.AVALIBLE_MODULES)
            {
                Console.WriteLine(module.Name);
            }
            Console.WriteLine();
        }
    }
}