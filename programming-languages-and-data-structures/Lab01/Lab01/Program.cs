namespace Lab01;

public static class Program
{
    public static void Main(string[] args)
    {
        Greeting();
        ArrayVector vec = ArrayVector.GetFromUserInput();
        vec.Log("Созданный вектор");
        while (true)
        {
            Console.WriteLine("Выберете действие:\n\n" +
                              "\t1  - Посчитать сумму всех положительных элементов массива с четными номерами\n" +
                              "\t2  - Посчитать сумму элементов массива с нечетным индексом и одновременно меньше среднего значения всех модулей элементов массива\n" +
                              "\t3  - Сортировка по возрастанию\n" +
                              "\t4  - Сортировка по убыванию\n" +
                              "\t5  - Сумма векторов\n" +
                              "\t6  - Скалярное умножение\n" +
                              "\t7  - Умножить вектор на число\n" +
                              "\t8  - Посчитать модуль вектора\n" +
                              "\t9  - Перемножить все четные\n" +
                              "\t10 - Перемножить все нечетные, неделящиеся на 3\n" +
                              "\t11 - Установить элемент вектора\n" +
                              "\t0  - Выход из программы\n");
            
            string inp = Console.ReadLine();

            switch (inp)
            {
                case "1":
                    CountSumPositiveNumbersWithEvenIndex(vec);
                    break;
                case "2":
                    CountSumLessAverageAbsWithOddIndex(vec);
                    break;
                case "3":
                    SortAscending(vec);
                    break;
                case "4":
                    SortDescending(vec);
                    break;
                case "5":
                    SumVectors(vec);
                    break;
                case "6":
                    ScalarMultiply(vec);
                    break;
                case "7":
                    MultiplyVectorByNumber(vec);
                    break;
                case "8":
                    GetVectorNorm(vec);
                    break;
                case "9":
                    MultiplyEven(vec);
                    break;
                case "10":
                    MultiplyOdd(vec);
                    break;
                case "11":
                    SetVectorCoordinate(vec);
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

    public static void CountSumPositiveNumbersWithEvenIndex(ArrayVector vector)
    {
        vector.Log("Созданный вектор");

        int sum = vector.SumPositivesWithEvenIndex();

        if (sum == 0)
        {
            Console.WriteLine("В заданном векторе нет положительных элементов на четных позициях");
        }
        else
        {
            Console.WriteLine("Сумма положительных элементов с четными индексами: " + sum);
        }
    }
    
    public static void CountSumLessAverageAbsWithOddIndex(ArrayVector vector)
    {
        vector.Log("Созданный вектор");

        int sum = vector.SumLessAverageAbsoluteWithOddIndex();
        
        if (sum == 0)
        {
            Console.WriteLine("В заданном векторе нет значений на нечетных позициях, которые меньше среднего значения");
        }
        else
        {
            Console.WriteLine("Сумма элементов с нечетными индексами, которые меньше среднего значения всех модулей: " + sum);
        }
    }

    public static void SortAscending(ArrayVector vector)
    {
        vector.Log("Исходный вектор");
        
        vector.SortUp();
        
        vector.Log("Отсортированный вектор по возрастанию");
    }
    
    public static void SortDescending(ArrayVector vector)
    {
        vector.Log("Исходный вектор");
        
        vector.SortDown();
        
        vector.Log("Отсортированный вектор по убыванию");
    }

    public static void SumVectors(ArrayVector vector)
    {
        ArrayVector vec2 = new ArrayVector();
        
        vector.Log("Первый вектор");
        vec2.Log("Второй вектор");

        try
        {
            ArrayVector result = Vectors.Sum(vector, vec2);
            result.Log("Результирующий векторсложения");
        }
        catch
        {
            Console.WriteLine("Длины векторов не совпадают");
        }
    }

    public static void ScalarMultiply(ArrayVector vector)
    {
        ArrayVector vec2 = new ArrayVector();
        
        vector.Log("Первый вектор");
        vec2.Log("Второй вектор");

        try
        {
            double result = Vectors.ScalarMultiply(vector, vec2);
            Console.WriteLine("Результат скалярного произведения: " + result);
        }
        catch
        {
            Console.WriteLine("Длины векторов не совпадают");
        }
    }

    public static void MultiplyVectorByNumber(ArrayVector vector)
    {
        vector.Log("Созданный вектор");
        
        int number;
        string inp;
        do
        {
            Console.Write("Введите число на которое умножить вектор: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out number));

        ArrayVector result = Vectors.MultiplyByNumber(vector, number);
        
        result.Log("Результат умножения вектора на число");
    }

    public static void GetVectorNorm(ArrayVector vector)
    {
        vector.Log("Созданный вектор");
        
        Console.WriteLine($"Модуль созданного вектора: {vector.GetNorm():0.00}");
    }

    public static void MultiplyEven(ArrayVector vector)
    {
        vector.Log("Созданный вектор");
        
        int result = vector.MultiplyEven();
        if (result == 0)
        {
            Console.WriteLine("В векторе нет положительных четных элементов");
        }
        else
        {
            Console.WriteLine($"Результат умножения всех четных положительных элементов по значению: {result}");
        }
    }
    
    public static void MultiplyOdd(ArrayVector vector)
    {
        vector.Log("Созданный вектор");

        int result = vector.MultiplyOdd();
        if (result == 0)
        {
            Console.WriteLine("В векторе нет нечетных положительных элементов не делящихся на 3");
        }
        else
        {
            Console.WriteLine($"Результат умножения всех нечетных положительных элементов не делящихся на 3: {result}");
        }
    }
    
    public static void SetVectorCoordinate(ArrayVector vector)
    {
        vector.Log("Созданный вектор");

        int idx;
        string inp;
        do
        {
            Console.Write($"Введите номер координаты вектора для установки [1-{vector.Length}]: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out idx) || idx < 1 || idx > vector.Length);

        int value;
        do
        {
            Console.Write($"Введите значение для {{{idx}}}: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out value));

        vector[idx - 1] = value;
        
        vector.Log("Новый вектор");
    }
    
    public static void Greeting()
    {
        Console.WriteLine("Языки Программирования и Структуры Данных\n" +
                          "Лабораторная работа #1\n" +
                          "Выполнил студент группы 6101-020302D - Фадеев Артем");
    }
}