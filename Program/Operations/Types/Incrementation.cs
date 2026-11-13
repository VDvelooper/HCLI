using HCLI.Program.Core.Abstractions;
using System;

namespace HCLI.Program.Operations.Types
{
    internal class Incrementation : IOperation
    {
        public string OperationSymbol { get; private set; }
        public int OperandCount { get; private set; }
        public bool OperandBeforeSymbol { get; private set; }
        public Incrementation()
        {
            OperationSymbol = "++";
            OperandCount = 1;
            OperandBeforeSymbol = true;
        }

        public OperationData? Execute(List<string> input) // zzz
        {
            List<OperationData>? operandValues = new List<OperationData>();

            if (TryConvertToOprType(input, out operandValues))
            {
                if (operandValues!.Count != OperandCount)
                    return null;

                OperationData ret = operandValues[0];

                ret.DoubleValue++;

                return ret;

            }
            else return null;
        }

        public bool TryConvertToOprType(string raw, out OperationData operand)
        {
            operand = new OperationData();

            double value = 0;
            bool success = false;

            if (double.TryParse(raw, out value))
            {
                operand.DoubleValue = value;
                success = true;
            }

            return success;
        }

        public bool TryConvertToOprType(List<string> raw, out List<OperationData>? operands)
        {
            operands = new List<OperationData>();

            double value = 0;

            foreach (string r in raw)
            {
                if (double.TryParse(r, out value))
                    operands.Add(new OperationData() { DoubleValue = value });
                else
                {
                    operands = null!;
                    return false;
                }
            }

            return true;

        }
    }
}
