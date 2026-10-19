using HCLI.Program.Core;

namespace HCLI.Program.Abstractions
{
    public interface IModule
    {
        public CommandParser CommandParser { get; }
    }
}
