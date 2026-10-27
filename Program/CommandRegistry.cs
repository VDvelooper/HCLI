using HCLI.Program.Core.Commands;
using ModuleContracts.Abstractions;

namespace HCLI.Program
{
    internal class CommandRegistry
    {
        internal Dictionary<string, ICommand> Commands;

        internal CommandRegistry()
        {
            Core.Commands.CMD_Exit cmd_exit = new("exit", 0, 0, "Exits HCLI.", this);
            Core.Commands.CMD_Help cmd_help = new("help", 0, 1, "Writes out all the base commands.");
            Core.Commands.CMD_Echo cmd_echo = new("echo", 1, 0, "Outputs the text followed by the 'echo' keyword.");
            Core.Commands.CMD_Clear cmd_clear = new("clear", 0, 0, "Clears the console.");
            Core.Commands.CMD_ListLoadedModules cmd_llm = new("llm", 0, 0, "Lists out all the loaded modules.");
            Core.Commands.CMD_ListDetectedModules cmd_ldm = new("ldm", 0, 0, "Lists out all the detected .dll file formats, what can be loaded in as modules.");
            Core.Commands.CMD_LoadModule cmd_loadModule = new("loadmodule", 1, 0, "Loads the given module. The module is specified by it's name.");

            Commands = new()
            {
                { "exit", cmd_exit },
                { "help", cmd_help },
                { "echo", cmd_echo },
                { "clear", cmd_clear },
                { "llm", cmd_llm },
                { "ldm", cmd_ldm },
                { "loadmodule", cmd_loadModule }
            };
        }

        internal bool TryGetHCLICommand(string command, out ICommand? found)
        {
            found = Commands.Where(x=>x.Value.Syntax == command).Select(x => x.Value).FirstOrDefault();
            return found != null;
        }

        /// <summary>
        /// Search which includes only Module subcommands.
        /// </summary>
        /// <returns>Base command or Module command</returns>
        internal bool TryGetModuleSubCommand(string command, out ICommand? found)
        {
            ModuleRegistry mr = Runtime.ModuleRegistry;

            mr.TryGetModuleSubCommand(command, out found);

            return found != null;
        }

        /// <summary>
        /// Search which includes Base Commands and Module subcommands.
        /// </summary>
        /// <returns>Base command or Module's subcommand</returns>
        internal bool TryGetModuleCommand(string command, out ICommand? found)
        {
            ModuleRegistry mr = Runtime.ModuleRegistry;

            mr.TryGetBaseCommand(command, out found);
            if (found == null)
                mr.TryGetModuleSubCommand(command, out found); 

            return found != null;
        }
    }
}
