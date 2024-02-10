namespace RomanCalculator;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.Write("Введите выражение: ");
        string exp = Console.ReadLine();
        
        int cursorLeft = Console.CursorLeft;
        int cursorTop = Console.CursorTop;
        
        Console.SetCursorPosition(exp.Length + 19, cursorTop - 1);

        int result = RomanCalculator.Calculate(exp);
        if (result < 0)
        {
            Console.Write(" !=\n" +
                          "Отрицательно число не может быть записано в римское форме");
        }
        else if (result == 0)
        {
            Console.Write(" = 0");
        }
        else
        {
            Console.Write($" = {RomanCalculator.ConvertIntToRoman(result)} ({result})");
        }
    }
} 