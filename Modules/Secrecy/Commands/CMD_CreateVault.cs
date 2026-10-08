using HCLI.Program.Core.Shared;
using System.Text.Json;

namespace HCLI.Modules.Secrecy.Commands
{
    internal class CMD_CreateVault : Program.Core.Abstractions.ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }

        public CMD_CreateVault(string syntax, int requiredArgCount, int optionalArgCount, string description)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
        }

        public void Execute(UserInput userInput)
        {
            string savePath = "";

            if (userInput.RequiredArgs.Count == 0 || !Path.Exists(userInput.RequiredArgs[0]))
            {
                Console.Write($"The path doesn't exists or you didn't set it.\n\n");

                return;

                /*Console.Write($"Do you wish to use the default path ({SecrecyModule.MAIN_DIRECTORY_PATH})? (Y/N)");
                Console.Write("\nSecrecy > \n");
                string anwser = Console.ReadLine();

                while (anwser.ToLower() != "y" && anwser.ToLower() != "n")
                {
                    Console.Write($"Unknown anwser! Do you wish to use the default path ({SecrecyModule.MAIN_DIRECTORY_PATH})? (Y/N)");
                    Console.Write("\nSecrecy > \n");
                    anwser = Console.ReadLine();
                }

                if (anwser.ToLower() == "y") savePath = SecrecyModule.MAIN_DIRECTORY_PATH;*/
            }

            /*if (savePath == "")
                savePath = userInput.RequiredArgs[0];*/
            savePath = userInput.RequiredArgs[0];

            // Setting the vault's default settings.

            Console.Write($"Give the vault's owner's name: ");
            Console.Write("\nSecrecy > ");
            string ownerName = Console.ReadLine();

            Console.Write($"Set a master password for the vault. You will need this to access your vault.");
            Console.Write("\nSecrecy > ");
            string masterKey = Console.ReadLine();

            Console.Write($"Write your master password.");
            Console.Write("\nSecrecy > ");
            string masterKeyTest = Console.ReadLine();

            if (masterKeyTest != masterKey)
            {
                Console.Write($"Incorrect master password! Do you wish to create a new one? (Y/N)");
                Console.Write("\nSecrecy > ");
                string anwser = Console.ReadLine();

                while (anwser.ToLower() != "y" && anwser.ToLower() != "n")
                {
                    Console.Write($"Unknown anwser! Do you wish to create a new one? (Y/N)");
                    Console.Write("\nSecrecy > ");
                    anwser = Console.ReadLine();
                }

                if (anwser.ToLower() == "y")
                {
                    Console.Write($"Set a new master password for the vault. YOU WON'T BE ABLE TO REWRITE IT ANYMORE");
                    Console.Write("\nSecrecy > ");
                    masterKey = Console.ReadLine();
                }
            }


            Vault newVault = new Vault(ownerName, masterKey);

            string json = JsonSerializer.Serialize(newVault);
            File.WriteAllText(@$"{savePath}\vault.json", json);

            Console.Write($"\nVault has been created under the ownership's name: {ownerName}.\n");
        }
    }
}
