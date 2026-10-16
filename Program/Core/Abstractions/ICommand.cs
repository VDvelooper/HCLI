using HCLI.Program.Core.Shared.Feedback.Expecteds;

namespace HCLI.Program.Core.Abstractions
{
    public interface ICommand
    {
        abstract string Syntax { get; }
        abstract int RequiredArgCount { get; }
        abstract int OptionalArgCount { get; }
        abstract string Description { get; }
        abstract IFeedback? Run(UserInput userInput);

        IFeedback Execute(UserInput userInput) 
        { 
            return Run(userInput) ?? new ExpectedInput(); 
        }
    }
}
