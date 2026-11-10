using ModuleContracts.ServiceContracts;
using ModuleContracts.Abstractions;
using ModuleContracts.Feedback.Error;
using ModuleContracts.Feedback.Warning;
using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Compiler
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


            // unary/binary/(trinary) operations check


            for (int i = 0; i < tokens.Count; i++)
            {

                string token = tokens[i];

                if (!Runtime.OperationsRegistry.TryGetOperation(token, out IOperation? foundOp) || foundOp == null)
                    continue;

                int oprCount = foundOp.OperandCount;

                switch (foundOp.OperandCount)
                {
                    case 1:

                        if (foundOp.OperandBeforeSymbol)
                        {
                            string operandRaw = tokens[i - 1];

                            

                        }

                        break;

                    case 2:

                        // binary op

                        break;

                    case 3:

                        // trinary op

                        break;

                    default:
                        Runtime.ConsoleView.PresentFeedback(new NotFound("The found operation's OperandCount is not acceptable."));
                        break;
                }


            }


            // argument operation check 


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

            UserInput? userInput = new UserInput(command, reqArgs, returnReqArgs, optArgs, returnOptArgs, flags, raw);

            if (userInput == null)
            {
                Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new InternalNullExeption("Generated UserInput was null."));
                return null;
            }

            if (!Runtime.SemanticValidator.Validate(userInput))
            {
                Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new UnknownCommand("The given command couldn't be validated semanticly."));
                return null;
            }
            else
                return userInput;
        }

        public UserInput? Parse(string? raw, ModuleBase sender)
        {
            if (string.IsNullOrEmpty(raw))
                return null;

            List<string> tokens = _lexer.Tokenize(raw);

            if (tokens.Count == 0)
            {
                return null;
            }


            string rawCommand = tokens[0];


            ICommand command = null!;
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

            UserInput? userInput = new UserInput(command, reqArgs, returnReqArgs, optArgs, returnOptArgs, flags, raw);

            if (userInput == null)
            {
                Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new InternalNullExeption("Generated UserInput was null."));
                return null;
            }

            if (!Runtime.SemanticValidator.Validate(userInput))
            {
                Runtime.ContractsForModules.ConsoleViewContract.PresentFeedback(new UnknownCommand("The given command couldn't be validated semanticly."));
                return null;
            }
            else
                return userInput;
        }

        /*public bool TryParseGeneric<T>(string input, out T result) where T : struct
        {
            var type = typeof(T);
            var method = type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType() });

            if (method == null)
            {
                result = default(T);
                return false;
            }

            var parameters = new object[] { input, null! };
            bool success = (bool)method.Invoke(null, parameters);
            result = (T)parameters[1];
            return success;
        }*/


        // -- private -- //


        private bool IsTokenFlag(string token) => token.StartsWith("--");

        private  List<string> GetFlagValues(List<string> tokens, ref int i, FlagDefinition flagDef)
        {
            if (i + flagDef.RequiredValueCount >= tokens.Count)
                throw new ArgumentException($"A(z) '{flagDef.Syntax}' flag-hez {flagDef.RequiredValueCount} db érték szükséges.");

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
