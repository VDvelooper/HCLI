using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core.Shared.Feedback.Errors
{
    public class UnexpectedOptionalArgument : IFeedback
    {
        public FeedbackType ResultType { get; } = FeedbackType.Error;
        public string FeedbackName { get; } = "Error.UnexpectedOptionalArgument";
        public string? DebugMessage { get; }
        public string? ConsoleOutput { get; }
        public ConsoleOutputStyle ConsoleOutputStyle { get; private set; } = new(ConsoleColor.DarkRed, ConsoleColor.Black);

        public UnexpectedOptionalArgument() { }
        public UnexpectedOptionalArgument(string? debugMessage)
        {
            DebugMessage = debugMessage;
        }
        public UnexpectedOptionalArgument(string? consoleOutput, string? debugMessage = null)
        {
            DebugMessage = debugMessage;
            ConsoleOutput = consoleOutput;
        }
    }
}
