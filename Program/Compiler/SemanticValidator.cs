using HCLI.Program.Core.ConsoleComm;
using ModuleContracts.Abstractions;
using ModuleContracts.Feedback.Error;
using ModuleContracts.Feedback.Warning;

namespace HCLI.Program.Compiler
{
    internal class SemanticValidator
    {
        private ConsoleView _consoleView;

        internal SemanticValidator() 
        {
            _consoleView = Runtime.ConsoleView;
        }


        /// <returns>Wheter the validation was successful or not.</returns>
        internal bool Validate(UserInput userInput)
        {

            if (userInput.Command == null)
            {
                Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new UnknownCommand($"The command {userInput.Raw} is uknown."));
                return false;
            }

            if (userInput.RequiredArgs.Count != userInput.RequiredArgsCount)
            {
                _consoleView.PresentFeedback(new MissingRequiredArgument());
                return false;
            }

            /*if (userInput.OptionalArgs.Count > userInput.OptionalArgsCount)  ---- ez lehet nem kell egyáltalán.
            {
                Runtime.ConsoleView.PresentFeedback(new UnusedOptionalArgument());
            }*/ 

            return true;

        }

    }
}
