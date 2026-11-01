using ModuleContracts.Abstractions;
using ModuleContracts.Feedback;
using ModuleContracts.Feedback.Error;

namespace HCLI.Program.Core.Commands
{
    internal class CMD_Exit : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }
        public ModuleBase? ParentModule { get; }

        public CMD_Exit(string syntax, int requiredArgCount, int optionalArgCount, string description, object senderForVerification, ModuleBase? parentModule = null)
        {
            if (senderForVerification is not Program.CommandRegistry)
            {
                var ex = new InternalValidationExeption("Program.Core.Commands.CMD_Exit class was unauthorized to be instanciated.", null, true);
                Runtime.ConsoleView.PresentFeedback(ex);
                Runtime.ConsoleView.FatalError(ex.Description!);
                return;
            }

            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ParentModule = parentModule;
        }

        public IFeedback? Run(UserInput userInput)
        {
            Runtime.ValidateExitCommand(this);
            return null;
        }
    }
}