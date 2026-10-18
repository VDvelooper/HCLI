using HCLI.Program.Core;
using HCLI.Program.Core.Abstractions;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.ModuleCore.Commands
{
    internal class CMD_Help : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }
        public ModuleBase ParentModule { get; }

        public CMD_Help(string syntax, int requiredArgCount, int optionalArgCount, string description, ModuleBase parentModule)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ParentModule = parentModule;
        }


        public IFeedback? Run(UserInput userInput)
        {
            ParentModule.BaseCommands.ForEach(command => ConsoleView.Message($"{command.Syntax} - {command.Description}"));

            return null;
        }
    }
}
