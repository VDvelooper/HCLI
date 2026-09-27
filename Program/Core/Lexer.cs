using System.Text;

namespace HCLI.Program.Core
{
    public class Lexer
    {
        public List<string> Tokenize(string rawInput)
        {
            List<string> tokens = new List<string>();
            StringBuilder current = new StringBuilder();

            bool IsInsideQuotation = false;

            for (int i = 0; rawInput.Length > i; i++)
            {
                char c = rawInput[i];

                if (c == '"')
                {
                    IsInsideQuotation = !IsInsideQuotation;
                    continue;
                }

                // ha a felhasználó ténylegesen bele akarja tenni a "-t
                if (c == '\\' && i + 1 < rawInput.Length && rawInput[i + 1] == '"')
                {
                    current.Append('"');
                    i++;
                    continue;
                }

                if (char.IsWhiteSpace(c) && !IsInsideQuotation)
                {
                    if (current.Length > 0)
                    {
                        tokens.Add(current.ToString());
                        current.Clear();
                    }
                    continue;
                }

                current.Append(c);
            }

            if (current.Length > 0)
            {
                tokens.Add(current.ToString());
            }

            return tokens;
        }
    }
}
