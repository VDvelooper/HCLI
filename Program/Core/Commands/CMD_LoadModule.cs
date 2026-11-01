using ModuleContracts.Abstractions;
using ModuleContracts.Feedback;
using ModuleContracts.Feedback.Error;

namespace HCLI.Program.Core.Commands
{
    internal class CMD_LoadModule : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }
        public ModuleBase? ParentModule { get; }

        public CMD_LoadModule(string syntax, int requiredArgCount, int optionalArgCount, string description, ModuleBase? parentModule = null)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ParentModule = parentModule;
        }

        public IFeedback? Run(UserInput userInput)
        {
            ModuleRegistry mr = Runtime.ModuleRegistry;

            List<Type> types = mr.DetectedModules.Where(dm => dm.Key.Name.ToLower() == userInput.RequiredArgs[0].ToLower()).Select(dm => dm.Key).ToList();

            if (types.Count > 0)
            {
                ModuleBase? created = mr.InstanciateModule(types[0]);

                if (created == null)
                {
                    return new ValueWasNull($"Returned instance of the specified module was null.", null, true);
                }

                Runtime.ContractsForModules.ConsoleViewContract.Message($"The module is now avalible with the command, \"{created.ModuleCommand}\"");
                return null;
            }
            else
            {
                return new NotFound($"No module found with the name of \"{userInput.RequiredArgs[0]}\"", null, true);
            }
        }
    }
}
