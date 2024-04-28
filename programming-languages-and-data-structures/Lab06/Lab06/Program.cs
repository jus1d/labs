using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace Lab06;

public static class Program
{
    delegate void Action(List<IVectorable> vectors);
    
    public static void Main()
    {
        List<IVectorable> vectors = new List<IVectorable>();
        string inp;
        
        Console.WriteLine("Выберете действие:\n\n" +
                          "\t1  - Сумма векторов\n" +
                          "\t2  - Скалярное умножение\n" +
                          "\t3  - Умножение на число\n" +
                          "\t4  - Рассчитать модуль вектора\n" +
                          "\t5  - Добавить вектор в список\n" +
                          "\t6  - Удалить вектор из списка\n" +
                          "\t7  - Клонировать вектор\n" +
                          "\t8  - Вывести вектора с минимальным и максимальным количеством координат\n" +
                          "\t9  - Отсортировать вектора по количеству координат\n" +
                          "\t10 - Отсортировать вектора по модулю\n" +
                          "\t11 - Сравнить вектора\n" +
                          "\t0  - Выход\n");
        
        Console.Write("Введите траекторию выполнения программы через пробел: ");
        string[] commands = Console.ReadLine().Split(new[] { ',', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);

        Action execution = null;

        foreach (string command in commands)
        {
            switch (command)
            {
                case "0":
                    execution += Exit;
                    return;
                case "1":
                {
                    execution += LogVectors;
                    execution += SumTwoVectors;
                    break;
                }
                case "2":
                {
                    execution += LogVectors;
                    execution += MultiplyTwoVectors;
                    break;
                }
                case "3":
                {
                    execution += LogVectors;
                    execution += MultiplyVectorByNumber;
                    break;
                }
                case "4":
                {
                    execution += LogVectors;
                    execution += CalculateVectorNorm;
                    break;
                }
                case "5":
                {
                    execution += AddVector;
                    execution += LogVectors;
                    break;
                }
                case "6":
                {
                    execution += LogVectors;
                    execution += RemoveVector;
                    execution += LogVectors;
                    break;
                }
                case "7":
                {
                    execution += LogVectors;
                    execution += CloneVector;
                    execution += LogVectors;
                    break;
                }
                case "8":
                {
                    execution += LogVectors;
                    execution += LogLongestAndShortestVectors;
                    break;
                }
                case "9":
                {
                    execution += SortVectorsByLength;
                    break;
                }
                case "10":
                {
                    execution += SortVectorsByNorm;
                    break;
                }
                case "11":
                {
                    execution += CompareVectors;
                    execution += CompareVectors;
                    break;
                }
                default:
                    Console.WriteLine($"Пункт меню не распознан: {command}");
                    break;
            }
        }

        execution += LogVectors;

        execution(vectors);
        Console.WriteLine("Прорамма завершила работу . . .");
    }

    public static void Exit(List<IVectorable> vectors)
    {
        Console.WriteLine("Выход из программы . . .");
        Environment.Exit(0);
    }

    public static void SumTwoVectors(List<IVectorable> vectors)
    {
        if (vectors.Count == 0)
        {
            Console.WriteLine("Список векторов пустой");
            return;
        }
        
        int firstVectorIdx, secondVectorIdx;
        string inp;
        
        do
        {
            Console.Write("Введите индекс первого вектора: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out firstVectorIdx) ||  firstVectorIdx <= 0 || firstVectorIdx > vectors.Count);
                    
        do
        {
            Console.Write("Введите индекс второго вектора: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out secondVectorIdx) ||  secondVectorIdx <= 0 || secondVectorIdx > vectors.Count);

        try
        {
            var result = Vectors.Sum(vectors[firstVectorIdx - 1], vectors[secondVectorIdx - 1]);
            result.Log($"Результат сложения {firstVectorIdx}-го и {secondVectorIdx}-го векторов");
        }
        catch (Exception e)
        {
            Console.WriteLine("Длины векторов не совпадают");
        }
    }

    public static void MultiplyTwoVectors(List<IVectorable> vectors)
    {
        if (vectors.Count == 0)
        {
            Console.WriteLine("Список векторов пустой");
            return;
        }
        
        int firstVectorIdx, secondVectorIdx;
        string inp;
        
        do
        {
            Console.Write("Введите индекс первого вектора: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out firstVectorIdx) || firstVectorIdx <= 0 || firstVectorIdx > vectors.Count);

        do
        {
            Console.Write("Введите индекс второго вектора: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out secondVectorIdx) || secondVectorIdx <= 0 || secondVectorIdx > vectors.Count);
                    
        try
        {
            var result = Vectors.ScalarMultiply(vectors[firstVectorIdx - 1], vectors[secondVectorIdx - 1]);
            Console.WriteLine($"Результат скалярного умножения: {result}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Длины векторов не совпадают");
        }
    }

    public static void MultiplyVectorByNumber(List<IVectorable> vectors)
    {
        if (vectors.Count == 0)
        {
            Console.WriteLine("Список векторов пустой");
            return;
        }
        
        int idx, value;
        string inp;
        
        do
        {
            Console.Write("Введите индекс вектора: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out idx) ||  idx <= 0 || idx > vectors.Count);

        do
        {
            Console.Write("Введите значение на которое умножить число: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out value));

        var result = Vectors.MultiplyByNumber(vectors[idx - 1], value);
        result.Log($"Результат сложения умножения вектора на число");
    }

    public static void CalculateVectorNorm(List<IVectorable> vectors)
    {
        if (vectors.Count == 0)
        {
            Console.WriteLine("Список векторов пустой");
            return;
        }
        
        int idx;
        string inp;
        
        do
        {
            Console.Write("Введите номер вектора для удаления: ");
            inp = Console.ReadLine();
        } while (!Int32.TryParse(inp, out idx) || idx < 1 || idx > vectors.Count);

        double norm = vectors[idx - 1].GetNorm();
                    
        Console.WriteLine($"Модуль вектора: {norm}");
    }

    public static void AddVector(List<IVectorable> vectors)
    {
        vectors.Add(GetVectorFromUserInput());
    }

    public static void RemoveVector(List<IVectorable> vectors)
    {
        if (vectors.Count == 0)
        {
            Console.WriteLine("Список векторов пустой");
            return;
        }
        
        int idx;
        string inp;
        
        do
        {
            Console.Write("Введите номер вектора для удаления: ");
            inp = Console.ReadLine();
        } while (!Int32.TryParse(inp, out idx) || idx < 1 || idx > vectors.Count);

        vectors.Remove(vectors[idx - 1]);
    }

    public static void CloneVector(List<IVectorable> vectors)
    {
        if (vectors.Count == 0)
        {
            Console.WriteLine("Список векторов пустой");
            return;
        }
        
        int idx;
        string inp;
        
        do
        {
            Console.Write("Введите номер вектора для клонирования: ");
            inp = Console.ReadLine();
        } while (!Int32.TryParse(inp, out idx) || idx < 1 || idx > vectors.Count);

        var vec = vectors[idx - 1];
        IVectorable clone;
        if (vec is ArrayVector)
        {
            clone = (vec as ArrayVector).Clone() as IVectorable;
        }
        else
        {
            clone = (vec as LinkedListVector).Clone() as IVectorable;
        }
                    
        vectors.Add(clone);
    }

    public static void LogLongestAndShortestVectors(List<IVectorable> vectors)
    {
        if (vectors.Count == 0)
        {
            Console.WriteLine("Список векторов пустой");
            return;
        }
        
        try
        {
            int minLength = vectors[0].Length;
            int maxLength = vectors[0].Length;

            for (int i = 0; i < vectors.Count; i++)
            {
                if (minLength > vectors[i].Length)
                {
                    minLength = vectors[i].Length;
                }

                if (maxLength < vectors[i].Length)
                {
                    maxLength = vectors[i].Length;
                }
            }

            string type;
            Console.WriteLine("Вектора с минимальным значением координат: ");
            for (int i = 0; i < vectors.Count; i++)
            {
                if (vectors[i].Length == minLength)
                {
                    type = vectors[i] is ArrayVector ? "ArrayVector" : "LinkedListVector";
                    Console.WriteLine($"{type}: {vectors[i].ToString()}");
                }
            }

            Console.WriteLine("Вектора с максимальным значением координат: ");
            for (int i = 0; i < vectors.Count; i++)
            {
                if (vectors[i].Length == maxLength)
                {
                    type = vectors[i] is ArrayVector ? "ArrayVector" : "LinkedListVector";
                    Console.WriteLine($"{type}: {vectors[i].ToString()}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Нет векторов");
        }
    }

    public static void SortVectorsByLength(List<IVectorable> vectors)
    {
        if (vectors.Count == 0)
        {
            Console.WriteLine("Список векторов пустой");
            return;
        }
        
        IVectorable tmp;
        VectorsComparer comparer = new VectorsComparer();
                    
                    
        for (int i = 0; i < vectors.Count - 1; i++)
        {
            for (int j = i + 1; j < vectors.Count; j++)
            {
                int compareResult;
                if (vectors[i] is ArrayVector)
                {
                    compareResult = (vectors[i] as ArrayVector).CompareTo(vectors[j]);
                }
                else
                {
                    compareResult = (vectors[i] as LinkedListVector).CompareTo(vectors[j]);
                }
                            
                if (compareResult > 0)
                {
                    tmp = vectors[i];
                    vectors[i] = vectors[j];
                    vectors[j] = tmp;
                }
            }
        }
                    
        Console.WriteLine("Список векторов после сортировки по длине: ");
        LogVectors(vectors);
    }

    public static void SortVectorsByNorm(List<IVectorable> vectors)
    {
        if (vectors.Count == 0)
        {
            Console.WriteLine("Список векторов пустой");
            return;
        }
        
        IVectorable tmp;
        VectorsComparer comparer = new VectorsComparer();

        for (int i = 0; i < vectors.Count - 1; i++)
        {
            for (int j = i + 1; j < vectors.Count; j++)
            {
                if (comparer.Compare(vectors[i], vectors[j]) > 0)
                {
                    tmp = vectors[j];
                    vectors[j] = vectors[i];
                    vectors[i] = tmp;
                }
            }
        }
                    
        Console.WriteLine("Список векторов после сортировки по модулю: ");
        LogVectors(vectors);
    }

    public static void CompareVectors(List<IVectorable> vectors)
    {
        if (vectors.Count == 0)
        {
            Console.WriteLine("Список векторов пустой");
            return;
        }
        
        int firstIdx, secondIdx;
        string inp;
        
        do
        {
            Console.Write("Введите индекс первого вектора: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out firstIdx) ||  firstIdx <= 0 || firstIdx > vectors.Count);
                    
        do
        {
            Console.Write("Введите индекс второго вектора: ");
            inp = Console.ReadLine();
        } while (!int.TryParse(inp, out secondIdx) ||  secondIdx <= 0 || secondIdx > vectors.Count);

        if (vectors[firstIdx-1].Equals(vectors[secondIdx-1]))
        {
            Console.WriteLine("Вектора равны");
        }
        else
        {
            Console.WriteLine("Вектора не равны");
        }
    }

    public static IVectorable GetVectorFromUserInput()
    {
        IVectorable vec;
        string inp;
        do
        {
            Console.WriteLine("Выберете тип вектора:\n\n" +
                              "\t1 - Вектор\n" +
                              "\t2 - Связный список\n");
            inp = Console.ReadLine();
        } while (inp != "1" && inp != "2");

        if (inp == "1")
        {
            vec = ArrayVector.GetFromUserInput();
        }
        else
        {
            int length;
            do
            {
                Console.Write("Введите длину связного списка: ");
                inp = Console.ReadLine();
            } while (!Int32.TryParse(inp, out length));
                
            vec = new LinkedListVector(length);
        }
        
        return vec;
    }

    public static void LogVectors(List<IVectorable> vectors)
    {
        for (int i = 0; i < vectors.Count; ++i)
        {
            IVectorable vec = vectors[i];

            string typeView = vec is ArrayVector ? "Array" : "LinkedList";
            
            vec.Log($"{i + 1}: {typeView, 10}");
        }
    }
}
