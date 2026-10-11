using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core.Shared.Feedback.Expecteds
{
    public class ExpectedInput : IFeedback
    {
        public FeedbackType ResultType { get; } = FeedbackType.Expected;
        public string FeedbackName { get; } = "Expected.ExpectedInput";
        public string? DebugMessage { get; }
        public string? ConsoleOutput { get; }
        public ConsoleOutputStyle ConsoleOutputStyle { get; private set; } = new();

        public ExpectedInput() { } // <- ennek az összes referenciáját, callját módosítani kell hogy a konstr. keresztül adja az console outputot 
        public ExpectedInput(string? consoleOutput, string? debugMessage = null)
        {
            DebugMessage = debugMessage;
            ConsoleOutput = consoleOutput;
        }
    }
}
