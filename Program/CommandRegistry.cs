using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Commands;

namespace HCLI.Program
{
    public class CommandRegistry
    {
        public static Dictionary<string, ICommand> Commands;

        public CommandRegistry()
        {
            CMD_Exit cmd_exit = new("exit", 0, 0, "Exits HCLI.");
            CMD_Help cmd_help = new("help", 0, 1, "Writes out all the base commands.");
            CMD_Echo cmd_echo = new("echo", 1, 0, "Outputs the text followed by the 'echo' keyword.");
            CMD_Clear cmd_clear = new("clear", 0, 0, "Clears the console.");
            CMD_ListAvalibleModules cmd_lam = new("lam", 0, 0, "Writes out all the avalible modules.");

            Commands = new()
            {
                { "exit", cmd_exit },
                { "help", cmd_help },
                { "echo", cmd_echo },
                { "clear", cmd_clear },
                { "lam", cmd_lam }
            };
        }

        public static bool TryGetHCLICommand(string command, out Program.Core.Abstractions.ICommand? found)
        {
            found = Commands.Where(x=>x.Value.Syntax == command).Select(x => x.Value).FirstOrDefault();
            return found != null;
        }

        /// <summary>
        /// Search which includes only Module subcommands.
        /// </summary>
        /// <returns>Base command or Module command</returns>
        public static bool TryGetModuleSubCommand(string command, out ICommand? found)
        {
            ModuleRegistry mr = Runtime.ModuleRegistry;

            mr.TryGetModuleSubCommand(command, out found);

            return found != null;

        }

        /// <summary>
        /// Search which includes Base Commands and Module subcommands.
        /// </summary>
        /// <returns>Base command or Module's subcommand</returns>
        public static bool TryGetModuleCommand(string command, out ICommand? found)
        {
            ModuleRegistry mr = Runtime.ModuleRegistry;

            mr.TryGetBaseCommand(command, out found);
            if (found == null)
                mr.TryGetModuleSubCommand(command, out found); 

            return found != null;
        }
        
        public static bool IsThereEnoughArguments(List<string> args, int expectedArgsCount)
        {
            return args.Count == expectedArgsCount - 1;
        }
    }
}
