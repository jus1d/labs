namespace Lab02;

public static class Program
{
    public static void Main(string[] args)
    {
        Greeting();
        while (true)
        {
            Console.WriteLine("Выберете действие:\n\n" +
                              "\t0 - Выход из программы\n");

            string? inp = Console.ReadLine();

            switch (inp)
            {
                case "0":
                    Console.WriteLine("До скорой встречи, до скорой встречи!");
                    return;
                default:
                    Console.WriteLine("Нет такого пункта в меню");
                    break;
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    public static void Greeting()
    {
        Console.WriteLine("Языки Программирования и Структуры Данныхn\n" +
                          "Лабораторная работа #2\n" +
                          "Выполнил студент группы 6101-020302D - Фадеев Артем");
    }
}