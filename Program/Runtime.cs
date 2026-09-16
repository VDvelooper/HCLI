using HCLI.Program.Core.Configuration;

namespace HCLI.Program
{
    public class Runtime
    {
        public static CommandManager CommandManager { get; private set; }
        public static ConfigManager ConfigManager { get; private set; }
        public static Session.SessionData SessionData { get; private set; }

        public static ModuleRegistry ModuleRegistry { get; private set; }



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

                Core.Shared.UserInput currentInput = new Core.Shared.UserInput(userInput);

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
            SessionData = new Session.SessionData();

            ModuleRegistry = new ModuleRegistry();


            SessionData.Running = true;
        }
    }
}