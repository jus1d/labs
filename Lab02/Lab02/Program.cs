namespace Lab02;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Лабораторная работа #2. Выполнил Фадеев Артем. Группа - 6101-020302D\n");
        while (true)
        {
            Console.WriteLine("Выберете действие:\n\n" +
                              "\t1 - Таблица значений функции\n" +
                              "\t2 - Серия выстрелов по мишени\n" +
                              "\t3 - Сумма ряда\n" +
                              "\t4 - Выйти из программы");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    double xMin, xMax, dx;
                    
                    Console.Write("Введите первый элемент: ");
                    xMin = Convert.ToDouble(Console.ReadLine().Replace(".", ","));
                    
                    Console.Write("Введите последний элемент: ");
                    xMax = Convert.ToDouble(Console.ReadLine().Replace(".", ","));
                    
                    Console.Write("Введите шаг: ");
                    dx = Convert.ToDouble(Console.ReadLine().Replace(".", ","));
                    
                    FirstTask(xMin, xMax, dx);
                    break;
                case "2":
                    SecondTask();
                    break;
                case "3":
                    double acc, x;
                    
                    Console.Write("Введите точность для расчетов: ");
                    acc = Convert.ToDouble(Console.ReadLine().Replace(".", ","));
                    
                    Console.Write("Введите X: ");
                    x = Convert.ToDouble(Console.ReadLine().Replace(".", ","));
                    
                    ThirdTask(acc, x);
                    break;
                case "4":
                    Console.WriteLine("До встречи!");
                    return;
                default:
                    Console.WriteLine("Нет такого пункта в меню, попробуйте еще раз");
                    break;
            }
        }
        
    }

    public static void FirstTask(double xMin, double xMax, double dx)
    {
        Console.WriteLine("\t\tx\t\ty\n");
        for (double i = xMin; i <= xMax; i += dx)
        {
            double y = Function(i);
            if (y == double.MinValue)
            {
                Console.WriteLine($"\t{i,11:0.00}\t\t-");
            }
            else
            {
                Console.WriteLine($"\t{i,11:0.00}\t{y,11:0.00}");
            }
        }
    }

    public static void SecondTask()
    {
        bool isContinue = true;
                    
        do
        {
            Console.Write("Введите две координаты для выстрела (X, Y через пробел): ");
            string coordsInput = Console.ReadLine().Replace(".", ",");
            while (coordsInput == "" || coordsInput.Split().Length != 2)
            {
                Console.Write("Введите две координаты для выстрела (X, Y через пробел): ");
                coordsInput = Console.ReadLine().Replace(".", ",");
            }

            double[] pos = Array.ConvertAll(coordsInput.Split(), Convert.ToDouble);

            string message = Shot(pos) ? "Вы попали в мишень!" : "Вы промахнулись, в следующий раз получится :с";
            
            Console.WriteLine(message);
            
            Console.Write("Хотите выстрелить еще раз? [Да/Нет]: ");
            string isContinueInput = Console.ReadLine().Replace(".", ",");
            
            if (isContinueInput.ToLower() != "да")
            {
                isContinue = false;
            }
        } while (isContinue);
    }

    public static void ThirdTask(double acc, double x)
    {
        double x0, arctg = Math.PI / 2;
        int n = 0;
                    
        if (x > 1)
        {
            do
            {
                x0 = Math.Pow(-1, n + 1) / ((2 * n + 1) * Math.Pow(x, 2 * n + 1));
                arctg += x0;
                n++;
            } while (Math.Abs(x0) > Math.Abs(acc));
            
            Console.WriteLine($"Сумма ряда: {arctg}");
            Console.WriteLine($"Кол-во членов в ряду: {n}");
        }
        else
        {
            Console.WriteLine("X должен быть больше 1.");
        }
    }
    
    public static double Function(double x)
    {
        if (x < -7 || x > 3)
            return double.MinValue;
        if (x <= -6)
            return 2;
        if (x <= -2)
            return 0.25 * x + 0.5;
        if (x <= 0)
            return -Math.Sqrt(-x * (x + 4)) + 2;
        if (x <= 2)
            return Math.Sqrt((2 + x) * (2 - x));
        return -x + 2;
    }
    
    private static bool Shot(double[] pos)
    {
        return pos[1] >= Math.Pow(pos[0] - 2, 2) - 3 && (pos[1] <= pos[0] || (pos[1] <= pos[0] && pos[1] >= 0));
    }
}