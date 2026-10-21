using HCLI.Modules.ExampleModule;
using HCLI.Modules.Secrecy;
using HCLI.Program.Abstractions;
using HCLI.Program.Core.Configuration;
using HCLI.Program.ModuleCore.Shared;
using System.Reflection;

namespace HCLI.Program
{
    public class ModuleRegistry
    {
        internal List<ModuleBase> AVALIBLE_MODULES; // szeretném hogy lehessen külön kiválasztani, hogy milyen modulok legyenek betöltve (1)
        internal List<ModuleBase> FOUND_MODULES;

        public string ModuleDllStoragePath
        {
            get { return @$"{ConfigManager.ConfigData.MainDirectoryPath}\ModuleData"; }
        }

        public ModuleRegistry()
        {
            // -- base object initialization -- //    <- (1) Ezeket csak akkor inicializálnánk, ha a felhasználó kiválasztja (mondjuk külső forrásból) a modult betöltésre
            CLASS_EXAMPLE = new ExampleModule(true);
            CLASS_SECRECY = new SecrecyModule(true);
            
            AVALIBLE_MODULES = new();
            FOUND_MODULES = new();

            AVALIBLE_MODULES.Add(CLASS_SECRECY); // (1) Akkor adjuk hozzá amikor a felhasználó kijelöli azt implementációra
        }

        public void FindModules()
        {
            Type targetClass = typeof(ModuleBase);
            Type moduleCoreType;

            List<string> possibleDics = Directory.GetDirectories(ModuleDllStoragePath).ToList();

            List<Type> FoundTypes = new();
            List<>

            foreach (string possibleDic in possibleDics)
            {
                Type[] moduleDllTypes = Assembly.LoadFrom(possibleDic).GetTypes();

                Type? foundModuleCore = moduleDllTypes.Where(type => 
                !type.IsAbstract && 
                !type.IsInterface && 
                type.IsSubclassOf(targetClass) && 
                !FoundTypes.Contains(type))
                .FirstOrDefault();

                if (foundModuleCore == null)
                    continue;

                dynamic? found = Activator.CreateInstance(foundModuleCore, true, Runtime.Services); // zzz majd a ModuleBase konstruktor param.-ben csak a Services-t keljen megadni!



                moduleCoreType = foundModuleCore;
                FoundTypes.Add(foundModuleCore);
            }
        }

        public bool TryGetModuleSubCommand(string command, out ICommand? found)
        {
            found = null;

            foreach (ModuleBase module in AVALIBLE_MODULES)
            {
                if (!module.SubCommands.Where(x => x.Syntax == command).Any()) 
                    continue;

                found = module.SubCommands.Where(x => x.Syntax == command).First();
                return true;
            }

            return false;
        }

        public bool TryGetBaseCommand(string command, out ICommand? found)
        {
            found = null;

            foreach (ModuleBase module in AVALIBLE_MODULES)
            {
                found = module.BaseCommands.Where(c => c.Syntax == command).FirstOrDefault();

                if (found != null)
                {
                    found = module.BaseCommands.Where(c => c.Syntax == command).First();
                    break;
                }
            }

            return found != null;
        }
        
        // -- Source Sets -- //

        private readonly ExampleModule CLASS_EXAMPLE;
        private readonly SecrecyModule CLASS_SECRECY;

    }
}
