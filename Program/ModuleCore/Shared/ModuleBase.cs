using HCLI.Program.Core;
using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.ModuleCore.Shared
{
    public class    ModuleBase
    {
        public string ModuleName { get; private set; }
        public string ModuleCommand { get; private set; }
        public Dictionary<string, Action<UserInput>> BaseCommands { get; private set; }
        public List<ICommand> SubCommands { get; private set; }

        private bool _isSeparateMode;
        protected bool _isSeparateModeRunning;
        public bool IsSeparateMode
        {
            get { return _isSeparateMode; }
            internal set { _isSeparateMode = value; }
        }
        public bool IsSeparateModeRunning
        {
            get { return _isSeparateModeRunning; }
            protected set { _isSeparateModeRunning = value; }
        }

        public static string MODULE_DATA_DIR_PATH = @$"{Runtime.ConfigManager.ConfigData.MainDirectoryPath}\ModuleData";

        
        public ModuleBase(string moduleName, string moduleCommand, bool separateMode)
        {
            _isSeparateMode = separateMode;
            IsSeparateModeRunning = false;

            ModuleName = moduleName;
            ModuleCommand = moduleCommand;

            SubCommands = new List<ICommand>();

            BaseCommands = new Dictionary<string, Action<UserInput>>()
            {
                {
                    this.ModuleCommand,
                    args => ModuleExecute(args)
                },
                {
                    "exit",
                    args => ExitModuleCommand(args)
                },
                {
                    "help",
                    args => HelpModuleCommand(args)
                },
                {
                    "printdir",
                    args => PrintDirectory(args)
                }
            };

            if (!Path.Exists(MODULE_DATA_DIR_PATH))
                Directory.CreateDirectory(MODULE_DATA_DIR_PATH);
        }

        public virtual void ModuleExecute(UserInput userInput) { }

        private void ExitModuleCommand(UserInput _)
        {
            if (!_isSeparateMode) return;
            if (!IsSeparateModeRunning) return;

            IsSeparateModeRunning = false;
        }

        private void HelpModuleCommand(UserInput _)
        {
            List<string> commandList = new List<string>();

            foreach (KeyValuePair<string, Action<UserInput>> pair in BaseCommands)
            {
                commandList.Add(pair.Key);
            }

            Console.WriteLine();

            foreach (string printout in commandList)
            {
                Console.WriteLine(printout);
            }

            Console.WriteLine();
        }

        private void PrintDirectory(UserInput _)
        {
            ConsoleView.Message($"Module data directory: {MODULE_DATA_DIR_PATH}");
        }
    }
}
