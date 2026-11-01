using ModuleContracts.Abstractions;
using ModuleContracts.Feedback.Error;
using ModuleContracts.ServiceContracts;
using System.Text.Json;

namespace HCLI.Program.Core.Configuration
{
    public class ConfigManager : IConfigManagerContract // re-name/-factor? we are storing paths here for modules, 
    {
        public string MainDirectoryPath { get; }
        public string ConfigAbsolutePath { get; }
        public string CLI_USER_INPUT_PREFIX { get; private set; } = "";

        internal ConfigData ConfigData { get; private set; }

        public ConfigManager()
        {
            ConfigData = new ConfigData();

            MainDirectoryPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\HCLI";
            ConfigAbsolutePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\HCLI\config.json";
        }


        public BaseModuleConfig? LoadBaseModuleConfig(string path, out bool ignoreNullValue)
        {
            ignoreNullValue = false;

            if (!Path.Exists(path))
            {
                BaseModuleConfig emptyConfig = new BaseModuleConfig();

                string newContent = JsonSerializer.Serialize(emptyConfig);
                File.WriteAllText(path, newContent);

                Runtime.ConsoleView.PresentFeedback(new FilePathNotFound("Not found set path while loading a module's base config.", null, true));
                Runtime.ConsoleView.Warn("Oops! It seems like you haven't set up the module's base config file yet, or it has been deleted.\n" +
                    "The config.json file has been created with the required base content. You need to edit the file manually.");
                Runtime.ConsoleView.Warn("Check the documentation for more information on how to set up a module.");

                ignoreNullValue = true;
                return null;
            }

            string fileContent = File.ReadAllText(path);
            BaseModuleConfig? baseModuleConfig = JsonSerializer.Deserialize<BaseModuleConfig>(fileContent);

            if (baseModuleConfig == null)
            {
                Runtime.ConsoleView.PresentFeedback(new InternalNullExeption("Returning base module was null after Json deserialization."));
                return null;
            }

            return baseModuleConfig!;
        }


        // Core //

        internal void LoadConfig()
        {
            if (!Path.Exists(ConfigAbsolutePath))
            {
                InintialSetUp();
                return;
            }

            string fileContent = File.ReadAllText(ConfigAbsolutePath);
            ConfigData = JsonSerializer.Deserialize<ConfigData>(fileContent);
        }
        internal void SaveConfig()
        {
            if (!Path.Exists(ConfigAbsolutePath)) return;

            string file = JsonSerializer.Serialize(ConfigData);
            File.WriteAllText(ConfigAbsolutePath, file);
        }


        private void InintialSetUp()
        {
            if (!Path.Exists(ConfigAbsolutePath))
            {
                if (!Path.Exists(MainDirectoryPath))
                {
                    Runtime.ContractsForModules.ConsoleViewContract.Message($"\nThe installation is not set up yet. The setup will be at this location: {MainDirectoryPath}");
                    Directory.CreateDirectory(MainDirectoryPath);
                }

                // getting default information

                Runtime.ContractsForModules.ConsoleViewContract.Message("Set your username: ");
                string userName = Runtime.ContractsForModules.ConsoleViewContract.ReadUserInput("HCLI");

                Runtime.ContractsForModules.ConsoleViewContract.Message($"Setup complete {userName}! You can find the config file here: {ConfigAbsolutePath}\n");

                CreateConfigFile(userName, MainDirectoryPath);
            }

            ConfigData.SetupComplete = true;
        }


        private void CreateConfigFile(string userName, string? mainDirectoryPath)
        {
            ConfigData.UserName = userName;
            ConfigData.SetupComplete = true;


            string fileContent = JsonSerializer.Serialize(ConfigData);

            File.WriteAllText(ConfigAbsolutePath, fileContent);
            ConfigData = ConfigData;
        }
    }
}