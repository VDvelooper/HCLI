using System.Diagnostics;

using HCLI.Program.Core;
using HCLI.Program.Core.Configuration;

using ModuleContracts;
using ModuleContracts.Abstractions;
using ModuleContracts.Feedback.Error;

namespace HCLI.Program
{
    internal sealed class Runtime
    {
        private static void Main(string[] args)
        {
            IsRunning = true;

            InstanciateCore();

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

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


        private static void CommandLoop()
        {
            if (!ConfigManager.ConfigData.SetupComplete)
                ConsoleView.Message($">>Welcome to HCLI!<< \n Made by Danikaz64 \n");
            else
                ConsoleView.Message($"Welcome back {ConfigManager.ConfigData.UserName}!\n");


            while (IsRunning)
            {
                string? rawInput = ConsoleView.ReadUserInput("HCLI");

                UserInput? userInput = CommandParser.Parse(rawInput);

                CommandExecuter.ExecuteCommand(userInput);
            }
        }

        private static void InstanciateCore()
        {
            CommandParser = new CommandParser();
            CommandExecuter = new CommandExecuter();
            CommandRegistry = new CommandRegistry();
            ConfigManager = new ConfigManager();
            ModuleRegistry = new ModuleRegistry();
            ConsoleView = new ConsoleView();

            ContractsForModules = new Contracts(ConsoleView, CommandExecuter, ConfigManager, CommandParser);
        }

        internal static void ValidateExitCommand(object sender) // weak ass validation. => bárki megadhat bármilyen object-et amihez hozzáfér
        {
            if (sender is not Program.Core.Commands.CMD_Exit)
            {
                var ex = new InternalValidationExeption("Exit command's sender was not Program.Core.Commands.CMD_Exit.", null, true);
                ConsoleView.PresentFeedback(ex);
                ConsoleView.FatalError(ex.Description!);

                return;
            }
            IsRunning = false;
        }



        internal static bool IsRunning = true;

        
        internal static ConsoleView ConsoleView { get; private set; } = new ConsoleView();


        internal static CommandParser CommandParser { get; private set; } = new CommandParser();
        internal static CommandExecuter CommandExecuter { get; private set; } = new CommandExecuter();
        internal static CommandRegistry CommandRegistry { get; private set; } = new CommandRegistry();


        internal static ConfigManager ConfigManager { get; private set; } = new ConfigManager();
        internal static ModuleRegistry ModuleRegistry { get; private set; } = null!;


        internal static Contracts ContractsForModules { get; private set; } = null!;
    }
}