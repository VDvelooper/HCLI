using HCLI.Program.Core.Shared;

namespace HCLI.Program
{
    public class FlagRegistry
    {
        public List<FlagDefinition> Flags;

        public FlagRegistry()
        {

            FlagDefinition flag_debug = new FlagDefinition("--debug", Core.FlagID.Debug);
            FlagDefinition flag_flags = new FlagDefinition("--flags", Core.FlagID.Debug);

            Flags = new()
            {
                { flag_debug },
                { flag_flags }
            };
        }


        public bool TryGet(string syntax, out FlagDefinition found)
        {
            found = Flags.FirstOrDefault(x => x.Syntax == syntax);
            return found != null;
        }
    }
}
