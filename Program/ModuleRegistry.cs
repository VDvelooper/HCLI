using HCLI.Modules.ExampleModule;
using HCLI.Modules.Secrecy;
using HCLI.Program.Core.Shared;

//using HCLI.Program.Base;
using HCLI.Program.ModuleCore;
using System.Security.Cryptography.X509Certificates;

namespace HCLI.Program
{
    public class ModuleRegistry
    {
        // -- Modules -- //

        public ModuleData MODULE_SECRECY;
        public List<ModuleData> AVALIBLE_MODULES; // szeretném hogy lehessen külön kiválasztani, hogy milyen modulok legyenek betöltve (1)

        // -- Fields -- //

        public Dictionary<string, ModuleData> MODULE_DATABASE;


        public ModuleRegistry()
        {

            // -- base object initialization -- //    <- (1) Ezeket csak akkor inicializálnánk, ha a felhasználó kiválasztja (mondjuk külső forrásból) a modult betöltésre
            CLASS_EXAMPLE = new ExampleModule(true);
            CLASS_SECRECY = new SecrecyModule(true);
            


            // -- data object initialization -- //
            MODULE_SECRECY = new ModuleData(CLASS_SECRECY.ModuleName, CLASS_SECRECY.ModuleCommand, CLASS_SECRECY, CLASS_SECRECY.SubCommands);



            MODULE_DATABASE = new Dictionary<string, ModuleData>()
            {
                [MODULE_SECRECY.ModuleCommand] = MODULE_SECRECY
            };


            AVALIBLE_MODULES = new();

            AVALIBLE_MODULES.Add(MODULE_SECRECY); // (1) Akkor adjuk hozzá amikor a felhasználó kijelöli azt implementációra
        }


        public bool TryExecutingCommandFromModule(Core.Shared.UserInput userInput)
        {
            foreach (ModuleData module in AVALIBLE_MODULES)
            {
                if (module.ModuleCommand == userInput.Command)
                {
                    module.Module.ModuleExecute(userInput);
                    return true;
                }
            }

            return false;
        }

        public bool TryGetModuleCommand(string comment, out Core.Abstractions.ICommand? found) // zzz -> át kell írni a module-oknak a kommandjait külön .cs-re, mindet aztán erre visszatérni.
        {
            found = null;

            foreach (ModuleData module in AVALIBLE_MODULES)
            {
                if (!module.SubCommands.Where(x => x.Syntax == comment).Any()) 
                    continue;

                found = module.SubCommands.Where(x => x.Syntax == comment).First();
                return true;
            }

            return false;
        }

        // -- Source Sets -- //

        private readonly ExampleModule CLASS_EXAMPLE;
        private readonly SecrecyModule CLASS_SECRECY;

    }
}
