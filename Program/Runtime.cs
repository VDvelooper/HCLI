using System.Diagnostics;
using HCLI.Program.Core;
using HCLI.Program.Core.Configuration;

namespace HCLI.Program
{
    public class Runtime
    {
        // public services
        public static CommandRegistry CommandRegistry { get; private set; }

        public static ConfigManager ConfigManager { get; private set; }
        public static Session.SessionData SessionData { get; private set; }

        public static ModuleRegistry ModuleRegistry { get; private set; }


        // non-public services
        private static CommandParser CommandParser { get; } = new CommandParser();

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            SetDefault();

            ConfigManager.LoadConfig();
            CommandLoop();
            ConfigManager.SaveConfig();
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            Debug.WriteLine("Saving config");
            ConfigManager.SaveConfig();
            Debug.WriteLine("Savinged config");
        }


        public static void CommandLoop()
        {
            if (!ConfigManager.ConfigData.SetupComplete)
            {
                Console.WriteLine($">>Welcome to HCLI!<< \n Made by Danikaz64 \n");
            }
            else
            {
                Console.WriteLine($"Welcome back {ConfigManager.ConfigData.UserName}!\n");
            }


            while (SessionData.Running)
            {
                Console.Write("HCLI > ");
                string userInput = Console.ReadLine();

                Core.Shared.UserInput currentInput = CommandParser.Parse(userInput);
                CommandRegistry.CommandExecuter(currentInput); // <- külön class?
            }
        }


        /// <summary>
        /// A simple submethod to collapse things.
        /// </summary>
        private static void SetDefault()
        {
            CommandRegistry = new CommandRegistry();
            ConfigManager = new ConfigManager();
            SessionData = new Session.SessionData();

            ModuleRegistry = new ModuleRegistry();


            SessionData.Running = true;
        }
    }
}