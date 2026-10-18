using HCLI.Program.Abstractions;
using HCLI.Program.Core.Shared.Feedback.Error;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.ModuleCore.Commands
{
    internal class CMD_ModuleExecute : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }

        public ModuleBase ParentModule { get; }

        public CMD_ModuleExecute(string syntax, int requiredArgCount, int optionalArgCount, string description, ModuleBase parentModule)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ParentModule = parentModule;
        }

        /// <summary>
        /// Calls the parent Module's entry point.
        /// </summary>
        public IFeedback? Run(UserInput userInput)
        {
            if (userInput.OptionalArgs.Count > 0)
            {
                ICommand? subCommand;

                CommandRegistry.TryGetModuleCommand(userInput.OptionalArgs[0], out subCommand);

                if (subCommand != null)
                    return subCommand.Execute(userInput);
            }

            return ParentModule.ModuleExecute(userInput);
        }
    }
}