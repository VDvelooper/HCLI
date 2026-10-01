using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core.Commands
{
    public class CMD_Echo : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }

        public CMD_Echo(string syntax, int requiredArgCount, int optionalArgCount, string description)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
        }

        public void Execute(UserInput userInput)
        {
            Console.WriteLine(userInput.RequiredArgs[0]);
        }
    }
}
