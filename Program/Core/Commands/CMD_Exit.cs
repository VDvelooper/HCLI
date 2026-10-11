using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;
using HCLI.Program.Core.Shared.Feedback.Expecteds;

namespace HCLI.Program.Core.Commands
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

        public IFeedback Execute(UserInput userInput)
        {
            Runtime.SessionData.Running = false;
            return new ExpectedInput();
        }
    }
}