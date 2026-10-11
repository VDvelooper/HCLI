using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core
{
    public class ConsoleView
    {
        public static void ShowOutputOnConsole(IFeedback feedback)
        {
            switch (feedback.ResultType)
            {
                case Shared.Feedback.FeedbackType.Expected:

                    if (feedback.DebugMessage != null)
                        Debug($"{feedback.FeedbackName}: {feedback.DebugMessage}");
                    if (feedback.ConsoleOutput != null)
                        Message(feedback.ConsoleOutput, feedback.ConsoleOutputStyle);
                    break;


                case Shared.Feedback.FeedbackType.Warning:

                    if (feedback.DebugMessage != null)
                        Debug($"{feedback.FeedbackName}: {feedback.DebugMessage}");
                    else
                        Warn($"{feedback.FeedbackName}");
                    break;


                case Shared.Feedback.FeedbackType.Error:

                    if (feedback.DebugMessage != null)
                        Debug($"{feedback.FeedbackName}: {feedback.DebugMessage}");
                    else
                        Error($"{feedback.FeedbackName}");
                    break;
            }
        }



        public static void Message(string message, ConsoleOutputStyle? outputStyle)
        {
            if (outputStyle != null) {
                Console.ForegroundColor = outputStyle.ForegroundColor;
                Console.BackgroundColor = outputStyle.BackgroundColor;
            }
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
