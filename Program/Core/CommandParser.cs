using HCLI.Program.Core.Abstractions;
using HCLI.Program.Core.Shared;
using HCLI.Program.Core.Shared.Feedback.Error;
using HCLI.Program.Core.Shared.Feedback.Errors;
using HCLI.Program.Core.Shared.Feedback.Warning;

namespace HCLI.Program.Core
{
    public class CommandParser
    {
        private static readonly Lexer _lexer = new Lexer();
        private static readonly FlagRegistry _flagRegistry = new FlagRegistry();

        public static UserInput? Parse(string rawInput)
        {
            List<string> tokens = _lexer.Tokenize(rawInput);

            if (tokens.Count == 0) {
                return null;
            }


            string command = tokens[0];
            List<string> reqArgs = new List<string>();
            List<string> optArgs = new List<string>();
            List<ParsedFlag> flags = new List<ParsedFlag>();



            int _reqArgsRemain = 0;
            int _optArgsRemain = 0;



            if (Runtime.CommandRegistry.TryGetHCLICommand(command, out _))
            {
                _reqArgsRemain = Runtime.CommandRegistry.Commands[command].RequiredArgCount;
                _optArgsRemain = Runtime.CommandRegistry.Commands[command].OptionalArgCount;
            }
            else if (Runtime.ModuleRegistry.TryGetModuleCommand(command, out ICommand? foundCommand))
            {
                if (foundCommand == null) throw new System.ArgumentException($"Command was not found. command: {command}"); 

                _reqArgsRemain = foundCommand.RequiredArgCount;
                _optArgsRemain = foundCommand.OptionalArgCount;
            }
            else if (Runtime.ModuleRegistry.TryGetBaseCommand(command, out Action<UserInput>? found))
            {
                _reqArgsRemain = 0; // zzz -> a base command-okat át kellene írni ICommand-ra.
                _optArgsRemain = 0;
            }
            else
            {
                ConsoleView.PresentFeedback(new UknownCommand($"The command {command} is uknown."));
                return null;
            }



            int returnReqArgs = _reqArgsRemain;
            int returnOptArgs = _optArgsRemain;
            bool flaggedWarning = false;



            for (int i = 1; i < tokens.Count; i++)
            {
                string token = tokens[i];
                
                if (IsTokenFlag(token))
                {
                    if (!_flagRegistry.TryGet(token, out FlagDefinition? foundFlag) || foundFlag == null)
                    {
                        ConsoleView.PresentFeedback(new NotFound($"Flag was not found with the token \"{token}\"."));
                        return null;
                    }
                
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
                    // parsing the arguments
                    string arg = tokens[i];
                
                    if (_reqArgsRemain > 0)
                    {
                         reqArgs.Add(arg);
                         _reqArgsRemain--;
                         continue;
                    }
                    else if (_optArgsRemain > 0)
                    {
                        optArgs.Add(arg);
                        _optArgsRemain--;
                        continue;
                    }
                    else if (!flaggedWarning) 
                    {
                        ConsoleView.PresentFeedback(new UnusedOptionalArgument("CommandParser.cs"));
                    }
                }
            }



            if (reqArgs.Count < returnReqArgs)
            {
                ConsoleView.PresentFeedback(new MissingRequiredArgument($"Not enough arguments were provided. {returnReqArgs - reqArgs.Count} required argument(s) is missing."));
                return null;
            }
            else
                return new UserInput(command, reqArgs, returnReqArgs, optArgs, returnOptArgs, flags, rawInput);


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
