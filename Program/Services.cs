using HCLI.Program.Core;
using HCLI.Program.Core.Configuration;

namespace HCLI.Program
{
    public class Services
    {
        public ConsoleView ConsoleView { get; }
        public CommandExecuter CommandExecuter { get; }
        public CommandParser CommandParser { get; }
        public ModuleRegistry ModuleRegistry { get; }

        internal Services(ConsoleView cv, CommandExecuter ce, CommandParser cp, ModuleRegistry mr) 
        { 
            ConsoleView = cv;
            CommandExecuter = ce;
            CommandParser = cp;
            ModuleRegistry = mr;
        } 
    }
}
