using System.Text.Json;

namespace HCLI.Program.Core.Configuration
{
    public class ConfigManager
    {
        public ConfigData ConfigData { get; private set; }

        public string CLI_USER_INPUT_PREFIX = "";

        public ConfigManager()
        {
            ConfigData = new ConfigData();
            ConfigData.MainDirectoryPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\HCLI";
            ConfigData.ConfigAbsolutePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\HCLI\config.json";
        }


        public void InintialSetUp()
        {
            if (!Path.Exists(ConfigData.ConfigAbsolutePath))
            {
                if (!Path.Exists(ConfigData.MainDirectoryPath))
                {
                    Console.Write($"\nThe installation is not set up yet. The setup will be at this location: {ConfigData.MainDirectoryPath}");
                    Directory.CreateDirectory(ConfigData.MainDirectoryPath);
                }

                // getting default information

                Console.Write("\nSet your username: ");
                Console.Write($"\nSecrecy > ");
                string userName = Console.ReadLine();

                Console.WriteLine($"\nSetup complete {userName}! You can find the config file here: {ConfigData.ConfigAbsolutePath}\n\n");

                CreateConfigFile(userName, ConfigData.MainDirectoryPath, null);
            }
            

            ConfigData.SetupComplete = true;
        }

        public void LoadConfig()
        {
            if (!Path.Exists(ConfigData.ConfigAbsolutePath))
            {
                InintialSetUp();
                return;
            }

            string fileContent = File.ReadAllText(ConfigData.ConfigAbsolutePath);
            ConfigData = JsonSerializer.Deserialize<ConfigData>(fileContent);
        }

        public void SaveConfig()
        {
            if (!Path.Exists(ConfigData.ConfigAbsolutePath)) return;

            string file = JsonSerializer.Serialize(ConfigData);
            File.WriteAllText(ConfigData.ConfigAbsolutePath, file);
        }




        private void CreateConfigFile(string userName, string? mainDirectoryPath, List<string>? vaultPaths)
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