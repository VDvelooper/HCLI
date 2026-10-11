using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core.Shared.Feedback.Warning
{
    public class UnusedOptionalArgument : IFeedback
    {
        public FeedbackType ResultType { get; } = FeedbackType.Warning;
        public string FeedbackID { get; } = "Warning.UnexpectedOptionalArgument";
        public string? Description { get; }
        public bool Silent { get; } = false;
        public string? ConsoleOutputOverride { get; }
        public ConsoleOutputStyle ConsoleOutputStyle { get; private set; } = new(ConsoleColor.DarkYellow, ConsoleColor.Black);

        public UnusedOptionalArgument() { }
        public UnusedOptionalArgument(string? description = null, string? consoleOutputOverride = null, bool silent = false)
        {
            Description = description;
            ConsoleOutputOverride = consoleOutputOverride;
            Silent = silent;
        }
    }
}
