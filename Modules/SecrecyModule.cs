using HCLI.Modules.Secrecy;
using HCLI.Program.ModuleCore.Shared;
using HCLI.Program.ModuleCore.Abstractions;
using System.Text.Json;

namespace HCLI.Modules
{
    public class SecrecyModule : ModuleBase, IModule
    {

        public bool SETUP_COMPLETE = false;
        public string USERNAME = "";

        public string MAIN_DIRECTORY_PATH = $@"{MODULE_DATA_DIR_PATH}\Secrecy";
        public string CONFIG_ABSOLUTE_PATH = @$"{MODULE_DATA_DIR_PATH}\Secrecy\config.hclidata";
        public string USERS_ABSOLUTE_PATH = @$"{MODULE_DATA_DIR_PATH}\Secrecy\Data\users.hclidata";

        public SecrecyModule(string Name, string ModuleCommand, bool separateMode) : base(
            Name,
            ModuleCommand,
            separateMode
        )
        {
            Name = "Secrecy";
            ModuleCommand = "secrecy";

            SubCommands.Add("cv", args => this.CreateSinglePathVault(args));
            SubCommands.Add("setup", args => this.InintialSetUp());
            SubCommands.Add("test", args => TestCommand(args));
        }


        public void Execute(ModuleModeUserInput currentInput)
        {
            if (!TryLoadConfig())
                InintialSetUp();

            ManageAccount();

            SubCommandParser(currentInput);

            while (_separateModeRunning)
            {
                Console.Write("Secrecy > ");
                string userInput = Console.ReadLine();

                ModuleModeUserInput newInput = new ModuleModeUserInput(userInput);

                SubCommandParser(newInput);
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
                    Console.Write($"\nWelcome to Secrecy! It seems like you haven't set up the module yet!");
                    Console.Write($"\nThe setup will be at this location: {MAIN_DIRECTORY_PATH}");
                    Directory.CreateDirectory(MAIN_DIRECTORY_PATH);
                    Directory.CreateDirectory($"{MAIN_DIRECTORY_PATH}\\Data");
                }

                // getting default information

                if (!Path.Exists(USERS_ABSOLUTE_PATH) || File.ReadAllText(USERS_ABSOLUTE_PATH) == "")
                {
                    //zzz
                }

                Console.Write("\nSet your username: ");
                Console.Write($"\nSecrecy > ");
                string userName = Console.ReadLine();

                Console.WriteLine($"\nSetup complete {userName}! You can find the config file here: {CONFIG_ABSOLUTE_PATH}\n\n");

                CONFIG_ABSOLUTE_PATH = CreateConfigFile(userName, MAIN_DIRECTORY_PATH, null);
                USERNAME = userName;
            }

            SETUP_COMPLETE = true;
        }

        private bool TryLoadConfig()
        {
            if (!Path.Exists(CONFIG_ABSOLUTE_PATH)) return false;

            string json = File.ReadAllText(CONFIG_ABSOLUTE_PATH);
            Secrecy.ConfigData configData = JsonSerializer.Deserialize<Secrecy.ConfigData>(json);

            SETUP_COMPLETE = configData.SetupComplete;
            CONFIG_ABSOLUTE_PATH = configData.configAbsolutePath;
            MAIN_DIRECTORY_PATH = configData.mainDirectoryPath;
            USERNAME = configData.Username;
            return true;
        }

        private void SaveConfig()
        {
            if (!Path.Exists(CONFIG_ABSOLUTE_PATH)) return;

            string json = File.ReadAllText(CONFIG_ABSOLUTE_PATH);
            Secrecy.ConfigData configData = JsonSerializer.Deserialize<Secrecy.ConfigData>(json);


            configData.SetupComplete = SETUP_COMPLETE;
            configData.configAbsolutePath = CONFIG_ABSOLUTE_PATH;
            configData.mainDirectoryPath = MAIN_DIRECTORY_PATH;
            configData.Username = USERNAME;


            json = JsonSerializer.Serialize(configData);
            File.WriteAllText(CONFIG_ABSOLUTE_PATH, json);
        }

        private void ManageAccount()
        {
            UserData userData;

            if (!Path.Exists(USERS_ABSOLUTE_PATH))
            {
                userData = new UserData();

                CreateUser(ref userData);
                
                string fileContent = JsonSerializer.Serialize<UserData>(userData);

                File.WriteAllText(USERS_ABSOLUTE_PATH, fileContent);
            }
            else
            {
                string fileContent = File.ReadAllText(USERS_ABSOLUTE_PATH);
                userData = JsonSerializer.Deserialize<UserData>(fileContent);

                if (fileContent != "")
                {
                    Console.WriteLine("Log-in or create a new account. Commands: login / new\n");

                    string inputAnwser = Console.ReadLine();
                    while (inputAnwser != "login" || inputAnwser != "new")
                    {
                        Console.WriteLine("Unknown command. Try again.");
                        Console.Write("Secrecy > ");
                        inputAnwser = Console.ReadLine();
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

            Console.WriteLine("Existing accounts:\n");

            foreach (KeyValuePair<string, string> pair in userData.UserDataDictionary)
            {
                Console.WriteLine(pair.Key);
            }


            Console.WriteLine("\nWrite the username to proceed to logining into that account:");
            Console.Write("Secrecy > ");


            string userInput = Console.ReadLine();
            while (!userData.UserDataDictionary.ContainsKey(userInput) || userInput != "new")
            {
                Console.WriteLine("That's not an existing account. Try again.");
                Console.WriteLine("If you wish to create a new account then type \'new\'.");
                Console.Write("Secrecy > ");

                userInput = Console.ReadLine();
            }

            if (userInput == "new") CreateUser(ref userData);
            else 
            {
                string username = userInput;

                Console.WriteLine("\nPassword:");
                Console.Write("Secrecy > ");
                userInput = Console.ReadLine();

                // password check
                while (userInput != userData.UserDataDictionary[username])
                {
                    Console.WriteLine("\nWrong password. Try again:");
                    Console.Write("Secrecy > ");
                    userInput = Console.ReadLine();
                }

                _loggedInUserName = username;

                Console.WriteLine($"Welcome back {username}!");
            }
        }

        private void CreateUser(ref UserData userData)
        {
            Console.WriteLine("Set your username:");
            Console.Write("Secrecy > ");
            string username = Console.ReadLine();

            while (userData.UserDataDictionary.ContainsKey(username))
            {
                Console.WriteLine("Set your username:");
                Console.Write("Secrecy > ");
                username = Console.ReadLine();
            }

            Console.WriteLine("Set your password:");
            Console.Write("Secrecy > ");
            string password = Console.ReadLine();

            userData.AddUser(username, password);

            _loggedInUserName = username;
        }

        /// <summary>
        /// Submethod. Used for the initial creation of the config file.
        /// </summary>
        /// <returns>The config file's path as a string. Used to set CONFIG_PATH.</returns>
        private string CreateConfigFile(string userName, string? mainDirectoryPath, List<string>? vaultPaths)
        {
            Secrecy.ConfigData configData = new Secrecy.ConfigData();

            configData.Username = userName;
            configData.mainDirectoryPath = mainDirectoryPath;
            configData.configAbsolutePath = @$"{MAIN_DIRECTORY_PATH}\config.hclidata";
            configData.SetupComplete = true;
            configData.VaultPaths = vaultPaths;

            string json = JsonSerializer.Serialize(configData);
            string configAbsolutePath = configData.configAbsolutePath;

            CONFIG_ABSOLUTE_PATH = configData.configAbsolutePath;

            File.WriteAllText(configAbsolutePath, json);
            return configAbsolutePath;
        }

        public void CreateSinglePathVault(ModuleModeUserInput userInput)
        {
            List<string> convertedArgs = userInput.Args.ToList();

            if (convertedArgs.Count == 0 || !Path.Exists(convertedArgs[0]))
            {
                Console.Write($"The path doesn't exists or you didn't set it.\n\n");

                Console.Write($"Do you wish to use the default path ({MAIN_DIRECTORY_PATH})? (Y/N)");
                Console.Write("\nSecrecy > \n");
                string anwser = Console.ReadLine();

                while (anwser.ToLower() != "y" && anwser.ToLower() != "n")
                {
                    Console.Write($"Unknown anwser! Do you wish to use the default path ({MAIN_DIRECTORY_PATH})? (Y/N)");
                    Console.Write("\nSecrecy > \n");
                    anwser = Console.ReadLine();
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


            Secrecy.Vault newVault = new Secrecy.Vault(ownerName, masterKey);

            string json = JsonSerializer.Serialize(newVault);
            File.WriteAllText(@$"{savePath}\vault.json", json);

            Console.Write($"\nVault has been created under the ownership's name: {ownerName}.\n");
        }

        private void TestCommand(ModuleModeUserInput _)
        {
            Console.WriteLine("\nSecrecy > Test success!\n");
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
            this.OwnerName = ownerName;
            this.MasterKey = masterKey;

            this.Items = new List<VaultItem>();
        }

    }
}
