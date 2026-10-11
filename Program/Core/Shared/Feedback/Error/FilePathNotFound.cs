using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core.Shared.Feedback.Error
{
    public class FilePathNotFound : IFeedback
    {
        public FeedbackType ResultType { get; } = FeedbackType.Error;
        public string FeedbackName { get; } = "Error.FilePathNotFound";
        public string? DebugMessage { get; }
        public string? ConsoleOutput { get; }
        public ConsoleOutputStyle ConsoleOutputStyle { get; private set; } = new(ConsoleColor.DarkRed, ConsoleColor.Black);

        public FilePathNotFound() { }
        public FilePathNotFound(string? debugMessage)
        {
            DebugMessage = debugMessage;
        }
        public FilePathNotFound(string? consoleOutput, string? debugMessage = null)
        {
            DebugMessage = debugMessage;
            ConsoleOutput = consoleOutput;
        }
    }
}
