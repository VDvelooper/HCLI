namespace HCLI.Program.Core
{

    // do I need this? -> interface maybe? -> but then I'd need a ton of classes for each command...

    public class Command
    {
        public string Syntax { get; private set; }
        public string Description { get; private set; }
        public Action<List<string>> Function { get; private set; }

        public Command(string syntax, string description, Action<List<string>> function)
        {
            Syntax = syntax;
            Description = description;
            Function = function;
        }
    }
}
