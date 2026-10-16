using HCLI.Program.Core.Shared.Feedback.Expecteds;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.Core.Abstractions
{
    public interface ICommand
    {
        abstract string Syntax { get; }
        abstract int RequiredArgCount { get; }
        abstract int OptionalArgCount { get; }
        abstract string Description { get; }
        abstract IFeedback? Run(UserInput userInput, ModuleBase? sender = null);

        IFeedback Execute(UserInput userInput)
        { 
            return Run(userInput) ?? new ExpectedInput(); 
        }
    }
}
