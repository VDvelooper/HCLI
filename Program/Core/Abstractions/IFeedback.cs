using HCLI.Program.Core.Shared.Feedback;

namespace HCLI.Program.Core.Abstractions
{
    public interface IFeedback
    {
        public FeedbackType ResultType { get; }
        public string FeedbackID { get; }
        public string? Description { get; }
        public bool Silent { get; }
        public string? ConsoleOutputOverride { get; }
        public ConsoleOutputStyle ConsoleOutputStyle { get; }
    }
}
