using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program.Core.Shared
{
    public class UserInput
    {
        public string Raw;
        private List<string> _splitted;

        public string Command;
        public List<string> Args;

        public UserInput(string rawInput)
        {
            Raw = rawInput;
            _splitted = rawInput.Split(' ').ToList();

            Command = _splitted[0];

            Args = new List<string>();
            Args.AddRange(_splitted);
            Args.Remove(Command); // we remove the command part of the input
        }

        public ModuleModeUserInput ToModuleModeInput()
        {
            ModuleModeUserInput converted = new ModuleModeUserInput(this.Raw);
            return converted;
        }
    }
}
