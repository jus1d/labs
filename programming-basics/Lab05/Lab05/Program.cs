namespace Lab05;

public static class Program
{
    public static void Main(string[] args)
    {
        Greeting();

        while (true)
        {
            Console.WriteLine("Выберете действие:\n" +
                              "\t1 - Сложение\n" +
                              "\t2 - Вычитание\n" +
                              "\t3 - Умножение\n" +
                              "\t4 - Деление\n" +
                              "\t5 - Сокращение\n" +
                              "\t6 - Выход из программы");

            string inp = Console.ReadLine();

            Fraction a, b, result;
            switch (inp)
            {
                case "1":
                    a = Fraction.GetFromUserInput("Введите первую дробь: ");
                    b = Fraction.GetFromUserInput("Введите вторую дробь: ");
                    result = a + b; 
                    
                    Console.WriteLine($"{a.ToString()} + {b.ToString()} = {result.ToString()}");
                    break;
                case "2":
                    a = Fraction.GetFromUserInput("Введите первую дробь: ");
                    b = Fraction.GetFromUserInput("Введите вторую дробь: ");
                    result = a - b; 
                    
                    Console.WriteLine($"{a.ToString()} - {b.ToString()} = {result.ToString()}");
                    break;
                case "3":
                    a = Fraction.GetFromUserInput("Введите первую дробь: ");
                    b = Fraction.GetFromUserInput("Введите вторую дробь: ");
                    result = FractionActions.Multiplication(a, b);
                    
                    Console.WriteLine($"{a.ToString()} * {b.ToString()} = {result.ToString()}");
                    break;
                case "4":
                    a = Fraction.GetFromUserInput("Введите первую дробь: ");
                    do
                    {
                        b = Fraction.GetFromUserInput("Введите вторую дробь: ");
                    } while (b.Numerator == 0);
                    result = a / b;
                    result = FractionActions.Division(a, b);
                    
                    Console.WriteLine($"{a.ToString()} / {b.ToString()} = {result.ToString()}");
                    break;
                case "5":
                    a = Fraction.GetFromUserInput();
                    Console.WriteLine($"Результат сокращения: {a.Reduce().ToString()}");
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("В меню нет такого пункта");
                    break;
            }
        }
    }

    public static void Greeting()
    {
        Console.WriteLine("Лабораторная работа #5\n" +
                          "Выполнил студент группы 6101-020302D - Фадеев Артем");
    }
}