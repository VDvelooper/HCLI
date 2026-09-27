using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core.Commands
{
    public class CMD_Clear : ICommand
    {
        public string Syntax { get; }
        public string Description { get; }

        public CMD_Clear(string syntax, string description)
        {
            Syntax = syntax;
            Description = description;
        }

        public void Execute(UserInput userInput)
        {
            Console.Clear();
        }
    }
}
