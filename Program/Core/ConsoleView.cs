using HCLI.Program.Core.Abstractions;

namespace HCLI.Program.Core
{
    public class ConsoleView
    {
        public static void ThrowFeedback(IFeedback feedback)
        {
            if (feedback.Silent) return;

            switch (feedback.ResultType)
            {
                // When expected, the ConsoleOutputOverride is never null.
                case Shared.Feedback.FeedbackType.Expected:

                    if (feedback.ConsoleOutputOverride != null)
                        Message(feedback.ConsoleOutputOverride, feedback.ConsoleOutputStyle);
                    else if (feedback.Description != null)
                        Debug($"{feedback.FeedbackID}: {feedback.Description}");
                    break;


                case Shared.Feedback.FeedbackType.Warning:

                    if (feedback.Description != null)
                        Warn($"{feedback.Description} : {feedback.FeedbackID}");
                    else if (feedback.ConsoleOutputOverride != null)
                        Warn($"{feedback.ConsoleOutputOverride}");
                    else
                        Warn($"{feedback.ConsoleOutputOverride} (origin: {feedback.FeedbackID})");
                    break;


                case Shared.Feedback.FeedbackType.Error:

                    if (feedback.Description != null)
                        Error($"{feedback.Description} : {feedback.FeedbackID}");
                    else if (feedback.ConsoleOutputOverride != null)
                        Error($"{feedback.ConsoleOutputOverride}");
                    else
                        Error($"{feedback.ConsoleOutputOverride} (origin: {feedback.FeedbackID})");
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
            Console.WriteLine();
        }
        public static void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine();
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
            Console.WriteLine();
        }
    }
}
