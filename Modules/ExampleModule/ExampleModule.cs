using HCLI.Modules.ExampleModule.Commands;
using HCLI.Program.Core.Shared;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Modules.ExampleModule
{
    public class ExampleModule : ModuleBase
    {
        public ExampleModule(bool isSeparateMode) : base("ModuleExample", "moduleexample", isSeparateMode)
        {
            CMD_ExampleModuleCommand cmd_exampleModuleCommand = new("test", 0, 0, "This is an example module command!");

            SubCommands.Add(cmd_exampleModuleCommand);
        }

        public override void ModuleExecute(UserInput userInput) 
        {
            IsSeparateModeRunning = true;

            while (IsSeparateModeRunning)
            {
                Console.Write("ExampleModule > ");
                string newRawInput = Console.ReadLine();

                UserInput? newUserInput = Program.Core.CommandParser.Parse(newRawInput, false);

                if (newUserInput != null) 
                    Program.Core.CommandExecuter.ExecuteCommand(userInput);
            }
        }
    }
}
