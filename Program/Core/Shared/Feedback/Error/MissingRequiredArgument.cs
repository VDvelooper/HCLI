using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core.Shared.Feedback.Errors
{
    public class MissingRequiredArgument : IFeedback
    {
        public FeedbackType ResultType { get; } = FeedbackType.Error;
        public string FeedbackName { get; } = "Error.MissingRequiredArgument";
        public string? DebugMessage { get; }
        public string? ConsoleOutput { get; }
        public ConsoleOutputStyle ConsoleOutputStyle { get; private set; } = new(ConsoleColor.DarkRed, ConsoleColor.Black);

        public MissingRequiredArgument() { }
        public MissingRequiredArgument(string? debugMessage)
        {
            DebugMessage = debugMessage;
        }
        public MissingRequiredArgument(string? consoleOutput, string? debugMessage = null)
        {
            DebugMessage = debugMessage;
            ConsoleOutput = consoleOutput;
        }
    }
}