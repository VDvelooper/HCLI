using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.ModuleCore.Abstractions
{
    public interface IModule
    {
        public void Execute(ModuleModeUserInput userInput);
    }
}
