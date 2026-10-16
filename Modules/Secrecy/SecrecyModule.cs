using HCLI.Modules.Secrecy.Commands;
using HCLI.Program.Core;
using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared.Feedback.Error;
using HCLI.Program.ModuleCore.Shared;
using System.Text.Json;

namespace HCLI.Modules.Secrecy
{
    public class SecrecyModule : ModuleBase
    {
        public bool SETUP_COMPLETE = false;
        public string USERNAME = "";

        public static string MAIN_DIRECTORY_PATH = $@"{MODULE_DATA_DIR_PATH}\Secrecy";
        public static string CONFIG_ABSOLUTE_PATH = @$"{MODULE_DATA_DIR_PATH}\Secrecy\config.hclidata";
        public static string USERS_ABSOLUTE_PATH = @$"{MODULE_DATA_DIR_PATH}\Secrecy\Data\users.hclidata";

        public SecrecyModule(bool isSeparateMode) : base("Secrecy", "secrecy", isSeparateMode)
        {

            // new good way *1
            CMD_CreateVault cmd_createVault = new CMD_CreateVault("cv", 1, 0, "Creates a new password vault on the file path that is given in parenthesies.");


            SubCommands.Add(cmd_createVault);
        }


        public override void ModuleExecute(UserInput userInput)
        {
            IsSeparateModeRunning = true;

            if (!TryLoadConfig())
                InintialSetUp();

            ManageAccount();

            while (IsSeparateModeRunning)
            {
                string newRawInput = ConsoleView.ReadUserInput(ModuleName);

                UserInput? newUserInput = CommandParser.Parse(newRawInput, false);

                CommandExecuter.ExecuteCommand(userInput);
            }

            SaveConfig();
        }


        // -- Commands -- //

        private void InintialSetUp()
        {
            if (!Path.Exists(CONFIG_ABSOLUTE_PATH))
            {

                if (!Path.Exists(MAIN_DIRECTORY_PATH))
                {
                    ConsoleView.Message($"\nWelcome to Secrecy! It seems like you haven't set up the module yet!", false);
                    ConsoleView.Message($"The setup will be at this location: {MAIN_DIRECTORY_PATH}", false);
                    Directory.CreateDirectory(MAIN_DIRECTORY_PATH);
                    Directory.CreateDirectory($"{MAIN_DIRECTORY_PATH}\\Data");
                }

                // getting default information

                if (!Path.Exists(USERS_ABSOLUTE_PATH) || File.ReadAllText(USERS_ABSOLUTE_PATH) == "")
                {
                    //zzz
                }

                ConsoleView.Message("\nSet your username: ", false);
                string userName = ConsoleView.ReadUserInput(ModuleName);

                ConsoleView.Message($"\nSetup complete {userName}! You can find the config file here: {CONFIG_ABSOLUTE_PATH}\n", true);

                CONFIG_ABSOLUTE_PATH = CreateConfigFile(userName, MAIN_DIRECTORY_PATH, new List<string>());
                USERNAME = userName;
            }

            SETUP_COMPLETE = true;
        }

        private bool TryLoadConfig()
        {
            if (!Path.Exists(CONFIG_ABSOLUTE_PATH)) return false;

            string json = File.ReadAllText(CONFIG_ABSOLUTE_PATH);
            ConfigData? configData = JsonSerializer.Deserialize<ConfigData>(json);

            if (configData == null)
                return false;

            SETUP_COMPLETE = configData.SetupComplete;
            CONFIG_ABSOLUTE_PATH = configData.configAbsolutePath;
            MAIN_DIRECTORY_PATH = configData.mainDirectoryPath;
            USERNAME = configData.Username;
            return true;
        }

        private void SaveConfig()
        {
            if (!Path.Exists(CONFIG_ABSOLUTE_PATH)) 
                return;

            string json = File.ReadAllText(CONFIG_ABSOLUTE_PATH);
            ConfigData? configData = JsonSerializer.Deserialize<ConfigData>(json);

            if (configData == null) 
                return;

            configData.SetupComplete = SETUP_COMPLETE;
            configData.configAbsolutePath = CONFIG_ABSOLUTE_PATH;
            configData.mainDirectoryPath = MAIN_DIRECTORY_PATH;
            configData.Username = USERNAME;


            json = JsonSerializer.Serialize(configData);
            File.WriteAllText(CONFIG_ABSOLUTE_PATH, json);
        }

        private void ManageAccount()
        {
            UserData? userData;

            if (!Path.Exists(USERS_ABSOLUTE_PATH))
            {
                userData = new UserData();

                CreateUser(ref userData);
                
                string fileContent = JsonSerializer.Serialize(userData);

                File.WriteAllText(USERS_ABSOLUTE_PATH, fileContent);
            }
            else
            {
                string fileContent = File.ReadAllText(USERS_ABSOLUTE_PATH);
                userData = JsonSerializer.Deserialize<UserData>(fileContent);

                if (userData == null)
                {
                    ConsoleView.PresentFeedback(new InternalNullExeption($"Null encounter during loading user data. (Secrecy)"));
                    return;
                }

                if (fileContent != "")
                {
                    ConsoleView.Message("Log-in or create a new account. Commands: login / new");

                    string inputAnwser = ConsoleView.ReadUserInput(ModuleName);

                    while (inputAnwser != "login" || inputAnwser != "new")
                    {
                        ConsoleView.PresentFeedback(new UnkownAnwser(null, "Unknown command. Try again."));
                        inputAnwser = ConsoleView.ReadUserInput(ModuleName);
                    }

                    if (inputAnwser == "login")
                    {
                        Login(userData);
                    }
                    else 
                    {
                        CreateUser(ref userData);
                    }
                }
                else 
                {
                    CreateUser(ref userData);
                }
            }
        }

        private void Login(UserData userData)
        {

            ConsoleView.Message("Existing accounts:\n");

            foreach (KeyValuePair<string, string> pair in userData.UserDataDictionary)
            {
                ConsoleView.Message(pair.Key);
            }


            ConsoleView.Message("\nWrite the username to proceed to logining into that account:");
            string userInput = ConsoleView.ReadUserInput(ModuleName);

            while (!userData.UserDataDictionary.ContainsKey(userInput) || userInput != "new")
            {
                ConsoleView.Message("That's not an existing account. Try again.");
                ConsoleView.Message("If you wish to create a new account then type \'new\'.");

                userInput = ConsoleView.ReadUserInput(ModuleName);
            }

            if (userInput == "new") CreateUser(ref userData);
            else 
            {
                string username = userInput;

                ConsoleView.Message("\nPassword:");
                userInput = ConsoleView.ReadUserInput(ModuleName);

                // password check
                while (userInput != userData.UserDataDictionary[username])
                {
                    ConsoleView.Message("\nWrong password. Try again:");
                    userInput = ConsoleView.ReadUserInput(ModuleName);
                }

                _loggedInUserName = username;

                Console.WriteLine($"Welcome back {username}!");
            }
        }

        private void CreateUser(ref UserData userData)
        {
            ConsoleView.Message("Set your username:");
            string username = ConsoleView.ReadUserInput(ModuleName);

            while (userData.UserDataDictionary.ContainsKey(username))
            {
                ConsoleView.Message("Set your username:");
                username = ConsoleView.ReadUserInput(ModuleName);
            }

            ConsoleView.Message("Set your password:");
            string password = ConsoleView.ReadUserInput(ModuleName);

            userData.AddUser(username, password);

            _loggedInUserName = username;
        }

        /// <summary>
        /// Submethod. Used for the initial creation of the config file.
        /// </summary>
        /// <returns>The config file's path as a string. Used to set CONFIG_PATH.</returns>
        private string CreateConfigFile(string userName, string? mainDirectoryPath, List<string>? vaultPaths)
        {
            ConfigData configData = new ConfigData();

            configData.Username = userName;
            if (mainDirectoryPath != null) configData.mainDirectoryPath = mainDirectoryPath;
            configData.configAbsolutePath = @$"{MAIN_DIRECTORY_PATH}\config.hclidata";
            configData.SetupComplete = true;
            configData.VaultPaths = vaultPaths;

            string json = JsonSerializer.Serialize(configData);
            string configAbsolutePath = configData.configAbsolutePath;

            CONFIG_ABSOLUTE_PATH = configData.configAbsolutePath;

            File.WriteAllText(configAbsolutePath, json);
            return configAbsolutePath;
        }

        public void CreateSinglePathVault(UserInput userInput)
        {
            List<string> convertedArgs = userInput.RequiredArgs.ToList();

            if (convertedArgs.Count == 0 || !Path.Exists(convertedArgs[0]))
            {
                ConsoleView.Message($"The path doesn't exists or you didn't set it.\n");

                ConsoleView.Message($"Do you wish to use the default path ({MAIN_DIRECTORY_PATH})? (Y/N)");
                string anwser = ConsoleView.ReadUserInput(ModuleName);

                while (anwser.ToLower() != "y" && anwser.ToLower() != "n")
                {
                    ConsoleView.Message($"Unknown anwser! Do you wish to use the default path ({MAIN_DIRECTORY_PATH})? (Y/N)");
                    anwser = ConsoleView.ReadUserInput(ModuleName);
                }

                if (anwser.ToLower() == "n") return;
            }

            string savePath = "";

            if (convertedArgs.Count == 0)
                savePath = MAIN_DIRECTORY_PATH;
            else
            {
                foreach (string part in convertedArgs)
                {
                    savePath += $"{part}";
                }
            }


            // Setting the vault's default settings.

            ConsoleView.Message($"Give the vault's owner's name: ");
            string ownerName = ConsoleView.ReadUserInput(ModuleName);

            ConsoleView.Message($"Set a master password for the vault. You will need this to access your vault.");
            string masterKey = ConsoleView.ReadUserInput(ModuleName);

            ConsoleView.Message($"Write your master password.");
            string masterKeyTest = ConsoleView.ReadUserInput(ModuleName);

            if (masterKeyTest != masterKey)
            {
                ConsoleView.Message($"Incorrect master password! Do you wish to create a new one? (Y/N)");
                string anwser = ConsoleView.ReadUserInput(ModuleName);

                while (anwser.ToLower() != "y" && anwser.ToLower() != "n")
                {
                    ConsoleView.Message($"Unknown anwser! Do you wish to create a new one? (Y/N)");
                    anwser = ConsoleView.ReadUserInput(ModuleName);
                }

                if (anwser.ToLower() == "y")
                {
                    ConsoleView.Message($"Set a new master password for the vault. YOU WON'T BE ABLE TO REWRITE IT ANYMORE");
                    masterKey = ConsoleView.ReadUserInput(ModuleName);
                }
            }


            Vault newVault = new Vault(ownerName, masterKey);

            string json = JsonSerializer.Serialize(newVault);
            File.WriteAllText(@$"{savePath}\vault.json", json);

            ConsoleView.Message($"\nVault has been created under the ownership's name: {ownerName}.");
        }

        private void TestCommand(UserInput _)
        {
            ConsoleView.Message("\nSecrecy > Test success!\n");
        }

        private string _loggedInUserName;
    }
}

namespace HCLI.Modules.Secrecy
{ 
    // (we don't really need this, because we can store it all in the) <-> might need this actually, because in the main class (secrecy) there only the logic goes. and this is the data of that custom module ?
    public class ModuleConstans // ?????
    {
        
    }

    public class ConfigData
    {
        public string Username { get; set; }
        public List<string>? VaultPaths { get; set; }
        public bool SetupComplete { get; set; }
        public string mainDirectoryPath { get; set; }
        public string configAbsolutePath { get; set; }
    }

    public class UserData
    {
        public Dictionary<string, string> UserDataDictionary;

        public UserData() 
        {
            UserDataDictionary = new Dictionary<string, string>();
        }

        public void AddUser(string Username, string Password)
        {
            while (UserDataDictionary.ContainsKey(Username))
            {
                Console.WriteLine("This username already exists! Please select a different one.");
                Console.Write("\nSecrecy > ");
                Username = Console.ReadLine();

                Console.WriteLine();
            }

            UserDataDictionary.Add(Username, Password);
        }
    }

    public class VaultItem
    {
        public string Label;
        public string Email;
        public string Password;
    }

    public class Vault
    {
        public string OwnerName { get; set; }
        public string MasterKey { get; set; }
        public List<VaultItem> Items { get; set; }

        public int VaultID { get; set; } = -1;

        public Vault(string ownerName, string masterKey)
        {
            OwnerName = ownerName;
            MasterKey = masterKey;

            Items = new List<VaultItem>();
        }

    }
}
