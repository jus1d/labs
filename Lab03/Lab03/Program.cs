namespace Lab03;

public static class Program
{
    public static void Main(string[] args)
    {
        Greeting();

        while (true)
        {
            Console.WriteLine("Выберете действие:\n\n" +
                              "\t1 - Работа с матрицами\n" +
                              "\t2 - Перевод в двоичную систему\n" +
                              "\t3 - Выйти из программы");

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
                    Console.WriteLine("Нет такого действия");
                    break;
            }
        }
        SecondTask();
    }

    public static void FirstTask()
    {
        while (true)
        {
            int[,] a = new int[,] { };
            int[,] b = new int[,] { };
            int num;
            
            Console.WriteLine("Выберете действие которое хотите совершить над матрицами:\n\n" +
                              "\t1 - Сложение\n" +
                              "\t2 - Вычетание\n" +
                              "\t3 - Умножение\n" +
                              "\t4 - Умножить на число\n" +
                              "\t5 - Сравнить на равенство\n" +
                              "\t6 - Выйти в меню");

            string inp = Console.ReadLine();
            bool matrixSizeEquals;
            
            switch (inp)
            {
                case "1":
                    a = GetMatrixFromUserInput();
                    b = GetMatrixFromUserInput();

                    matrixSizeEquals = CheckMatrixSize(a, b);
                    if (!matrixSizeEquals)
                    {
                        Console.WriteLine("Матрицы должны быть одинакового размера");
                        break;
                    }
                    
                    LogMatrix(AddMatrices(a, b), "Результирующая матрица: ");
                    break;
                case "2":
                    a = GetMatrixFromUserInput();
                    b = GetMatrixFromUserInput();
                    
                    matrixSizeEquals = CheckMatrixSize(a, b);
                    if (!matrixSizeEquals)
                    {
                        Console.WriteLine("Матрицы должны быть одинакового размера");
                        break;
                    }
                    
                    LogMatrix(SubtractMatrices(a, b), "Результирующая матрица: ");
                    break;
                case "3":
                    a = GetMatrixFromUserInput();
                    b = GetMatrixFromUserInput();
                    
                    matrixSizeEquals = CheckMatrixSizeForMultiplication(a, b);
                    if (!matrixSizeEquals)
                    {
                        Console.WriteLine("Для умножения прямоугольных матриц, они должны быть размера: a: NxK, b: KxM");
                        break;
                    }

                    try
                    {
                        var result = MultiplyMatrices(a, b);
                        LogMatrix(result, "Результирующая матрица: ");
                    }
                    catch
                    {
                        Console.WriteLine("Некорректный размер матриц для перемножения\n" +
                                          "Размеры должны быть: a: NxK, b: KxM");
                    }
                    break;
                case "4":
                    a = GetMatrixFromUserInput();
                    Console.Write("Введите число на которое нужно умножить матрицу: ");
                    num = Convert.ToInt32(Console.ReadLine());
                    
                    LogMatrix(MultiplyMatrixBy(a, num), "Результирующая матрица: ");
                    break;
                case "5":
                    a = GetMatrixFromUserInput();
                    b = GetMatrixFromUserInput();

                    string message = CompareMatrices(a, b) ? "Матрицы которые Вы ввели равны" : "Матрицы которые Вы ввели не равны";
                    
                    Console.WriteLine(message);
                    
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Нет такого пункта");
                    break;
            }
        }
    }

    public static void SecondTask()
    {
        Console.Write("Введите число для преобразования в 2-ичную систему счисления: ");
        int n = Convert.ToInt32(Console.ReadLine());
        int replaced = ReplaceFirstAndThirdTriads(n);
        Console.WriteLine("(1) - переаод из десятичной системы в двоичную систему\n" +
                          "(2) - замена первой и третьей триады\n" +
                          "(3) - перевод из двоичной системы в десятичную");
        Console.WriteLine($"{n} -(1)-> {Convert.ToString(n, 2)} -(2)-> {Convert.ToString(replaced, 2)} -(3)-> {replaced}");
    }

    public static int ReplaceFirstAndThirdTriads(int number)
    {
        int firstTriad = (number & 0b111000000) >> 6;
        int thirdTriad = (number & 0b111) << 6;

        number &= ~0b111000111;

        number |= firstTriad | thirdTriad;

        return number;
    }

    public static int[,] AddMatrices(int[,] a, int[,] b)
    {
        int rows = a.GetLength(0);
        int cols = a.GetLength(1);

        int[,] result = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = a[i, j] + b[i, j];
            }
        }

        return result;
    }

    public static int[,] SubtractMatrices(int[,] a, int[,] b)
    {
        int rows = a.GetLength(0);
        int cols = a.GetLength(1);

        int[,] result = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = a[i, j] - b[i, j];
            }
        }

        return result;
    }

    public static int[,] MultiplyMatrices(int[,] a, int[,] b) // a: NxK, b: KxM
    {
        if (a.GetLength(1) != b.GetLength(0))
        {
            throw new Exception("Некорректный размер матриц, размеры двух матриц должны быть a: NxK, b: KxM");
        }
        int n = a.GetLength(0);
        int k = a.GetLength(1);
        int m = b.GetLength(1);

        int[,] result = new int[n, m];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                for (int l = 0; l < k; l++)
                {
                    result[i, j] += a[i, l] * b[l, j];
                }
            }
        }

        return result;
    }

    public static int[,] MultiplyMatrixBy(int[,] a, int b)
    {
        int rows = a.GetLength(0);
        int cols = a.GetLength(1);

        int[,] result = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = a[i, j] + b;
            }
        }

        return result;
    }

    public static bool CompareMatrices(int[,] a, int[,] b)
    {
        int rowsA = a.GetLength(0);
        int colsA = a.GetLength(1);
        int rowsB = b.GetLength(0);
        int colsB = b.GetLength(1);

        if (rowsA != rowsB || colsA != colsB)
        {
            return false;
        }

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsA; j++)
            {
                if (a[i, j] != b[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool CheckMatrixSize(int[,] a, int[,] b)
    {
        if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
        {
            return false;
        }

        return true;
    }
    
    public static bool CheckMatrixSizeForMultiplication(int[,] a, int[,] b)
    {
        return a.GetLength(0) != b.GetLength(1);
    }

    public static void LogMatrix(int[,] matrix, string message = "Матрица: ")
    {
        Console.WriteLine(message);
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i, j].ToString().PadLeft(4));
            }
            Console.WriteLine();
        }
    }

    public static int[,] GetMatrixFromUserInput()
    {
        Console.Write("Введите размер матрицы (NxM): ");
        int n, m;

        string inp = Console.ReadLine();

        while (inp == "" || !inp.Contains("x") || inp.Length < 3)
        {
            Console.Write("Введите размер матрицы (NxM): ");
            inp = Console.ReadLine();
        }
        
        n = Convert.ToInt32(inp.Split("x")[0]);
        m = Convert.ToInt32(inp.Split("x")[1]);
        
        int[,] matrix = new int[n, m];

        for (int i = 0; i < n; i++)
        {
            Console.Write($"Введите {i + 1} строку состоящую их {m} элементов: ");
            string[] values = Console.ReadLine().Trim().Split(' ');

            if (values.Length != m)
            {
                Console.WriteLine($"Некорректное количество элементов в строке матрицы, их должно быть {n}");
                i--;
                continue;
            }

            for (int j = 0; j < m; j++)
            {
                matrix[i, j] = Convert.ToInt32(values[j]);
            }
        }
        
        LogMatrix(matrix, "Вы ввели матрицу: ");

        return matrix;
    }

    public static void Greeting()
    {
        Console.WriteLine("Лабораторная работа №3\n" +
                          "Выполнил студент группы 6101-020302D - Фадеев Артем");
    }
}