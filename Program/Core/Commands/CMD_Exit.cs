using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core.Commands
{
    public class CMD_Exit : ICommand
    {
        public string Syntax { get; }
        public string Description { get; }

        public CMD_Exit(string syntax, string description)
        {
            Syntax = syntax;
            Description = description;
        }

        public void Execute(UserInput userInput)
        {
            Runtime.SessionData.Running = false;
        }
    }
}