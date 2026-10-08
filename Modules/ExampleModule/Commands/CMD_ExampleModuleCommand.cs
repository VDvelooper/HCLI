using HCLI.Program.Core.Shared;

namespace HCLI.Modules.ExampleModule.Commands
{
    public class CMD_ExampleModuleCommand : Program.Core.Abstractions.ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }

        public CMD_ExampleModuleCommand(string syntax, int requiredArgCount, int optionalArgCount, string description)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
        }

        public void Execute(UserInput userInput)
        {
            Console.WriteLine($"This is a test command! Called from {userInput.Command}");
        }
    }
}
