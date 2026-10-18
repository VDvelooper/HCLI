using HCLI.Program.Core.Abstractions;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.Core.Commands
{
    internal class CMD_Exit : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }
        public ModuleBase? ParentModule { get; }

        public CMD_Exit(string syntax, int requiredArgCount, int optionalArgCount, string description, ModuleBase? parentModule = null)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ParentModule = parentModule;
        }

        public IFeedback? Run(UserInput userInput)
        {
            Runtime.SessionData.Running = false;
            return null;
        }
    }
}