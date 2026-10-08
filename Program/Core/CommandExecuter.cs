using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core
{
    public class CommandExecuter
    {
        public static void ExecuteCommand(UserInput? userInput)
        {
            if (userInput == null) return;
            Console.WriteLine($"command: {userInput.Command}, reqargs: {userInput.RequiredArgs.Count}");
            
            if (Runtime.CommandRegistry.TryGetHCLICommand(userInput.Command, out var foundHCLICommand))
            {
                Console.WriteLine("1");
                if (foundHCLICommand != null) { foundHCLICommand.Execute(userInput); return; }
                else
                    Console.WriteLine("valami szar...");
            }
            else if (Runtime.ModuleRegistry.TryGetModuleCommand(userInput, out var foundModuleCommand) && foundModuleCommand != null)
            {
                Console.WriteLine("2");
                foundModuleCommand.Execute(userInput);
                return;
            }

            Console.WriteLine("3");//zzz -> valamiért az echo kommandnál csak ez fut le...

            if (userInput.TryGetFlag(FlagID.Debug, out _))
                Console.WriteLine(
                    $"Debug" +
                    $"Full command: {userInput.Raw}"
                );
        }
    }
}
