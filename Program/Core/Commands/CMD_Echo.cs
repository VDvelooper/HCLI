using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;
using HCLI.Program.Core.Shared.Feedback.Errors;
using HCLI.Program.Core.Shared.Feedback.Expecteds;
using HCLI.Program.Core.Shared.Feedback.Warning;

namespace HCLI.Program.Core.Commands
{
    public class CMD_Echo : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }

        public CMD_Echo(string syntax, int requiredArgCount, int optionalArgCount, string description)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
        }

        public IFeedback Execute(UserInput userInput)
        {
            return new ExpectedInput(userInput.RequiredArgs[0]);
        }
    }
}
