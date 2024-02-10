namespace Lab01;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Лабораторная работа #1. Выполнил Фадеев Артем. Группа - 6101-020302D\n");
        
        // Задание 1
        Console.Write("Задание 1\nВведите значение для вычисления по формуле: ");
        double a = Convert.ToDouble(Console.ReadLine().Replace(".", ","));
        FirstTask(a);
        
        // Задание 2
        Console.Write("Задание 2\nВведите координату X для определение функции: ");
        double b = Convert.ToDouble(Console.ReadLine().Replace(".", ","));
        SecondTask(b);
        
        // Задание 3
        Console.Write("Задание 3\nВведите 2 координаты для выстрела (X, Y через пробел): ");
        string input = Console.ReadLine().Replace(".", ",");
        while (input == null || input.Split().Length != 2)
        {
            Console.Write("Вы должны ввести 2 координаты (через пробел)\nВведите еще раз: ");
            input = Console.ReadLine().Replace(".", ",");
        }
        
        double[] pos = Array.ConvertAll(input.Split(), Convert.ToDouble);
        bool thirdTaskResult = ThirdTask(pos);
        string resultMessage = thirdTaskResult ? $"Вы попали" : $"Вы промахнулись, в следующий раз повезет :c";
        Console.WriteLine(resultMessage);
    }

    private static void FirstTask(double a)
    {
        if (2 * a + Math.Pow(a, 2) == 0 || 2 * a - Math.Pow(a, 2) == 0)
        {
            Console.WriteLine("Деление на 0 запрещено");
            return;
        }
        
        double z1, z2;
        z1 = Math.Pow((1 + a + Math.Pow(a, 2)) / (2 * a + Math.Pow(a, 2)) + 2 - (1 - a + Math.Pow(a, 2)) / (2 * a - Math.Pow(a, 2)), -1) * (5 - 2 * Math.Pow(a, 2));
        z2 = (4 - Math.Pow(a, 2)) / 2;
        
        Console.WriteLine($"z1 = {z1}, z2 = {z1}");
    }

    private static void SecondTask(double x)
    {
        if (x < -7 || x > 3)
            Console.WriteLine("Функция не определена");
        else if (x <= -6)
            Console.WriteLine($"f(x) = {2}");
        else if (x <= -2)
            Console.WriteLine($"f(x) = {0.25 * x + 0.5}");
        else if (x <= 0)
            Console.WriteLine($"f(x) = {-Math.Sqrt(-x * (x + 4)) + 2}");
        else if (x <= 2)
            Console.WriteLine($"f(x) = {Math.Sqrt((2 + x) * (2 - x))}");
        else
            Console.WriteLine($"f(x) = {-x + 2}");
    }

    private static bool ThirdTask(double[] pos)
    {
        return pos[1] >= Math.Pow(pos[0] - 2, 2) - 3 && (pos[1] <= pos[0] || (pos[1] <= pos[0] && pos[1] >= 0));
    }
}