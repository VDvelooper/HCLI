using HCLI.Program.Core.Shared.Feedback.Expecteds;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.Abstractions
{
    public interface ICommand
    {
        abstract string Syntax { get; }
        abstract int RequiredArgCount { get; }
        abstract int OptionalArgCount { get; }
        abstract string Description { get; }
        ModuleBase? ParentModule { get; }

        /// <summary>
        /// Contains the command's main logic. It is the command's body. Usually it's called in the command entry.
        /// </summary>
        /// <param name="sender">The Module that called the method.</param>
        /// <returns>If the command executed successfuly then return null or a IFeedback.</returns>
        abstract IFeedback? Run(UserInput userInput);

        /// <summary>
        /// Entry point of the command. Calls the command body and returns the success of the command execution.
        /// </summary>
        /// <returns>The command body's execution success.</returns>
        IFeedback Execute(UserInput userInput)
        { 
            return Run(userInput) ?? new ExpectedInput(); 
        }
    }
}
