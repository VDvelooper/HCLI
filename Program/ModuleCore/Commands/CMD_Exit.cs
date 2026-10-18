using HCLI.Program.Abstractions;
using HCLI.Program.Core.Shared.Feedback.Error;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.ModuleCore.Commands
{
    public class CMD_Exit : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }
        public ModuleBase ParentModule { get; }

        public CMD_Exit(string syntax, int requiredArgCount, int optionalArgCount, string description, ModuleBase parentModule)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ParentModule = parentModule;
        }


        public IFeedback? Run(UserInput userInput)
        {
            if (ParentModule == null) return new NotFound("The sender module is null.");
            if (!ParentModule.IsSeparateMode) return null;
            if (!ParentModule.IsSeparateModeRunning) return null;

            ParentModule.IsSeparateModeRunning = false;

            return null;
        }
    }
}
