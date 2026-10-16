using System.Text.Json;

namespace HCLI.Program.Core.Configuration
{
    internal class ConfigManager
    {
        public static ConfigData ConfigData { get; private set; }

        public string CLI_USER_INPUT_PREFIX = "";

        public ConfigManager()
        {
            ConfigData = new ConfigData();
            ConfigData.MainDirectoryPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\HCLI";
            ConfigData.ConfigAbsolutePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\HCLI\config.json";
        }


        public static void LoadConfig()
        {
            if (!Path.Exists(ConfigData.ConfigAbsolutePath))
            {
                InintialSetUp();
                return;
            }

            string fileContent = File.ReadAllText(ConfigData.ConfigAbsolutePath);
            ConfigData = JsonSerializer.Deserialize<ConfigData>(fileContent);
        }

        public static void SaveConfig()
        {
            if (!Path.Exists(ConfigData.ConfigAbsolutePath)) return;

            string file = JsonSerializer.Serialize(ConfigData);
            File.WriteAllText(ConfigData.ConfigAbsolutePath, file);
        }



        private static void InintialSetUp()
        {
            if (!Path.Exists(ConfigData.ConfigAbsolutePath))
            {
                if (!Path.Exists(ConfigData.MainDirectoryPath))
                {
                    ConsoleView.Message($"\nThe installation is not set up yet. The setup will be at this location: {ConfigData.MainDirectoryPath}");
                    Directory.CreateDirectory(ConfigData.MainDirectoryPath);
                }

                // getting default information

                ConsoleView.Message("Set your username: ");
                string userName = ConsoleView.ReadUserInput("Secrecy");

                ConsoleView.Message($"Setup complete {userName}! You can find the config file here: {ConfigData.ConfigAbsolutePath}\n");

                CreateConfigFile(userName, ConfigData.MainDirectoryPath, null);
            }
            

            ConfigData.SetupComplete = true;
        }

        private static void CreateConfigFile(string userName, string? mainDirectoryPath, List<string>? vaultPaths)
        {
            //ConfigData = new Base.ConfigData(); // problem was only to set using at top to HCLI.Base. Look at the bottom of this script where they are set.

            ConfigData.UserName = userName;
            ConfigData.SetupComplete = true;


            string fileContent = JsonSerializer.Serialize(ConfigData);

            File.WriteAllText(ConfigData.ConfigAbsolutePath, fileContent);
            ConfigData = ConfigData;
        }
    }
}