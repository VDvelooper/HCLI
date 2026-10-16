using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core.Shared.Feedback.Error
{
    public class InternalNullExeption : IFeedback
    {
        public FeedbackType ResultType { get; } = FeedbackType.Error;
        public string FeedbackID { get; } = "Error.InternalNullExeption";
        public string? Description { get; }
        public bool Silent { get; } = false;
        public string? ConsoleOutputOverride { get; }
        public ConsoleOutputStyle ConsoleOutputStyle { get; private set; } = new(ConsoleColor.DarkRed, ConsoleColor.Black);

        public InternalNullExeption() { }
        public InternalNullExeption(string? description = null, string? consoleOutputOverride = null, bool silent = false)
        {
            Description = description;
            ConsoleOutputOverride = consoleOutputOverride;
            Silent = silent;
        }
    }
}
