using HCLI.Program.Core.Shared;

namespace HCLI.Program.Core
{
    public class CommandParser
    {
        /*public UserInput CommandParse(string rawInput)
        {
            List<string> tokens = rawInput

            if (tokens.Count == 0) return null;


            if (Runtime.CommandRegistry.Commands.ContainsKey(tokens[0]))
            {
                userInput.Command = tokens[0];
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i];

                if (token.StartsWith("--"))
                {
                    List<string> toBeRemovedParts = new List<string>();

                    bool doNotRemoveFlagsFlag = false;
                    bool requestingFlagArg = false;

                    switch (currentPart)
                    {
                        case "--debug":
                            userInput.DetectedFlags.Debug = true;
                            toBeRemovedParts.Add(token);
                            break;
                        case "--flags":
                            doNotRemoveFlagsFlag = true;
                            toBeRemovedParts.Add(token);
                            break;
                    }

                    if (!doNotRemoveFlagsFlag)
                    {
                        foreach (string toBeRemoved in toBeRemovedParts)
                        {
                            tokens.Remove(toBeRemoved);
                        }
                    }

                    return detectedFlags;
                }

                if (i == 0)
                {
                    if (Runtime.CommandRegistry.Commands.ContainsKey(userInput.Command))
                    {      //                                                                    zzz
                        Runtime.CommandRegistry.Commands[userInput.Command].Execute(userInput); // a Commands-ban a Value-nál (ICommand) be kell vezetni az args db számát (property)
                    }
                    else if (Runtime.ModuleRegistry.MODULE_DATABASE.ContainsKey(userInput.Command))
                    {
                        Runtime.ModuleRegistry.TryExecutingCommandFromModule(userInput);
                    }
                    else
                    {
                        Console.WriteLine($"HCLI > The command '{userInput.Command}' is unknown.\n");
                        return null;
                    }
                }

                
            }

            UserInput userInput = new UserInput();
        }

        private */

        private readonly Lexer _lexer = new Lexer();
        private readonly FlagRegistry _flagRegistry = new FlagRegistry();

        public UserInput Parse(string rawInput) // lehetne static?
        {
            List<string> tokens = _lexer.Tokenize(rawInput);

            if (tokens.Count == 0) {
                throw new System.ArgumentException("Internal error: ParseException - Tokens count needs to be more than 0.");
            }

            string command = tokens[0];
            List<string> args = new List<string>();
            List<ParsedFlag> flags = new List<ParsedFlag>();

            for (int i = 1; i < tokens.Count; i++)
            {
                string token = tokens[i];

                if (IsFlagToken(token))
                {
                    if (!_flagRegistry.TryGet(tokens[i], out var foundFlag))
                        throw new System.ArgumentException($"Ismeretlen flag: {tokens[i]}");

                    ParsedFlag parsedFlag = new ParsedFlag(foundFlag);

                    List<string>? values = parsedFlag.FlagDefinition!.RequiresValue
                        ? GetFlagValues(tokens, ref i, foundFlag)
                        : null;

                    parsedFlag.Values.AddRange(values);
                    flags.Add(parsedFlag);
                }
                else {
                    args.Add(tokens[i]);
                }
            }
            
            return new UserInput(command, args, flags);
        }

        // innen mind AI

        private bool IsFlagToken(string token) => token.StartsWith("--");

        private List<string> GetFlagValues(List<string> tokens, ref int i, FlagDefinition flagDef)
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
