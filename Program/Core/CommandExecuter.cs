using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core
{
    public class CommandExecuter
    {
        public static void ExecuteCommand(UserInput? userInput)
        {
            if (userInput == null) return;

            IFeedback feedback;

            feedback = userInput.Command.Execute(userInput);

            ConsoleView.PresentFeedback(feedback);



            if (userInput.TryGetFlag(FlagID.Debug, out _))
            {
                ConsoleView.Debug(
                    $"Debug details:\n" +
                    $"Full command: {userInput.Raw}\n" +
                    $"Lexed command: {userInput.Command}\n" +
                    $"Required args count: {userInput.RequiredArgs.Count}\n" +
                    $"Optional args count: {userInput.OptionalArgs.Count}"
                );
                ConsoleView.Debug("Found flags:", false);
                userInput.DetectedFlags.ForEach(flag => ConsoleView.Debug($"  {flag.FlagDefinition.Name}"));
                ConsoleView.Message();
            }
        }
    }
}

