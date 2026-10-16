using HCLI.Program.Core;
using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared.Feedback.Error;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.ModuleCore.Commands
{
    internal class CMD_Help : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }

        public CMD_Help(string syntax, int requiredArgCount, int optionalArgCount, string description)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
        }


        public IFeedback? Run(UserInput userInput, ModuleBase? sender)
        {
            if (sender == null) return new NotFound("The sender module is null.");

            sender.BaseCommands.ForEach(command => ConsoleView.Message($"{command.Syntax} - {command.Description}"));

            return null;
        }
    }
}
