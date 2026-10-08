using HCLI.Program.Core.Shared;

namespace HCLI.Program
{
    public class FlagRegistry
    {
        public List<FlagDefinition> Flags;

        public FlagRegistry()
        {

            FlagDefinition flag_debug = new FlagDefinition("--debug", "Debug", Core.FlagID.Debug);

            Flags = new()
            {
                { flag_debug }
            };
        }


        public bool TryGet(string syntax, out FlagDefinition? found)
        {
            found = Flags.FirstOrDefault(x => x.Syntax == syntax);
            return found != null;
        }
    }
}
