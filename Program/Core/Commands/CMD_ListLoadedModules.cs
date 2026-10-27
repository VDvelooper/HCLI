using ModuleContracts.Abstractions;
using ModuleContracts.Feedback;

namespace HCLI.Program.Core.Commands
{
    internal class CMD_ListLoadedModules : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }
        public ModuleBase? ParentModule { get; }

        public CMD_ListLoadedModules(string syntax, int requiredArgCount, int optionalArgCount, string description, ModuleBase? parentModule = null)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ParentModule = parentModule;
        }

        public IFeedback? Run(UserInput userInput)
        {
            if (Runtime.ModuleRegistry.LoadedModules.Count <= 0)
            {
                Runtime.ConsoleView.Message($"There is no module loaded at this time.");
                return null;
            }

            foreach (ModuleBase module in Runtime.ModuleRegistry.LoadedModules)
            {
                Runtime.ContractsForModules.ConsoleViewContract.Message(module.ModuleName);
            }
            return null;
        }
    }
}