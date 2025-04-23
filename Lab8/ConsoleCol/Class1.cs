namespace utils
{
    public static class ConsoleCol
    {
        public static void WriteLine(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = previousColor;
        }

        public static async Task WriteLineAsync(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            await Console.Out.WriteLineAsync(message); 

            Console.ForegroundColor = previousColor;
        }
        
    }
}