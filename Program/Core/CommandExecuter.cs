using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core
{
    public class CommandExecuter
    {
        public static void ExecuteCommand(UserInput? userInput)
        {
            if (userInput == null) return;

            if (Runtime.CommandRegistry.TryGetHCLICommand(userInput.Command, out var foundHCLICommand))
            {
                if (foundHCLICommand != null)
                    foundHCLICommand.Execute(userInput);
                else
                    throw new ArgumentNullException($"{userInput.Command} was not found as a HCLI command.");
            }
            else if (Runtime.ModuleRegistry.TryGetModuleCommand(userInput.Command, out var foundModuleCommand) && foundModuleCommand != null)
            {
                if (foundModuleCommand != null)
                    foundModuleCommand.Execute(userInput);
                else
                    throw new ArgumentNullException($"{userInput.Command} was not found as a module command.");
            }
            // this means, that if it is a base command for example: exit or help
            else if (Runtime.ModuleRegistry.TryGetBaseCommand(userInput.Command, out Action<UserInput>? found))
            {
                if (found != null) 
                    found.Invoke(userInput);
                else
                    throw new ArgumentNullException($"{userInput.Command} was not found as a base command.");
            }
            else
                Console.WriteLine($"Command {userInput.Command} is unknown.");

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

