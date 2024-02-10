namespace Lab06;

public static class Program
{
    public static void Main(string[] args)
    {
        Greeting();

        while (true)
        {
            Console.WriteLine("Выберете класс для работы:\n\n" +
                              "\t1 - Сортировка\n" +
                              "\t2 - Мой массив\n" +
                              "\t3 - Ступенчатый массив\n" +
                              "\t4 - Сравнить все методы сортировок\n" +
                              "\t5 - Выход из программы");

            string inp = Console.ReadLine();

            switch (inp)
            {
                case "1":
                    TestSorting();
                    break;
                case "2":
                    TestMyArray();
                    break;
                case "3":
                    TestJaggedArray();
                    break;
                case "4":
                    SortingCompare();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Нет такого класса");
                    break;
            }
        }
    }

    public static void TestSorting()
    {
        Console.WriteLine("Введите 1 - если хотите заполнить массив самостоятельно, 2 - если хотите использовать случайный массив");
        string inp = Console.ReadLine(); 
        while (!"12".Contains(inp)) 
        { 
            Console.WriteLine("Нет такого способа заполнения, введите 1 или 2"); 
            inp = Console.ReadLine();
        }
        int[] arr; 
        if (inp == "1") 
        { 
            Console.Write("Введите массив: "); 
            inp = Console.ReadLine(); 
            arr = Array.ConvertAll(inp.Split(" "), Convert.ToInt32);
        }
        
        else 
        { 
            arr = Sorting.CreateRandomArray(10, -10, 10);
        }
                    
        Console.Write("Ваш исходный массив: "); 
        Sorting.LogArray(arr);

        Console.WriteLine("Выберете метод сортировки:\n\n" +
                          "\t1 - Сортировка Шелла\n" +
                          "\t2 - Сортировка пузырьком\n" +
                          "\t3 - Сортировка перемешиванием\n" +
                          "\t4 - Сортировка вставкой\n");
        inp = Console.ReadLine();
        while (!"1234".Contains(inp))
        {
            Console.WriteLine("Нет такого метода сортировки, выберете из существующий пунктов меню");
            inp = Console.ReadLine();
        }

        switch (inp)
        {
            case "1":
                Sorting.Shell(arr);
                break;
            case "2":
                Sorting.Bubble(arr);
                break;
            case "3":
                Sorting.Shake(arr);
                break;
            case "4":
                Sorting.Insert(arr);
                break;
        }
                    
        Console.Write("Отсортированный массив: ");
        Sorting.LogArray(arr);
    }

    public static void TestMyArray()
    {
        string inp;
        int[] size;
        do
        {
            Console.Write("Введите размер массива в формате MxN (M, N > 0): ");
            inp = Console.ReadLine();
            size = Array.ConvertAll(inp.Split("x"), Convert.ToInt32);
        } while (size[0] < 1 || size[1] < 1);
        
        var arr = new MyArray(size[0], size[1]);
        
        Console.WriteLine("Исходный массив: ");
        arr.Log();
        Console.WriteLine("В каком порядке вы хотите отсортировать массив?\n\n" +
                          "\t1 - В порядке возрастания суммы столбцов\n" +
                          "\t2 - В порядке убывания суммы столбцов");

        inp = Console.ReadLine();

        while (!"1 2".Split(" ").Contains(inp))
        {
            Console.WriteLine("Нет такого пункта меню\n" +
                              "Введите число 1 или 2 для продолжения");
            inp = Console.ReadLine();
        }

        if (inp == "1")
        {
            arr.SortColumnsAscending();
            Console.WriteLine("Массив отсортированный в порядке возрастания суммы столбцов:");
        }
        else
        {
            arr.SortColumnsDescending();
            Console.WriteLine("Массив отсортированный в порядке убывания суммы столбцов:");
        }
        
        arr.Log();
    }

    public static void TestJaggedArray()
    {
        int length;
        do
        {
            Console.Write("Какой длины вы хотите ступенчатый массив? (Длина должна быть больше 0) ");
            length = Convert.ToInt32(Console.ReadLine());
        } while (length < 1);

        var arr = new JaggedArray(length);
        
        Console.WriteLine("Введите 1 - если хотите исопльзовать случайный массив, 2 - если хотите задать собственный");
        string inp = Console.ReadLine();
        while (!"1 2".Split(" ").Contains(inp))
        {
            Console.WriteLine("Нет такого варианта заполнения");
            inp = Console.ReadLine();
        }

        if (inp == "1")
        {
            arr.FillWithRandomData();
        }
        else
        {
            arr.FillArrayFromUserInput();
        }
        
        Console.WriteLine("Ваш исходный массив: ");
        arr.Log();
        
        arr.Sort();
        Console.WriteLine("Отсортированный массив: ");
        arr.Log();
    }

    delegate int[] SortingFunction(int[] a);
    
    public static void SortingCompare()
    {
        var size = new int[] { 1000, 10000, 50000, 100000 };
        SortingFunction[] sortings = { Sorting.Bubble, Sorting.Insert, Sorting.Shell, Sorting.Shake };

        Console.WriteLine("Size\t\tBubble\t\tInsert\t\t Shell\t\t Shake");
        for (int i = 0; i < size.Length; i++)
        {
            Console.Write(size[i] + "\t");
            for (int j = 0; j < sortings.Length; j++)
            {
                int[] array = Sorting.CreateRandomArray(size[i], -1000, 1000);
                DateTime start = DateTime.Now;
                array = sortings[j](array);
                TimeSpan diff = DateTime.Now - start;
                
                Console.Write($"{diff.TotalMicroseconds,12}μs\t");
            }
            Console.WriteLine();
        }
    }
    
    public static void Greeting()
    {
        Console.WriteLine("Лабораторная работа #6\n" +
                          "Выполнил студент группы 6101-020302D - Фадеев Артем");
    }
}