using HCLI.Program.Core.Abstractions;
using System;

namespace HCLI.Program.Operations.Types
{
    internal class Incrementation : IOperationExecute<double>, IOperation
    {
        public string OperationSymbol { get; private set; }
        public int OperandCount { get; private set; }
        public bool OperandBeforeSymbol { get; private set; }
        public Action<double> ExecuteMethod { get; private set; }
        public Incrementation()
        {
            OperationSymbol = "++";
            OperandCount = 1;
            OperandBeforeSymbol = true;
            ExecuteMethod = Execute;
        }

        public double Execute(double[] input)
        {
            return input[0]++;   
        }

        public bool TryConvertToOprType(string raw, out double operand)
        {
            return double.TryParse(raw, null, out operand);
        }
        public bool TryConvertToOprType(List<string> raw, out List<double>? operands)
        {
            operands = new List<double>();

            foreach (string r in raw)
            {
                if (double.TryParse(r, null, out double e))
                    operands.Add(e);

            }

            if (operands.Count == 0)
            {
                operands = null!;
                return false;
            }

            return true;

        }
    }
}
