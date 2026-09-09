using HCLI.Program.Base;

namespace HCLI.Program
{
    public class CommandManager
    {

        /// <summary>
        /// All variables related to finding the user given command and it's representitive method.
        /// </summary>
        public Dictionary<string, Action<List<string>>> Commands;

        public CommandManager()
        {
            Commands = new()
            {
                {
                    "exit",
                    args => Exit(args)
                },
                {
                    "echo",
                    args => Echo(args)
                },
                {
                    "help",
                    args => Help(args)
                },
                {
                    "clear",
                    args => Clear(args)
                },
                {
                    "lam",
                    args => ListAvalibleModules(args)
                }
            };
        }


        public void CommandParser(UserInput userInput)
        {
            if (Commands.ContainsKey(userInput.Command))
            {
                Commands[userInput.Command](userInput.Args);
            }
            else if (Runtime.ModuleManager.MODULE_DATABASE.ContainsKey(userInput.Command))
            {
                Runtime.ModuleManager.TryExecutingCommandFromModule(userInput);
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
            Console.WriteLine($"\necho -> Outputs the text followed by the 'echo' keyword. [1th arg: the text itself]\n" +
                $"clear -> Clears the console. [0 arguments]\n" +
                $"help -> Writes out all the base commands. [0 arguments]\n" +
                $"lam -> Writes out all the avalible modules. [0 arguments]\n" +
                $"setup -> Creating the password manager's base. [0 arguments]\n");
        }
        public void Clear(List<string> args)
        {
            Console.Clear();
        }
        public void ListAvalibleModules(List<string> args)
        {
            Console.WriteLine();
            foreach (ModuleData module in Runtime.ModuleManager.AVALIBLE_MODULES)
            {
                Console.WriteLine(module.Name);
            }
            Console.WriteLine();
        }
    }
}
