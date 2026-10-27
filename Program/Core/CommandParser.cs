using ModuleContracts.ServiceContracts;
using ModuleContracts.Abstractions;
using ModuleContracts.Feedback.Error;
using ModuleContracts.Feedback.Warning;

namespace HCLI.Program.Core
{
    public class CommandParser : ICommandParserContract
    {
        private static readonly Lexer _lexer = new Lexer();
        private static readonly FlagRegistry _flagRegistry = new FlagRegistry();

        internal UserInput? Parse(string? raw)
        {
            if (string.IsNullOrEmpty(raw)) 
                return null;

            List<string> tokens = _lexer.Tokenize(raw);

            if (tokens.Count == 0) {
                return null;
            }

            string rawCommand = tokens[0];

            ICommand command;
            List<string> reqArgs = new List<string>();
            List<string> optArgs = new List<string>();
            List<ParsedFlag> flags = new List<ParsedFlag>();



            int _reqArgsRemain = 0;
            int _optArgsRemain = 0;



            if (Runtime.CommandRegistry.TryGetHCLICommand(rawCommand, out ICommand? foundHCLICommand) && foundHCLICommand != null)
            {
                _reqArgsRemain = foundHCLICommand.RequiredArgCount;
                _optArgsRemain = foundHCLICommand.OptionalArgCount;
                command = foundHCLICommand;
                //Runtime.ContractsForModules.ConsoleViewContract.Debug($"Found HCLI command. Command: {rawCommand}");
            }
            else if (Runtime.ModuleRegistry.TryGetModuleSubCommand(rawCommand, out ICommand? foundModuleCommand) && foundModuleCommand != null)
            {
                _reqArgsRemain = foundModuleCommand.RequiredArgCount;
                _optArgsRemain = foundModuleCommand.OptionalArgCount;
                command = foundModuleCommand;
                //Runtime.ContractsForModules.ConsoleViewContract.Debug($"Found sub command. Command: {rawCommand}");
            }
            else if (Runtime.ModuleRegistry.TryGetBaseCommand(rawCommand, out ICommand? found) && found != null)
            {
                _reqArgsRemain = found.RequiredArgCount;
                _optArgsRemain = found.OptionalArgCount;
                command = found;
                //Runtime.ContractsForModules.ConsoleViewContract.Debug($"Found base command. Command: {rawCommand}");
            }
            else
            {
                Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new UnknownCommand($"The command {rawCommand} is uknown."));
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
                        Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new NotFound($"Flag was not found with the token \"{token}\"."));
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
                        Runtime.ConsoleView.PresentFeedback(new UnusedOptionalArgument("CommandParser.cs"));
                    }
                }
            }



            if (reqArgs.Count < returnReqArgs)
            {
                Runtime.ConsoleView.PresentFeedback(new MissingRequiredArgument($"Not enough arguments were provided. {returnReqArgs - reqArgs.Count} required argument(s) is missing."));
                return null;
            }
            else
                return new UserInput(command, reqArgs, returnReqArgs, optArgs, returnOptArgs, flags, raw);
        }

        public UserInput? Parse(string? rawInput, ModuleBase sender)
        {
            if (string.IsNullOrEmpty(rawInput))
                return null;

            List<string> tokens = _lexer.Tokenize(rawInput);

            if (tokens.Count == 0)
            {
                return null;
            }


            string rawCommand = tokens[0];

            tokens.ForEach(x => Console.WriteLine(x));

            Console.WriteLine();

            ICommand command;
            List<string> reqArgs = new List<string>();
            List<string> optArgs = new List<string>();
            List<ParsedFlag> flags = new List<ParsedFlag>();



            int _reqArgsRemain = 0;
            int _optArgsRemain = 0;

            
            // Nem kellene a BaseCommandokat prioretizálni?
            if (Runtime.ModuleRegistry.TryGetModuleSubCommand(rawCommand, out ICommand? foundModuleCommand) && foundModuleCommand != null)
            {
                _reqArgsRemain = foundModuleCommand.RequiredArgCount;
                _optArgsRemain = foundModuleCommand.OptionalArgCount;
                command = foundModuleCommand;
            }
            else if (Runtime.ModuleRegistry.TryGetBaseCommand(rawCommand, out ICommand? found) && found != null)
            {
                _reqArgsRemain = found.RequiredArgCount;
                _optArgsRemain = found.OptionalArgCount;
                command = found;
            }
            else
            {
                Runtime.ConsoleView.PresentFeedback(new UnknownCommand($"The command {rawCommand} is uknown."));
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
                        Runtime.ConsoleView.PresentFeedback(new NotFound($"Flag was not found with the token \"{token}\"."));
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
                        Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new UnusedOptionalArgument("CommandParser.cs"));
                    }
                }
            }



            if (reqArgs.Count < returnReqArgs)
            {
                Runtime.ConsoleView.PresentFeedback(new MissingRequiredArgument($"Not enough arguments were provided. {returnReqArgs - reqArgs.Count} required argument(s) is missing."));
                return null;
            }
            else
                return new UserInput(command, reqArgs, returnReqArgs, optArgs, returnOptArgs, flags, rawInput);
        }

        // -- private -- //


        private bool IsTokenFlag(string token) => token.StartsWith("--");

        private  List<string> GetFlagValues(List<string> tokens, ref int i, FlagDefinition flagDef)
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
