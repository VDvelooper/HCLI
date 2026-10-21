using HCLI.Program.Abstractions;
using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core
{
    public class CommandExecuter
    {
        public void ExecuteCommand(UserInput? userInput)
        {
            if (userInput == null) return;

            IFeedback feedback;

            feedback = userInput.Command.Execute(userInput);

            Runtime.Services.ConsoleView.PresentFeedback(feedback);

            if (userInput.TryGetFlag(FlagID.Debug, out _))
            {
                ConsoleView.Message(
                    $"Debug details:\n" +
                    $"Full command: {userInput.Raw}\n" +
                    $"Lexed command: {userInput.Command}\n" +
                    $"Required args count: {userInput.RequiredArgs.Count}\n" +
                    $"Optional args count: {userInput.OptionalArgs.Count}"
                );
                ConsoleView.Message("Found flags:");
                userInput.DetectedFlags.ForEach(flag => ConsoleView.Message($"  {flag.FlagDefinition.Name}"));
                ConsoleView.Message();
            }
        }
    }
}

