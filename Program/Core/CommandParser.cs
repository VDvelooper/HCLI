using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core
{
    public class CommandParser
    {
        private static readonly Lexer _lexer = new Lexer();
        private static readonly FlagRegistry _flagRegistry = new FlagRegistry();

        public static UserInput? Parse(string rawInput) // lehetne static?
        {
            List<string> tokens = _lexer.Tokenize(rawInput);

            if (tokens.Count == 0) {
                return null;
                //throw new System.ArgumentException("Internal error: ParseException - Tokens count needs to be more than 0.");
            }

            string command = tokens[0];
            List<string> reqArgs = new List<string>();
            List<string> optArgs = new List<string>();
            List<ParsedFlag> flags = new List<ParsedFlag>();

            int _reqArgsRemain = Runtime.CommandRegistry.Commands[command].RequiredArgCount;
            int _optArgsRemain = Runtime.CommandRegistry.Commands[command].OptionalArgCount;
            
            for (int i = 1; i < tokens.Count; i++)
            {
                string token = tokens[i];

                if (IsTokenFlag(token))
                {
                    if (!_flagRegistry.TryGet(token, out FlagDefinition? foundFlag))
                        throw new System.ArgumentException($"Unknown flag. token: {token}");

                    if (foundFlag == null) 
                        throw new System.ArgumentException($"Flag was not found. token: {token}");

                    ParsedFlag parsedFlag = new ParsedFlag(foundFlag);

                    List<string>? values = parsedFlag.FlagDefinition!.RequiresValue
                        ? GetFlagValues(tokens, ref i, foundFlag)
                        : null;

                    if (values != null)
                    {
                        parsedFlag.Values.AddRange(values);
                    }
                    
                    flags.Add(parsedFlag);
                }
                else
                {
                    var parsedCommand = Runtime.CommandRegistry.Commands[command];
                    string arg = tokens[i];

                    if (_reqArgsRemain > 0)
                    {
                        reqArgs.Add(arg);
                        _reqArgsRemain--;
                    }
                    else if (_optArgsRemain > 0)
                    {
                        optArgs.Add(arg);
                        _optArgsRemain--;
                    }
                }
            }

            return new UserInput(command, reqArgs, optArgs, flags, rawInput);
        }

        private static bool IsTokenFlag(string token) => token.StartsWith("--");

        

        private static List<string> GetFlagValues(List<string> tokens, ref int i, FlagDefinition flagDef)
        {
            if (i + flagDef.RequiredValueCount >= tokens.Count)
                throw new System.ArgumentException($"A(z) '{flagDef.Syntax}' flag-hez {flagDef.RequiredValueCount} db érték szükséges.");

            List<string> foundValues = new List<string>();

            for (int x = i; x < tokens.Count; x++)
            {
                string value = tokens[x];
                foundValues.Add(value);
            }

            
            return foundValues;
        }
    }
}
