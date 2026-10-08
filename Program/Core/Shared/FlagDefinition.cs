namespace HCLI.Program.Core.Shared
{
    public class FlagDefinition
    {
        public string Syntax { get; private set; }
        public string Name { get; private set; }
        public FlagID FlagID { get; }
        public bool Status { get; set; }
        public bool RequiresValue { get; set; }
        public int RequiredValueCount { get; set; }
        public List<string> Values { get; set; }

        public FlagDefinition(string syntax, string name, FlagID id)
        {
            Values = new List<string>();
            Syntax = syntax;
            Name = name + " flag";
        }
        public FlagDefinition(string syntax, string name, FlagID id, bool requiresValue, int requiredValueCount)
        {
            Values = new List<string>();
            Syntax = syntax;
            Name = name + " flag";
            RequiresValue = requiresValue;
            RequiredValueCount = requiredValueCount;
        }
    }
}
