using HCLI.Program.Core;
using HCLI.Program.Core.Abstractions;
using HCLI.Program.ModuleCore.Shared;
using static HCLI.Program.ModuleCore.Shared.ModuleBase;

namespace HCLI.Program.ModuleCore.Commands
{
    internal class CMD_ModuleExecute : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }

        private ModuleExecuteon ModuleExecuteMethod;

        public CMD_ModuleExecute(string syntax, int requiredArgCount, int optionalArgCount, string description, ModuleExecuteon moduleExecuteMethod)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ModuleExecuteMethod = moduleExecuteMethod;
        }


        public IFeedback? Run(UserInput userInput, ModuleBase? sender)
        {
            return ModuleExecuteMethod.Invoke(userInput, sender);
        }
    }
}
