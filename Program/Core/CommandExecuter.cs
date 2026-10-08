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
                {
                    foundHCLICommand.Execute(userInput);
                }
                else if (Runtime.ModuleRegistry.TryGetModuleCommand(userInput, out var foundModuleCommand) && foundModuleCommand != null)
                {
                    foundModuleCommand.Execute(userInput);
                }

                if (userInput.TryGetFlag(FlagID.Debug, out _))
                {
                    Console.WriteLine(
                        $"Debug details:\n" +
                        $"Full command: {userInput.Raw}\n" +
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
}

