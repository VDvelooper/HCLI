using HCLI.Program.ModuleCore.Abstractions;
using HCLI.Program.ModuleCore.Shared;
using System.Timers;
using System.Media;

namespace HCLI.Modules
{
    public class TimeIsCurrencyModule : ModuleBase, IModule
    {
        public TimeIsCurrencyModule(string Name, string ModuleCommand, bool SeparateMode) : base(Name, ModuleCommand, SeparateMode)
        {
            Name = "ModuleExample";
            ModuleCommand = "#moduleexample";

            SubCommands.Add("test", args => ExampleModuleCommand(args));
        }


        public void Execute(ModuleModeUserInput userInput) { }


        // --  Module commands  -- //


        private void ExampleModuleCommand(ModuleModeUserInput userInput)
        {
            Console.WriteLine($"This is a test command! Called from {userInput.ModuleCommand}");
        }
    }
}
