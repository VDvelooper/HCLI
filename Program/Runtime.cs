using System.Diagnostics;
using HCLI.Program.Abstractions;
using HCLI.Program.Core;
using HCLI.Program.Core.Configuration;

namespace HCLI.Program
{
    internal class Runtime
    {
        // public services
        internal static CommandRegistry CommandRegistry { get; private set; }
        internal static Session.SessionData SessionData { get; private set; }
        internal static ModuleRegistry ModuleRegistry { get; private set; }
        internal static ConfigManager ConfigManager { get; private set; }
        internal static Services Services { get; private set; }

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
            var CommandRegistry = new CommandRegistry();
            var SessionData = new Session.SessionData();
            var ConfigManager = new ConfigManager();
            var ModuleRegistry = new ModuleRegistry();

            Services = new Services();

            SessionData.Running = true;
        }
    }
}