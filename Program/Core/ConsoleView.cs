using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core
{
    public class ConsoleView
    {
        /*private const ConsoleColor DEFAULT_FOREGROUND_COLOR = ConsoleColor.Black;
        private const ConsoleColor DEFAULT_BACKGROUND_COLOR = ConsoleColor.White;*/

        public void ShowOutputOnConsole(IFeedback feedback)
        {
            switch (feedback.ResultType)
            {
                case Shared.Feedback.FeedbackType.Expected:
                    if (feedback.ConsoleOutput != null)
                        Message(feedback.ConsoleOutput);
                    break;
            }
        }

        
        
        public void Message(string message)
        {
            Console.ResetColor();
            Console.WriteLine(message);
        }
        public void Warn(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void Error(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
