using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;

namespace HCLI.Program.ModuleCore.Shared
{
    public class ModuleBase
    {
        public string ModuleName { get; private set; }
        public string ModuleCommand { get; private set; }
        public Dictionary<string, Action<UserInput>> BaseCommands { get; set; }
        public List<ICommand> SubCommands { get; private set; }


        public static string MODULE_DATA_DIR_PATH = @$"{Runtime.ConfigManager.ConfigData.MainDirectoryPath}\ModuleData";

        // Instance

        protected bool IsSeparateMode;
        protected bool IsSeparateModeRunning;

        public ModuleBase(string moduleName, string moduleCommand, bool separateMode)
        {
            IsSeparateMode = separateMode;
            IsSeparateModeRunning = false;

            ModuleName = moduleName;
            ModuleCommand = moduleCommand;

            SubCommands = new List<ICommand>();
            BaseCommands = new Dictionary<string, Action<UserInput>>()
            {
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

            if (IsSeparateMode)
            {
                BaseCommands.Add(
                    this.ModuleCommand,
                    args => EnterModuleCommand(args)
                );
            }

            Console.WriteLine($"MODULE_DATA_DIR_PATH:{MODULE_DATA_DIR_PATH}");

            if (!Path.Exists(MODULE_DATA_DIR_PATH))
                Directory.CreateDirectory(MODULE_DATA_DIR_PATH);
            
        }

        public virtual void ModuleExecute(UserInput userInput) { }

        public void SubCommandParser(UserInput userInput)
        {
            string subCommand = "";

            if (BaseCommands.ContainsKey(userInput.Command))
            {
                BaseCommands[userInput.Command](userInput);
            }
            else
            {
                Console.WriteLine($"{ModuleName}> The module command '{userInput.Command}' is unknown.\n");
                return;
            }
        }

        private void EnterModuleCommand(UserInput _)
        {
            if (!IsSeparateMode) return;
            if (IsSeparateModeRunning) return;

            IsSeparateModeRunning = true;
        }
        private void ExitModuleCommand(UserInput _)
        {
            if (!IsSeparateMode) return;
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
            Console.WriteLine($"Module data directory: {MODULE_DATA_DIR_PATH}");
        }
    }
}
