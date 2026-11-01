using ModuleContracts.Abstractions;
using ModuleContracts.Feedback;

namespace HCLI.Program.Core.Commands
{
    internal class CMD_ListDetectedModules : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }
        public ModuleBase? ParentModule { get; }

        public CMD_ListDetectedModules(string syntax, int requiredArgCount, int optionalArgCount, string description, ModuleBase? parentModule = null)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ParentModule = parentModule;
        }

        public IFeedback? Run(UserInput userInput)
        {
            if (Runtime.ModuleRegistry.DetectedModules.Count <= 0)
            {
                Runtime.ConsoleView.Message($"There is no dll file found at the storage path '{Runtime.ModuleRegistry.ModuleDllStoragePath}'.");
                return null;
            }

            foreach (KeyValuePair<Type, string> pair in Runtime.ModuleRegistry.DetectedModules)
            {
                Runtime.ConsoleView.Message(pair.Key.Name);
            }
            return null;
        }
    }
}
