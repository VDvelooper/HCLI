using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared.Feedback.Expecteds;

namespace HCLI.Program.Core.Commands
{
    internal class CMD_Exit : ICommand
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

        public IFeedback? Run(UserInput userInput)
        {
            Runtime.SessionData.Running = false;
            return null;
        }
    }
}