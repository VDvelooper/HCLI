using HCLI.Program.Core.Shared;

namespace HCLI.Program.Abstractions
{
    public class UserInput
    {
        public string Raw;
        public ICommand Command;

        public List<string> RequiredArgs;
        public int RequiredArgsCount;

        public List<string> OptionalArgs;
        public int OptionalArgsCount;
        public List<ParsedFlag> DetectedFlags { get; set; }

        public bool IsFlagged { get { return DetectedFlags.Count > 0; } }
        public bool ContainsRequiredArgs 
        { 
            get {
                if (RequiredArgsCount > 0)
                    return RequiredArgs.Count > 0;
                else return true;
            } 
        }
        public bool ContainsOptionalArgs
        {
            get {
                if (RequiredArgsCount > 0)
                    return OptionalArgs.Count > 0;
                else return true;
            }
        }

        public UserInput(ICommand command, List<string> requiredArgs, int requiredArgsCount, List<string> optionalArgs, int optionalArgsCount, List<ParsedFlag> flags, string raw) 
        {
            Raw = raw;
            Command = command;
            RequiredArgs = requiredArgs;
            RequiredArgsCount = requiredArgsCount;
            OptionalArgs = optionalArgs;
            OptionalArgsCount = optionalArgsCount;
            DetectedFlags = flags;
        }

        public bool TryGetFlag(FlagID id, out ParsedFlag? found)
        {
            found = DetectedFlags.Where(x => x.FlagID == id).FirstOrDefault();
            return DetectedFlags.Where(x => x.FlagID == id).Any();
        }
    }
}
