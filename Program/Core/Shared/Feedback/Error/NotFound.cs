using HCLI.Program.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCLI.Program.Core.Shared.Feedback.Error
{
    internal class NotFound : IFeedback
    {
        public FeedbackType ResultType { get; } = FeedbackType.Error;
        public string FeedbackID { get; } = "Error.NotFound";
        public string? Description { get; }
        public bool Silent { get; } = false;
        public string? ConsoleOutputOverride { get; }
        public ConsoleOutputStyle ConsoleOutputStyle { get; private set; } = new(ConsoleColor.DarkRed, ConsoleColor.Black);

        public NotFound() { }
        public NotFound(string? description = null, string? consoleOutputOverride = null, bool silent = false)
        {
            Description = description;
            ConsoleOutputOverride = consoleOutputOverride;
            Silent = silent;
        }
    }
}
