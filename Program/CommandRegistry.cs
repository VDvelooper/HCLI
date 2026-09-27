using HCLI.Program.Core.Shared;
using HCLI.Program.ModuleCore;
using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Commands;

namespace HCLI.Program
{
    public class CommandRegistry
    {

        /// <summary>
        /// All variables related to finding the user given command and it's representitive method.
        /// </summary>
        public Dictionary<string, ICommand> Commands;

        public CommandRegistry()
        {

            CMD_Exit cmd_exit = new("exit", "Exits HCLI.");
            CMD_Help cmd_help = new("help", "Writes out all the base commands.");
            CMD_Echo cmd_echo = new("echo <the message>", "Outputs the text followed by the 'echo' keyword.");
            CMD_Clear cmd_clear = new("clear", "Clears the console.");
            CMD_ListAvalibleModules cmd_lam = new("lam", "Writes out all the avalible modules.");

            Commands = new()
            {
                { "exit", cmd_exit },
                { "help", cmd_help },
                { "echo", cmd_echo },
                { "clear", cmd_clear },
                { "lam", cmd_lam }
            };
        }


        /// <summary>
        /// DEPRICATED
        /// </summary>
        public void CommandExecuter(UserInput userInput)
        {
            if (Commands.ContainsKey(userInput.Command))
            {
                Commands[userInput.Command].Execute(userInput);
            }
            else if (Runtime.ModuleRegistry.MODULE_DATABASE.ContainsKey(userInput.Command))
            {
                Runtime.ModuleRegistry.TryExecutingCommandFromModule(userInput);
            }
            else
            {
                Console.WriteLine($"HCLI > The command '{userInput.Command}' is unknown.\n");
                return;
            }
        }


        public static bool IsThereEnoughArguments(List<string> args, int expectedArgsCount)
        {
            return args.Count == expectedArgsCount - 1;
        }


        // The methods of the base CLI commands defined from here.


        public void Exit(List<string> args)
        {
            Runtime.SessionData.Running = false;
        }
        public void Echo(List<string> args)
        {
            string restoredString = "";

            foreach (string part in args)
            {
                restoredString += $"{part} ";
            }

            Console.WriteLine($"{restoredString}\n");
        }
        public static void Help(List<string> args)
        {
            Console.WriteLine($"\nexit -> Exits HCLI.\n" +
                $"echo -> Outputs the text followed by the 'echo' keyword. [1th arg: the text itself]\n" +
                $"clear -> Clears the console. [0 arguments]\n" +
                $"help -> Writes out all the base commands. [0 arguments]\n" +
                $"lam -> Writes out all the avalible modules. [0 arguments]\n");
        }
        public void Clear(List<string> args)
        {
            Console.Clear();
        }
        public void ListAvalibleModules(List<string> args)
        {
            Console.WriteLine();
            foreach (ModuleData module in Runtime.ModuleRegistry.AVALIBLE_MODULES)
            {
                Console.WriteLine(module.Name);
            }
            Console.WriteLine();
        }
    }
}
