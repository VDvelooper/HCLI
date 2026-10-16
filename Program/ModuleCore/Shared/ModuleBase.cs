using HCLI.Program.Core;
using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Configuration;
using HCLI.Program.ModuleCore.Commands;

namespace HCLI.Program.ModuleCore.Shared
{
    public class ModuleBase
    {
        public string ModuleName { get; private set; }
        public string ModuleCommand { get; private set; }
        public List<ICommand> BaseCommands { get; private set; }
        public List<ICommand> SubCommands { get; private set; }

        public bool _isSeparateMode;
        protected bool _isSeparateModeRunning;

        public bool IsSeparateMode
        {
            get { return _isSeparateMode; }
            set { _isSeparateMode = value; } // később problémás lehet hogy nem internal vagy más
        }
        public bool IsSeparateModeRunning
        {
            get { return _isSeparateModeRunning; }
            set { _isSeparateModeRunning = value; } // később problémás lehet hogy nem internal vagy más
        }

        public delegate IFeedback? ModuleExecuteon(UserInput userInput, ModuleBase? sender);
        public static string MODULE_DATA_DIR_PATH = @$"{ConfigManager.ConfigData.MainDirectoryPath}\ModuleData";

        
        public ModuleBase(string moduleName, string moduleCommand, bool separateMode)
        {
            _isSeparateMode = separateMode;
            IsSeparateModeRunning = false;

            ModuleName = moduleName;
            ModuleCommand = moduleCommand;

            SubCommands = new List<ICommand>();

            CMD_Exit cmd_exit = new CMD_Exit("exit", 0, 0, "Exits the Module if it is a seperate program like module.");
            CMD_Help cmd_help = new CMD_Help("help", 0, 1, "Writes out all avalible commands in the current module.");
            CMD_ModuleExecute cmd_moduleExecute = new CMD_ModuleExecute(moduleCommand, 0, 1, "Writes out all avalible commands in the current module.");
            // zzz...

            /*BaseCommands = new List<ICommand>()
            {
                CMD_ModuleExecute(args)
                },
                
                {
                    "help",
                    args => HelpModuleCommand(args)
                },
                {
                    "printdir",
                    args => PrintDirectory(args)
                }
            };*/

            if (!Path.Exists(MODULE_DATA_DIR_PATH))
                Directory.CreateDirectory(MODULE_DATA_DIR_PATH);
        }
        public virtual void ModuleExecute(UserInput userInput) { }


        private void HelpModuleCommand(UserInput _)
        {
            List<string> commandList = new List<string>();

            foreach (KeyValuePair<string, Action<UserInput>> pair in BaseCommands)
            {
                commandList.Add(pair.Key);
            }

            ConsoleView.Message();

            foreach (string printout in commandList)
            {
                ConsoleView.Message(printout);
            }

            ConsoleView.Message();
        }

        private void PrintDirectory(UserInput _)
        {
            ConsoleView.Message($"Module data directory: {MODULE_DATA_DIR_PATH}");
        }
    }
}
