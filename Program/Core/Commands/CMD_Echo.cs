using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core.Commands
{
    public class CMD_Echo : ICommand
    {
        public string Syntax { get; }
        public string Description { get; }

        public CMD_Echo(string syntax, string description)
        {
            Syntax = syntax;
            Description = description;
        }

        public void Execute(UserInput userInput)
        {
            string restoredString = "";

            foreach (string part in userInput.Args)
            {
                restoredString += $"{part} ";
            }



            if (!userInput.DetectedFlags.Debug)
            {
                Console.WriteLine($"{restoredString}\n");
            }
            else
            {
                userInput.DetectedFlags.DebugPrintFlags();
                Console.WriteLine($"\n{restoredString}\n");
            }
        }
    }
}
