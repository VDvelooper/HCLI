using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core.Commands
{
    public class CMD_Help : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }

        public CMD_Help(string syntax, int requiredArgCount, int optionalArgCount, string description)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
        }

        public void Execute(UserInput userInput) 
        {
            Console.WriteLine($"\nexit -> Exits HCLI.\n" +
             $"echo -> Outputs the text followed by the 'echo' keyword. [1th arg: the text itself]\n" +
             $"clear -> Clears the console. [0 arguments]\n" +
             $"help -> Writes out all the base commands. [0 arguments]\n" +
             $"lam -> Writes out all the avalible modules. [0 arguments]\n");
        }
    }
}
