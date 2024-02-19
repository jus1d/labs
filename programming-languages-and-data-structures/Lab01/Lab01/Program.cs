namespace Lab01;

public static class Program
{
    public static void Main(string[] args)
    {
        Greeting();
        while (true)
        {
            Console.WriteLine("Выберете действие:\n\n" +
                              "\t1 - Посчитать сумму всех положительных элементов массива с четными номерами\n" +
                              "\t2 - Посчитать сумму элементов массива с нечетным индексом и одновременно меньше среднего значения всех модулей элементов массива\n" +
                              "\t3 - Сортировка по возрастанию\n" +
                              "\t4 - Сортировка по убыванию\n" +
                              "\t5 - Сумма векторов\n" +
                              "\t6 - Скалярное умножение\n" +
                              "\t7 - Умножить вектор на число\n" +
                              "\t8 - Посчитать модуль вектора\n" +
                              "\t0 - Выход из программы\n");
            
            string? inp = Console.ReadLine();

            switch (inp)
            {
                case "1":
                    CountSumPositiveNumbersWithEvenIndex();
                    break;
                case "2":
                    CountSumLessAverageAbsWithOddIndex();
                    break;
                case "3":
                    SortAscending();
                    break;
                case "4":
                    SortDescending();
                    break;
                case "5":
                    SumVectors();
                    break;
                case "6":
                    ScalarMultiply();
                    break;
                case "7":
                    MultiplyVectorByNumber();
                    break;
                case "8":
                    GetVectorNorm();
                    break;
                case "0":
                    Console.WriteLine("До встречи!");
                    return;
                default:
                    Console.WriteLine("Нет такого пункта в меню");
                    break;
            }
            
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    public static void CountSumPositiveNumbersWithEvenIndex()
    {
        var vector = ArrayVector.GetFromUserInput();
        
        vector.Log("Созданный вектор");

        int sum = vector.SumPositivesWithEvenIndex();
        
        Console.WriteLine("Сумма положительных элементов с четными индексами: " + sum);
    }
    
    public static void CountSumLessAverageAbsWithOddIndex()
    {
        var vector = ArrayVector.GetFromUserInput();
        
        vector.Log("Созданный вектор");

        int sum = vector.SumLessAverageAbsoluteWithOddIndex();
        
        Console.WriteLine("Сумма элементов с нечетными индексами, которые меньше среднего значения всех модулей: " + sum);
    }

    public static void SortAscending()
    {
        var vector = ArrayVector.GetFromUserInput();
        
        vector.Log("Исходный вектор");
        
        vector.SortUp();
        
        vector.Log("Отсортированный вектор по возрастанию");
    }
    
    public static void SortDescending()
    {
        var vector = ArrayVector.GetFromUserInput();
        
        vector.Log("Исходный вектор");
        
        vector.SortDown();
        
        vector.Log("Отсортированный вектор по убыванию");
    }

    public static void SumVectors()
    {
        var vec1 = ArrayVector.GetFromUserInput();
        var vec2 = ArrayVector.GetFromUserInput();
        
        vec1.Log("Первый вектор");
        vec2.Log("Второй вектор");

        try
        {
            var result = Vectors.Sum(vec1, vec2);
            result.Log("Результирующий векторсложения");
        }
        catch
        {
            Console.WriteLine("Длины векторов не совпадают");
        }
    }

    public static void ScalarMultiply()
    {
        var vec1 = ArrayVector.GetFromUserInput();
        var vec2 = ArrayVector.GetFromUserInput();
        
        vec1.Log("Первый вектор");
        vec2.Log("Второй вектор");

        try
        {
            var result = Vectors.ScalarMultiply(vec1, vec2);
            Console.WriteLine("Результат скалярного произведения: " + result);
        }
        catch
        {
            Console.WriteLine("Длины векторов не совпадают");
        }
    }

    public static void MultiplyVectorByNumber()
    {
        var vector = ArrayVector.GetFromUserInput();
        
        vector.Log("Созданный вектор");
        
        int number;
        string inp;
        do
        {
            Console.Write("Введите число на которое умножить вектор: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out number));

        var result = Vectors.MultiplyByNumber(vector, number);
        
        result.Log("Результат умножения вектора на число");
    }

    public static void GetVectorNorm()
    {
        var vector = ArrayVector.GetFromUserInput();
        
        vector.Log("Созданный вектор");
        
        Console.WriteLine($"Модуль созданного вектора: {vector.GetNorm():0.00}");
    }
    
    public static void Greeting()
    {
        Console.WriteLine("Языки Программирования и Структуры Данных\n" +
                          "Лабораторная работа #1\n" +
                          "Выполнил студент группы 6101-020302D - Фадеев Артем");
    }
}