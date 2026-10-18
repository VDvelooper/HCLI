namespace HCLI.Program.Core.Shared
{
    public class ParsedFlag
    {
        public FlagID FlagID { get; }
        public FlagDefinition FlagDefinition { get; }
        public List<string> Values { get; } = new List<string>();

        public ParsedFlag(FlagDefinition flagDefinition) {
            FlagDefinition = flagDefinition;
            FlagID = FlagDefinition.FlagID;
        }
    }
}
