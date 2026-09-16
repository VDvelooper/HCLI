namespace HCLI.Program.ModuleCore.Shared
{
    public class ModuleBase
    {
        public string ModuleName { get; protected set; }
        public string ModuleCommand { get; protected set; }
        public Dictionary<string, Action<ModuleModeUserInput>> SubCommands { get; set; }

        public static string MODULE_DATA_DIR_PATH = @$"{Runtime.ConfigManager.ConfigData.MainDirectoryPath}\ModuleData";

        // Instance

        protected bool _separateMode;
        protected bool _separateModeRunning;

        public ModuleBase(string Name, string ModuleCommand, bool separateMode)
        {
            ModuleName = Name;
            this.ModuleCommand = ModuleCommand;
            _separateMode = separateMode;
            _separateModeRunning = false;

            SubCommands = new()
            {
                {
                    "exit",
                    args => ExitModuleCommand(args)
                },
                {
                    this.ModuleCommand,
                    args => EnterModuleCommand(args)
                },
                {
                    "help",
                    args => HelpModuleCommand(args)
                }
            };

            Console.WriteLine($"MODULE_DATA_DIR_PATH:{MODULE_DATA_DIR_PATH}");

            if (!Path.Exists(MODULE_DATA_DIR_PATH))
                Directory.CreateDirectory(MODULE_DATA_DIR_PATH);
        }


        public void SubCommandParser(ModuleModeUserInput userInput)
        {
            string subCommand = "";

            if (SubCommands.ContainsKey(userInput.ModuleCommand))
            {
                SubCommands[userInput.ModuleCommand](userInput);
            }
            else
            {
                Console.WriteLine($"{ModuleName}> The module command '{userInput.ModuleCommand}' is unknown.\n");
                return;
            }
        }

        private void EnterModuleCommand(ModuleModeUserInput userInput)
        {
            if (!_separateMode) return;
            if (_separateModeRunning) return;

            _separateModeRunning = true;
        }
        private void ExitModuleCommand(ModuleModeUserInput userInput)
        {
            if (!_separateMode) return;
            if (!_separateModeRunning || userInput.Args.Count > 0) return;

            _separateModeRunning = false;
        }
        private void HelpModuleCommand(ModuleModeUserInput userInput)
        {
            List<string> commandList = new List<string>();

            foreach (KeyValuePair<string, Action<ModuleModeUserInput>> pair in SubCommands)
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
    }
}
