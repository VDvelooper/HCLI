using ModuleContracts.Abstractions;
using ModuleContracts.Feedback;

namespace HCLI.Program.Core.Commands
{
    internal class CMD_Help : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }
        public ModuleBase? ParentModule { get; }

        public CMD_Help(string syntax, int requiredArgCount, int optionalArgCount, string description, ModuleBase? parentModule = null)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ParentModule = parentModule;
        }

        public IFeedback? Run(UserInput userInput) 
        {
            foreach (KeyValuePair<string, ICommand> command in Runtime.CommandRegistry.Commands)
            {
                Runtime.ConsoleView.Message($"{command.Value.Syntax} - {command.Value.Description}");
            }

            return null;
        }
    }
}
