namespace HCLI.Program.Core.Abstractions
{
    public class ConsoleOutputStyle
    {
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;
        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleOutputStyle() { }
        public ConsoleOutputStyle(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }
}
