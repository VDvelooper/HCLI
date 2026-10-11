using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;
using HCLI.Program.Core.Shared.Feedback.Expecteds;
using HCLI.Program.ModuleCore;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.Core.Commands
{
    public class CMD_ListAvalibleModules : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }

        public CMD_ListAvalibleModules(string syntax, int requiredArgCount, int optionalArgCount, string description)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
        }

        public IFeedback Execute(UserInput userInput)
        {
            Console.WriteLine();
            foreach (ModuleBase module in Runtime.ModuleRegistry.AVALIBLE_MODULES)
            {
                Console.WriteLine(module.ModuleName);
            }
            Console.WriteLine();
            return new ExpectedInput();
        }
    }
}