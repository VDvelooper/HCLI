using HCLI.Program.Core.Shared;

namespace HCLI.Program.ModuleCore.Shared
{
    public class ModuleModeUserInput
    {
        public string Raw;
        private List<string> _splitted;

        public string ModuleCommand;
        public List<string> Args;

        public ModuleModeUserInput(string rawInput)
        {
            Raw = rawInput;
            _splitted = rawInput.Split(' ').ToList();

            ModuleCommand = _splitted[0];

            Args = new List<string>();
            Args.AddRange(_splitted);
            Args.Remove(ModuleCommand); // we remove the command part of the input
        }

        public UserInput ToUserInput()
        {
            UserInput converted = new UserInput(Raw);
            return converted;
        }
    }
}
