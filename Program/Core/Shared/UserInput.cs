using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.Core.Shared
{
    public class UserInput
    {
        public string Raw;

        public string Command;
        public List<string> Args;

        public bool IsFlagged { get { return DetectedFlags.Any; } }
        public Flags DetectedFlags { get; set; }
        public List<ParsedFlag> DetectedFlags2 { get; set; }

        public UserInput() 
        { 
            DetectedFlags = new Flags();
        }
        public UserInput(string command, List<string> args, List<ParsedFlag> flags) 
        { 
            Command = command;
            Args = args;
            DetectedFlags2 = flags;
        }

        public UserInput(string rawInput)
        {
            Raw = rawInput;
            List<string> splittedCommand = rawInput.Split(' ').ToList();

            Command = splittedCommand[0];
            Args = splittedCommand;

            Args.Remove(Command);
        }

        public ModuleModeUserInput ToModuleModeInput()
        {
            ModuleModeUserInput converted = new ModuleModeUserInput(this.Raw);
            return converted;
        }
    }
}
