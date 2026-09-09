using HCLI.Modules;
using HCLI.Program.Base;

namespace HCLI.Program
{
    public class ModuleManager
    {
        // -- Modules -- //

        public ModuleData MODULE_SECRECY;
        public List<ModuleData> AVALIBLE_MODULES; // szeretném hogy lehessen külön kiválasztani, hogy milyen modulok legyenek betöltve (1)

        // -- Variables -- //

        public Dictionary<string, ModuleData> MODULE_DATABASE;


        public ModuleManager()
        {

            // -- base object initialization -- //    <- (1) Ezeket csak akkor inicializálnánk, ha a felhasználó kiválasztja (mondjuk külső forrásból) a modult betöltésre
            CLASS_SECRECY = new SecrecyModule("Secrecy", "secrecy", true);


            // -- data object initialization -- //
            MODULE_SECRECY = new ModuleData(CLASS_SECRECY.ModuleName, CLASS_SECRECY.ModuleCommand, CLASS_SECRECY.Execute);



            MODULE_DATABASE = new Dictionary<string, ModuleData>()
            {
                [MODULE_SECRECY.ModuleCommand] = MODULE_SECRECY
            };


            AVALIBLE_MODULES = new();

            AVALIBLE_MODULES.Add(MODULE_SECRECY); // (1) Akkor adjuk hozzá amikor a felhasználó kijelöli azt implementációra
        }


        public bool TryExecutingCommandFromModule(UserInput userInput)
        {
            foreach (ModuleData module in AVALIBLE_MODULES)
            {
                if (module.ModuleCommand == userInput.Command)
                {
                    module.Execute.Invoke(userInput.ToModuleModeInput());
                    return true;
                }
            }

            return false;
        }


        // -- Source Sets -- //

        private readonly SecrecyModule CLASS_SECRECY;

    }
}
