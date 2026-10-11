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

            if (Runtime.CommandRegistry.TryGetHCLICommand(userInput.Command, out var foundHCLICommand)
                &&
                foundHCLICommand != null
            )
            {
                feedback = foundHCLICommand.Execute(userInput);
                ConsoleView.ThrowFeedback(feedback);
            }

            else if (Runtime.ModuleRegistry.TryGetModuleCommand(userInput.Command, out var foundModuleCommand) 
                && foundModuleCommand != null
            )
            {
                feedback = foundModuleCommand.Execute(userInput);
                ConsoleView.ThrowFeedback(feedback);
            }

            // this means, that if it is a base command for example: exit or help
            else if (Runtime.ModuleRegistry.TryGetBaseCommand(userInput.Command, out Action<UserInput>? found)
                && found != null
            )
            {
                found.Invoke(userInput);
            }
            else
                ConsoleView.Error($"Command {userInput.Command} is unknown.");

            if (userInput.TryGetFlag(FlagID.Debug, out _))
            {
                Console.WriteLine(
                    $"Debug details:\n" +
                    $"Full command: {userInput.Raw}\n" +
                    $"Lexed command: {userInput.Command}\n" +
                    $"Required args count: {userInput.RequiredArgs.Count}\n" +
                    $"Optional args count: {userInput.OptionalArgs.Count}"
                );
                Console.WriteLine("Found flags:");
                userInput.DetectedFlags.ForEach(flag => Console.WriteLine($"  {flag.FlagDefinition.Name}"));
                Console.WriteLine();
            }
        }
    }
}

