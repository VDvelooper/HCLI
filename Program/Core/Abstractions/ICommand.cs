using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core.Abstractions
{
    public interface ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }
        public void Execute(UserInput userInput);
    }
}
