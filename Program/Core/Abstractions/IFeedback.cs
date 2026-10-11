using HCLI.Program.Core.Shared.Feedback;

namespace HCLI.Program.Core.Abstractions
{
    public interface IFeedback
    {
        public FeedbackType ResultType { get; }
        public string FeedbackName { get; }
        public string? DebugMessage { get; }
        public string? ConsoleOutput { get; }
    }
}
