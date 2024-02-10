namespace Lab03;

public static class Program
{
    public static void Main(string[] args)
    {
        Calendar calendar = new Calendar(2015);
        
        Console.WriteLine("Введите номер месяца в 2015 году, для просмотра календаря: ");
        
        int month = 0;
        string inp = Console.ReadLine();
        
        while (!Int32.TryParse(inp, out month) || month < 1 || month > 12)
        {
            Console.WriteLine("Вы ввели некорректный месяц\n" +
                              "Введите номер месяца в 2015 году, для просмотра календаря: ");
            inp = Console.ReadLine();
        }
        
        calendar.LogMonth(month);
    }
}