using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core.Shared.Feedback.Expecteds
{
    public class ExpectedInput : IFeedback
    {
        public FeedbackType ResultType { get; } = FeedbackType.Expected;
        public string FeedbackID { get; } = "Expected.ExpectedInput";
        public string? Description { get; }
        public bool Silent { get; } = false;
        public string? ConsoleOutputOverride { get; }
        public ConsoleOutputStyle ConsoleOutputStyle { get; private set; } = new();

        public ExpectedInput() { } // <- ennek az összes referenciáját, callját módosítani kell hogy a konstr. keresztül adja az console outputot 
        public ExpectedInput(string? consoleOutput, string? description = null, bool silent = false)
        {
            Description = description;
            ConsoleOutputOverride = consoleOutput;
            Silent = silent;
        }
    }
}
