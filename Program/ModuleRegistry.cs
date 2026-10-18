using HCLI.Modules.ExampleModule;
using HCLI.Modules.Secrecy;
using HCLI.Program.Abstractions;
using HCLI.Program.ModuleCore.Shared;

namespace HCLI.Program
{
    public class ModuleRegistry
    {
        public List<ModuleBase> AVALIBLE_MODULES; // szeretném hogy lehessen külön kiválasztani, hogy milyen modulok legyenek betöltve (1)

        public ModuleRegistry()
        {
            // -- base object initialization -- //    <- (1) Ezeket csak akkor inicializálnánk, ha a felhasználó kiválasztja (mondjuk külső forrásból) a modult betöltésre
            CLASS_EXAMPLE = new ExampleModule(true);
            CLASS_SECRECY = new SecrecyModule(true);
            
            AVALIBLE_MODULES = new();

            AVALIBLE_MODULES.Add(CLASS_SECRECY); // (1) Akkor adjuk hozzá amikor a felhasználó kijelöli azt implementációra
        }

        public bool TryGetModuleSubCommand(string command, out ICommand? found) // zzz -> át kell írni a module-oknak a kommandjait külön .cs-re, mindet aztán erre visszatérni.
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
