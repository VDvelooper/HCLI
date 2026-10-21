using HCLI.Program.Abstractions;
using HCLI.Program.Core;
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


        public Services Services { get; private set; }
        public string ModuleDllStoragePath // this needs to be centered in one class.
                                           // there is one more of this type of field in ModuleRegistry.cs
        {
            get { return @$"{ConfigManager.ConfigData.MainDirectoryPath}\ModuleData"; }
        }



        private bool _isSeparateMode;
        private bool _isSeparateModeRunning;

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

        

        
        public ModuleBase(string moduleName, string moduleCommand, bool separateMode, Services services)
        {
            Services = services;
            ModuleName = moduleName;
            ModuleCommand = moduleCommand;

            SubCommands = new List<ICommand>();


            _isSeparateMode = separateMode;
            IsSeparateModeRunning = false;


            CMD_Exit cmd_exit = new CMD_Exit("exit", 0, 0, "Exits the Module if it is a seperate program like module.", this);
            CMD_Help cmd_help = new CMD_Help("help", 0, 1, "Writes out all avalible commands in the current module.", this);
            CMD_ModuleExecute cmd_moduleExecute = new CMD_ModuleExecute(moduleCommand, 0, int.MaxValue, "Writes out all avalible commands in the current module.", this);

            BaseCommands = new List<ICommand>()
            {
                cmd_exit,
                cmd_help,
                cmd_moduleExecute
            };

            if (!Path.Exists(ModuleDllStoragePath))
                Directory.CreateDirectory(ModuleDllStoragePath);
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
            ConsoleView.Message($"Module data directory: {ModuleDllStoragePath}");
        }
    }
}
