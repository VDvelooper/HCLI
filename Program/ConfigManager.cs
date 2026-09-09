using System.Text.Json;

namespace HCLI.Program
{
    public class ConfigManager
    {
        public Base.ConfigData ConfigData { get; private set; }

        public string CLI_USER_INPUT_PREFIX = "";

        public ConfigManager()
        {
            ConfigData = new Base.ConfigData();
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
            ConfigData = JsonSerializer.Deserialize<Base.ConfigData>(fileContent);
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


namespace HCLI.Program.Base // PLEASE rework these
{
    public class ConfigData
    {
        public string UserName { get; set; }
        public bool SetupComplete { get; set; }
        public string MainDirectoryPath { get; set; }
        public string ConfigAbsolutePath { get; set; }
    }

    public class ModuleData
    {
        public string Name { get; private set; }
        public string ModuleCommand { get; private set; }
        public Action<UserModuleModeInput> Execute { get; private set; }

        public ModuleData(string name, string command, Action<UserModuleModeInput> execute)
        {
            Name = name;
            ModuleCommand = command;
            Execute = execute;
        }
    }
}