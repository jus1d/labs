namespace Calculator;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Введите математическое выражение:");
        string exp = Console.ReadLine();

        int cursorLeft = Console.CursorLeft;
        int cursorTop = Console.CursorTop;
        
        Console.SetCursorPosition(exp.Length, cursorTop - 1);

        try
        {
            double result = Calculator.Calculate(exp);
            Console.Write(" = " + result);
        }
        catch (DivideByZeroException)
        {
            Console.Write(" != ");
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.WriteLine("Выражение не может быть посчитано, деление на 0 запрещено!");
        }
        catch (Exception e)
        {
            Console.Write(" != ");
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.WriteLine("Некорректное выражение");
            Console.WriteLine(e.ToString());
        }
    }
}