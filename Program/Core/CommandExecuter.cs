using ModuleContracts.Abstractions;
using ModuleContracts.Feedback;
using ModuleContracts.ServiceContracts;

namespace HCLI.Program.Core
{
    public class CommandExecuter : ICommandExecuterContract
    {
        public void ExecuteCommand(UserInput? userInput)
        {
            if (userInput == null) return;

            IFeedback feedback;

            feedback = userInput.Command.Execute(userInput);

            Runtime.ConsoleView.PresentFeedback(feedback);


            Runtime.ConsoleView.Message();
            

            if (userInput.TryGetFlag(FlagID.Debug, out _))
            {
                Runtime.ContractsForModules.ConsoleViewContract.Debug(
                    $"Debug details:\n" +
                    $"Full command: {userInput.Raw}\n" +
                    $"Lexed command: {userInput.Command}\n" +
                    $"Required args count: {userInput.RequiredArgs.Count}\n" +
                    $"Optional args count: {userInput.OptionalArgs.Count}"
                );
                Runtime.ContractsForModules.ConsoleViewContract.Debug("Found flags:");
                userInput.DetectedFlags.ForEach(flag => Runtime.ContractsForModules.ConsoleViewContract.Debug($"  {flag.FlagDefinition.Name}"));

                Runtime.ConsoleView.Message();
            }            
        }
    }
}

