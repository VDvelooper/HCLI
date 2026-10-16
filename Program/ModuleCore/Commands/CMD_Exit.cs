using HCLI.Program.Core.Abstractions;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.ModuleCore.Commands
{
    public class CMD_Exit : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }

        public CMD_Exit(string syntax, int requiredArgCount, int optionalArgCount, string description)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
        }

        public IFeedback? Run(UserInput userInput, ModuleBase sender)
        {
            if (!_isSeparateMode) return;
            if (!IsSeparateModeRunning) return;

            IsSeparateModeRunning = false;
        }
    }
}
