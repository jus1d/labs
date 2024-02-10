namespace Lab04;

public class Program
{
    public static void Main(string[] args)
    {
        Greeting();
        while (true)
        {
            Console.WriteLine("Выберете действие:\n\n" +
                              "\t1 - Счетчик\n" +
                              "\t2 - Многочлен\n" +
                              "\t3 - Выход из программы\n");

            string inp = Console.ReadLine();

            switch (inp)
            {
                case "1":
                    FirstTask();
                    break;
                case "2":
                    SecondTask();
                    break;
                case "3":
                    Console.WriteLine("До встречи!");
                    return;
                default:
                    Console.WriteLine("Нет такого пункта в меню");
                    break;
            }
        }
    }

    public static void FirstTask()
    {
        Console.Write("Введите начальное значение для счетчика: ");
        int startValue = Convert.ToInt32(Console.ReadLine());
        
        Console.Write("Введите нижнюю границу счетчика: ");
        int minimalValue = Convert.ToInt32(Console.ReadLine());
        while (minimalValue > startValue)
        {
            Console.Write("Нижняя граница не может быть больше начального значенние счетчика\n" +
                          "Введите нижнюю границу счетчика: ");
            minimalValue = Convert.ToInt32(Console.ReadLine());
        }
        
        Console.Write("Введите верхнюю границу счетчика: ");
        int maximalValue = Convert.ToInt32(Console.ReadLine());
        while (maximalValue <= minimalValue || maximalValue < startValue)
        {
            Console.Write("Верхняя граница счетчика должна быть больше нижней и больше начального значение счетчика\n" +
                          "Введите верхнюю границу счетчика: ");
            maximalValue = Convert.ToInt32(Console.ReadLine());
        }
        
        var counter = new Counter();

        while (true)
        {
            Console.WriteLine("\nВыберете действие:\n\n" +
                              "\t1 - Увеличить счетчик\n" +
                              "\t2 - Уменьшить счетчик\n" +
                              "\t3 - Изменить границы счетчика\n" +
                              "\t4 - Выйти в меню");
            string inp = Console.ReadLine();

            switch (inp)
            {
                case "1":
                    counter.Increase();
                    counter.Log();
                    break;
                case "2":
                    counter.Decrease();
                    counter.Log();
                    break;
                case "3":
                    counter.Log();
                    Console.Write("Введите новые границы счетчика через пробел: ");
                    string[] inputBounds = Console.ReadLine().Split(' ');
                    int[] bounds = new int[2];
                    for (int i = 0; i < bounds.Length; i++)
                    {
                        bounds[i] = Convert.ToInt32(inputBounds[i]);
                    }

                    if (bounds[0] > bounds[1])
                    {
                        int tmp = bounds[0];
                        bounds[0] = bounds[1];
                        bounds[1] = tmp;
                    }

                    counter.SetMinimalBound(bounds[0]);
                    counter.SetMaximalBound(bounds[1]);
                    
                    counter.Log();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Нет такого пункта в меню:");
                    break;
            }
        }
    }

    public static void SecondTask()
    {
        var equation = new QuadraticEquation();
        equation.Log();
        equation.Solve();
    }

    public static void Greeting()
    {
        Console.WriteLine("Лабораторная работа #4\n" +
                          "Выполнил студент группы 6101-020302D - Фадеев Артем");
    }
}