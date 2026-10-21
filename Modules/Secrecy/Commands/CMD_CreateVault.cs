using HCLI.Program;
using HCLI.Program.Abstractions;
using HCLI.Program.Core;
using HCLI.Program.Core.Shared.Feedback.Error;
using HCLI.Program.Core.Shared.Feedback.Errors;
using HCLI.Program.ModuleCore.Shared;
using System.Text.Json;

namespace HCLI.Modules.Secrecy.Commands
{
    internal class CMD_CreateVault : ICommand
    {
        public string Syntax { get; }
        public int RequiredArgCount { get; }
        public int OptionalArgCount { get; }
        public string Description { get; }
        public ModuleBase ParentModule { get; }

        public CMD_CreateVault(string syntax, int requiredArgCount, int optionalArgCount, string description, ModuleBase parentModule)
        {
            Syntax = syntax;
            RequiredArgCount = requiredArgCount;
            OptionalArgCount = optionalArgCount;
            Description = description;
            ParentModule = parentModule;
        }


        public IFeedback? Run(UserInput userInput)
        {
            string savePath = "";
            if (userInput.RequiredArgs.Count == 0)
                return new MissingRequiredArgument();

            if (!Path.Exists(userInput.RequiredArgs[0]))
            {
                Console.Write($"The path doesn't exists or you didn't set it.\n\n");

                return new FilePathNotFound();
            }

            savePath = userInput.RequiredArgs[0];

            // Setting the vault's default settings.

            ConsoleView.Message($"Give the vault's owner's name: ");
            string ownerName = ConsoleView.ReadUserInput("Secrecy");

            ConsoleView.Message($"Set a master password for the vault. You will need this to access your vault.");
            string masterKey = ConsoleView.ReadUserInput("Secrecy");

            ConsoleView.Message($"Write your master password.");
            string masterKeyTest = ConsoleView.ReadUserInput("Secrecy");

            if (masterKeyTest != masterKey)
            {
                Runtime.Services.ConsoleView.PresentFeedback(new UnkownAnwser("Incorrect master password."));
                Runtime.Services.ConsoleView.Message($"Do you wish to create a new one? (Y/N)");
                string anwser = Runtime.Services.ConsoleView.ReadUserInput("Secrecy");

                while (anwser.ToLower() != "y" && anwser.ToLower() != "n")
                {
                    CRuntime.Services.ConsoleView.PresentFeedback(new UnkownAnwser("Unknown anwser. Possible anwsers: Y/N"));
                    ConsoleView.Message($"Do you wish to create a new one? (Y/N)");
                    anwser = ConsoleView.ReadUserInput("Secrecy");
                }

                if (anwser.ToLower() == "y")
                {
                    ConsoleView.Message($"Set a new master password for the vault. YOU WON'T BE ABLE TO REWRITE IT ANYMORE");
                    masterKey = anwser = ConsoleView.ReadUserInput("Secrecy");
                }
            }


            Vault newVault = new Vault(ownerName, masterKey);

            string json = JsonSerializer.Serialize(newVault);
            File.WriteAllText(@$"{savePath}\vault.json", json);

            ConsoleView.Message($"\nVault has been created under the ownership's name: {ownerName}.");

            return null;
        }
    }
}
