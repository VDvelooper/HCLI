using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core.Shared.Feedback.Warning
{
    public class UnusedOptionalArgument : IFeedback
    {
        public FeedbackType ResultType { get; } = FeedbackType.Warning;
        public string FeedbackName { get; } = "Warning.UnexpectedOptionalArgument";
        public string? DebugMessage { get; }
        public string? ConsoleOutput { get; }
        public ConsoleOutputStyle ConsoleOutputStyle { get; private set; } = new(ConsoleColor.DarkYellow, ConsoleColor.Black);

        public UnusedOptionalArgument() { }
        public UnusedOptionalArgument(string? debugMessage)
        {
            DebugMessage = debugMessage;
        }
        public UnusedOptionalArgument(string? consoleOutput, string? debugMessage = null)
        {
            DebugMessage = debugMessage;
            ConsoleOutput = consoleOutput;
        }
    }
}
