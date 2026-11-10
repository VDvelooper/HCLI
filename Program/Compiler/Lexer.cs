using HCLI.Program.Core.Abstractions;
using System.Text;

namespace HCLI.Program.Compiler
{
    internal class Lexer
    {
        public List<string> Tokenize(string rawInput)
        {
            List<string> tokens = new List<string>();

            char nextToken = char.MaxValue;
            StringBuilder current = new StringBuilder();

            bool IsInsideQuotation = false;

            for (int i = 0; rawInput.Length > i; i++)
            {
                char c = rawInput[i];

                if (i + 1 < rawInput.Length)
                    nextToken = rawInput[i + 1];
                else
                    nextToken = char.MaxValue;

                if (c == '"')
                {
                    IsInsideQuotation = !IsInsideQuotation;
                    continue;
                }

                // operations
                if (!IsInsideQuotation)
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        // both is contained within an operator and they match (++, --)
                        if (
                            nextToken != char.MaxValue &&
                            Runtime.OperationsRegistry.IsCharPartOfMultiSymbOp(c) &&
                            Runtime.OperationsRegistry.IsCharPartOfMultiSymbOp(nextToken) &&
                            Runtime.OperationsRegistry.TryGetOperation(c.ToString() + nextToken, out IOperation? _foundDouble) &&
                            _foundDouble != null)
                        {
                            if (current.Length > 0)
                            {
                                tokens.Add(current.ToString());

                                // we reset, so we add the operator differently
                                current.Clear();

                                current.Append(c);
                                current.Append(nextToken);

                                tokens.Add(current.ToString());

                                current.Clear();

                                i +=2;
                                continue;
                            }
                            else
                            {
                                current.Append(c);
                                current.Append(nextToken);

                                tokens.Add(current.ToString());

                                current.Clear();

                                i++;
                                continue;
                            }
                        }
                        else if (
                            Runtime.OperationsRegistry.TryGetOperation(c.ToString() + nextToken, out IOperation? _foundSingle) &&
                            _foundSingle != null)
                        {
                            if (current.Length > 0)
                            {
                                tokens.Add(current.ToString());

                                current.Clear();

                                // we reset, so we add the operator differently
                                tokens.Add(c.ToString());

                                continue;
                            }
                            else
                            {
                                tokens.Add(c.ToString());

                                continue;
                            }
                        }
                    }
                    else // its whitespace
                    {
                        if (current.Length > 0)
                        {
                            tokens.Add(current.ToString());
                            current.Clear();
                        }
                        continue;
                    }                    
                }
                else // it's in quotation
                {
                    // ha a felhasználó ténylegesen bele akarja tenni a "-t
                    if (c == '\\' && i + 1 < rawInput.Length && rawInput[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                        continue;
                    }
                }
                
                current.Append(c);
                
            }

            if (current.Length > 0)
            {
                tokens.Add(current.ToString());
            }

            if (tokens != null && tokens.Count > 0) tokens.ForEach(t => Runtime.ConsoleView.Debug($"{t},"));

            return tokens!;
        }

        private bool TryGetNextChar(int currentIndex, string raw, out char? c)
        {
            if (raw.Length <= currentIndex)
            {
                c = null;
                return false;
            }

            c = raw[currentIndex + 1];
            return true;
        }

    }
}
