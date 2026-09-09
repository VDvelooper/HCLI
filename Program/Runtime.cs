namespace HCLI.Program
{
    public class Runtime
    {
        public static CommandManager CommandManager { get; private set; }
        public static ConfigManager ConfigManager { get; private set; }
        public static SessionData SessionData { get; private set; }

        public static ModuleManager ModuleManager { get; private set; }



        static void Main(string[] args)
        {
            SetDefault();

            ConfigManager.LoadConfig();
            CommandLoop();
            ConfigManager.SaveConfig();
        }

        
        public static void CommandLoop()
        {
            if (!ConfigManager.ConfigData.SetupComplete)
            {
                Console.WriteLine($">>Welcome to HCLI!<<\n" +
                $"Made by Danikaz64\n\n" +
                $">>Some info<<\n" +
                $"  - The saving side of the application only works if you exit with the exit command.\n" +
                $"------------------------------------------------------------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine($"Welcome back {ConfigManager.ConfigData.UserName}!\n\n" +
                $">>Some info<<\n" +
                $"  - The saving side of the application only works if you exit with the exit command.\n" +
                $"------------------------------------------------------------------------------------------------------------------------");
            }


            while (SessionData.Running)
            {
                Console.Write("HCLI > ");
                string userInput = Console.ReadLine();

                Base.UserInput currentInput = new Base.UserInput(userInput);

                CommandManager.CommandParser(currentInput);
            }
        }


        /// <summary>
        /// A simple submethod to collapse things.
        /// </summary>
        private static void SetDefault()
        {
            CommandManager = new CommandManager();
            ConfigManager = new ConfigManager();
            SessionData = new SessionData();

            ModuleManager = new ModuleManager();


            SessionData.Running = true;
        }
    }

    public class SessionData
    {
        public bool Running = false;
        public Base.ModuleData RunningModule;
    }

}

namespace HCLI.Program.Base
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

        public UserModuleModeInput ToModuleModeInput()
        {
            UserModuleModeInput converted = new UserModuleModeInput(this.Raw);
            return converted;
        }
    }


    public class UserModuleModeInput
    {
        public string Raw;
        private List<string> _splitted;

        public string ModuleCommand;
        public List<string> Args;

        public UserModuleModeInput(string rawInput)
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
            UserInput converted = new UserInput(this.Raw);
            return converted;
        }
    }
}