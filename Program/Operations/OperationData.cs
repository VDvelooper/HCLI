namespace HCLI.Program.Operations
{
    internal struct OperationData(/*bool BoolValue = false, double DoubleValue = 0.0, string StringValue = ""*/)
    {
        internal bool? BoolValue { get; set; } = null;
        internal double? DoubleValue { get; set; } = null;
        internal string? StringValue { get; set; } = null;
    }
}
