using System.Diagnostics;
using HCLI.Program.Core;
using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Configuration;

namespace HCLI.Program
{
    internal class Runtime
    {
        // public services
        public static CommandRegistry CommandRegistry { get; private set; }
        public static Session.SessionData SessionData { get; private set; }
        public static ModuleRegistry ModuleRegistry { get; private set; }
        public static ConfigManager ConfigManager { get; private set; }

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
                ConsoleView.Message($">>Welcome to HCLI!<< \n Made by Danikaz64 \n");
            }
            else
            {
                ConsoleView.Message($"Welcome back {ConfigManager.ConfigData.UserName}!\n");
            }


            while (SessionData.Running)
            {
                string? rawInput = ConsoleView.ReadUserInput("HCLI");

                UserInput? userInput = CommandParser.Parse(rawInput);

                CommandExecuter.ExecuteCommand(userInput);
            }
        }


        /// <summary>
        /// A simple submethod to collapse things.
        /// </summary>
        private static void SetDefault()
        {
            CommandRegistry = new CommandRegistry();
            SessionData = new Session.SessionData();
            ConfigManager = new ConfigManager();

            ModuleRegistry = new ModuleRegistry();


            SessionData.Running = true;
        }
    }
}