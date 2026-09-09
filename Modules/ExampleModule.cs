using HCLI.Program.Base;

namespace HCLI.Modules
{
    public class ExampleModule : ModuleBase, IModule
    {


        public ExampleModule(string Name, string ModuleCommand, bool SeparateMode) : base(Name, ModuleCommand, SeparateMode)
        {
            Name = "ModuleExample";
            ModuleCommand = "#moduleexample";

            SubCommands.Add("test", args => ExampleModuleCommand(args));
        }


        public void Execute(UserModuleModeInput userInput) { }


        // --  Module commands  -- //


        private void ExampleModuleCommand(UserModuleModeInput userInput)
        {
            Console.WriteLine($"This is a test command! Called from {userInput.ModuleCommand}");
        }
    }
}
