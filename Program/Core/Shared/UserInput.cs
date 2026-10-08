using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.Core.Shared
{
    public class UserInput
    {
        public string Raw;
        public string Command;
        public List<string> RequiredArgs;
        public List<string> OptionalArgs;
        public List<ParsedFlag> DetectedFlags { get; set; }

        public bool IsFlagged { get { return DetectedFlags.Count > 0; } }

        public UserInput(string command, List<string> requiredArgs, List<string> optionalArgs, List<ParsedFlag> flags, string raw) 
        {
            Raw = raw;
            Command = command;
            RequiredArgs = requiredArgs;
            OptionalArgs = optionalArgs;
            DetectedFlags = flags;
        }

        public bool TryGetFlag(FlagID id, out ParsedFlag? found)
        {
            found = DetectedFlags.Where(x => x.FlagID == id).FirstOrDefault();
            return DetectedFlags.Where(x => x.FlagID == id).Any();
        }
    }
}
