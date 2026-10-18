using HCLI.Program.Core;
using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Configuration;
using HCLI.Program.Core.Shared.Feedback.Expecteds;
using HCLI.Program.ModuleCore.Commands;

namespace HCLI.Program.ModuleCore.Shared
{
    public abstract class ModuleBase
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
            /*set { _isSeparateMode = value; }*/ // később problémás lehet hogy nem internal vagy más
        }
        public bool IsSeparateModeRunning
        {
            get { return _isSeparateModeRunning; }
            set { _isSeparateModeRunning = value; } // később problémás lehet hogy nem internal vagy más
        }

        
        public static string MODULE_DATA_DIR_PATH = @$"{ConfigManager.ConfigData.MainDirectoryPath}\ModuleData";

        
        public ModuleBase(string moduleName, string moduleCommand, bool separateMode)
        {
            _isSeparateMode = separateMode;
            IsSeparateModeRunning = false;

            ModuleName = moduleName;
            ModuleCommand = moduleCommand;

            SubCommands = new List<ICommand>();

            CMD_Exit cmd_exit = new CMD_Exit("exit", 0, 0, "Exits the Module if it is a seperate program like module.", this);
            CMD_Help cmd_help = new CMD_Help("help", 0, 1, "Writes out all avalible commands in the current module.", this);
            CMD_ModuleExecute cmd_moduleExecute = new CMD_ModuleExecute(moduleCommand, 0, int.MaxValue, "Writes out all avalible commands in the current module.", this);

            BaseCommands = new List<ICommand>()
            {
                cmd_exit,
                cmd_help,
                cmd_moduleExecute
            };

            if (!Path.Exists(MODULE_DATA_DIR_PATH))
                Directory.CreateDirectory(MODULE_DATA_DIR_PATH);
        }

        /// <summary>
        /// The manager of the Module. Calls the core logic and manages Separate Program Module functionality.
        /// </summary>
        public IFeedback ModuleExecute(UserInput userInput)
        {
            if (IsSeparateMode) Entry();

            if (IsSeparateMode) IsSeparateModeRunning = true;

            IFeedback? feedback = null;

            feedback = ModuleBody(userInput);

            while (IsSeparateModeRunning)
            {
                feedback = ModuleBody(userInput);
            }

            if (IsSeparateMode) Exit();

            return feedback ?? new ExpectedInput();
        }

        /// <summary>
        /// The very first thing that happenes when the ModuleExecute method is called.
        /// </summary>
        public virtual void Entry() { }
        /// <summary>
        /// The very last thing that happenes when the ModuleExecute method is at the end.
        /// </summary>
        public virtual void Exit() { }

        /// <summary>
        /// The the body of a Module. The Module's entry calls it. 
        /// </summary>
        /// <returns></returns>
        public abstract IFeedback? ModuleBody(UserInput userInput);

        private void PrintDirectory(UserInput _)
        {
            ConsoleView.Message($"Module data directory: {MODULE_DATA_DIR_PATH}");
        }
    }
}
