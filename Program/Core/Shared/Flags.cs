namespace HCLI.Program.Core.Shared
{
    public class Flags
    {
        private bool _anyFlags = false;
        private bool _debugFlag = false;

        public bool Any { get { return _anyFlags; } set { _anyFlags = Debug; } }
        public bool Debug 
        { 
            get { return _debugFlag; } 
            set 
            { 
                _debugFlag = value;
                FlagNamesAndValues.Add("Debug", _debugFlag);
            } 
        }
        // a tobbi...

        public Dictionary<string, bool> FlagNamesAndValues { get; private set; }


        public Flags()
        {
            FlagNamesAndValues = new Dictionary<string, bool>();
        }

        public void DebugPrintFlags()
        {
            foreach (KeyValuePair<string, bool> pair in FlagNamesAndValues) {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }
    }
}
