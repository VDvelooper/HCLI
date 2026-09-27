namespace HCLI.Program.Core.Shared
{
    public class FlagDefinition
    {
        public string Syntax { get; set; }
        public bool Status { get; set; }
        public bool RequiresValue { get; set; }
        public int RequiredValueCount { get; set; }
        public List<string> Values { get; set; }

        public FlagDefinition(string syntax)
        {
            Values = new List<string>();
            Syntax = syntax;
        }
        public FlagDefinition(string syntax, bool requiresValue, int requiredValueCount)
        {
            Values = new List<string>();
            Syntax = syntax;
            RequiresValue = requiresValue;
            RequiredValueCount = requiredValueCount;
        }
    }
}
