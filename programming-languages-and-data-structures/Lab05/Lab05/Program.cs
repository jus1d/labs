using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace Lab05;

public static class Program
{
    public static void Main()
    {
        List<IVectorable> vectors = new List<IVectorable>();
        string inp;
        
        LogVectors(vectors);

        while (true)
        {
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
                              "\t12 - Потоки\n" +
                              "\t0  - Выход\n");

            inp = Console.ReadLine();

            switch (inp)
            {
                case "0":
                    Console.WriteLine("Выход из программы...");
                    return;
                case "1":
                {
                    LogVectors(vectors);
                    
                    int firstVectorIdx, secondVectorIdx;
                    
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
                    
                    break;
                }
                case "2":
                {
                    LogVectors(vectors);
                    
                    int firstVectorIdx, secondVectorIdx;

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

                    break;
                }
                case "3":
                {
                    LogVectors(vectors);
                    
                    int idx, value;
                    
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
                    
                    break;
                }
                case "4":
                {
                    LogVectors(vectors);
                    
                    int idx;

                    do
                    {
                        Console.Write("Введите номер вектора для удаления: ");
                        inp = Console.ReadLine();
                    } while (!Int32.TryParse(inp, out idx) || idx < 1 || idx > vectors.Count);

                    double norm = vectors[idx - 1].GetNorm();
                    
                    Console.WriteLine($"Модуль вектора: {norm}");
                    
                    break;
                }
                case "5":
                {
                    vectors.Add(GetVectorFromUserInput());
                    LogVectors(vectors);
                    break;
                }
                case "6":
                {
                    LogVectors(vectors);
                    int idx;

                    do
                    {
                        Console.Write("Введите номер вектора для удаления: ");
                        inp = Console.ReadLine();
                    } while (!Int32.TryParse(inp, out idx) || idx < 1 || idx > vectors.Count);

                    vectors.Remove(vectors[idx - 1]);

                    LogVectors(vectors);

                    break;
                }
                case "7":
                {
                    LogVectors(vectors);
                    int idx;

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
                    
                    LogVectors(vectors);
                    
                    break;
                }
                case "8":
                {
                    LogVectors(vectors);

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
                    
                    break;
                }
                case "9":
                {
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
                    
                    break;
                }
                case "10":
                {
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

                    for (int i = 0; i < vectors.Count; ++i)
                    {
                        IVectorable vec = vectors[i];

                        string typeView = vec is ArrayVector ? "ArrayVector" : "LinkedListVector";
            
                        vec.Log($"{i + 1}: {typeView}, Модуль: {vec.GetNorm():0.00}");
                    }
                    
                    break;
                }
                case "11":
                {
                    int firstIdx, secondIdx;
                    
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
                    break;
                }
                case "12":
                {
                    RunStreamSubmenu(vectors);
                    break;
                }
                default:
                    Console.WriteLine("Нет такого пункта в меню");
                    break;
            }
        }
    }

    public static void RunStreamSubmenu(List<IVectorable> vectors)
    {
        while (true)
        {
            Console.Write("Выберете поток:\n\n" +
                              "\t1. Байтовый поток\n" +
                              "\t2. Символьный поток\n" +
                              "\t3. Сериализация\n" +
                              "\t0. Выход в главное меню\n");

            string inp = Console.ReadLine();

            switch (inp)
            {
                case "0":
                    return;
                case "1":
                    RunByteStream(vectors);
                    break;
                case "2":
                    RunSymbolStream(vectors);
                    break;
                case "3":
                    // RunSerialization(vectors);
                    break;
                default:
                    Console.WriteLine("Нет такого пункта в меню");
                    break;
            }
        }
    }

    public static void RunByteStream(List<IVectorable> vectors)
    {
        string path = "../../../vectors.bin";
        if (File.Exists(path)) File.Delete(path);

        using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
        {
            Vectors.WriteVectors(fs, vectors);
        }
        
        Console.WriteLine("Запись в файл `vectors.bin` выполнена");

        List<IVectorable> vectorsRead = new List<IVectorable>();
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            vectorsRead = Vectors.ReadVectors(fs);
        }
        
        Console.WriteLine("Чтение из файла `vectors.bin` выполнено");
        
        Console.WriteLine("\nИсходный список векторов:");
        LogVectors(vectors);
        Console.WriteLine("\nСписок векторов считанный из файла:");
        LogVectors(vectorsRead);
    }
    
    public static void RunSymbolStream(List<IVectorable> vectors)
    {
        string path = "../../../vectors.txt";
        if (File.Exists(path)) File.Delete(path);
        
        using (TextWriter w = File.AppendText(path))
        {
            for (int i = 0; i < vectors.Count; i++)
            {
                Vectors.WriteVector(w, vectors[i]);
            }
            Console.WriteLine("Запись в файл `vectors.txt` выполнена");
        }

        TextReader r = File.OpenText(path);
        List<IVectorable> vectorsRead = new List<IVectorable>();
        for (int i = 0; i < vectors.Count; i++)
        {
            vectorsRead.Add(Vectors.ReadVector(r));
        }
        r.Close();
        
        Console.WriteLine("Чтение из файла `vectors.txt` выполнено");
        
        Console.WriteLine("\nИсходный список векторов:");
        LogVectors(vectors);
        Console.WriteLine("\nСписок векторов считанный из файла:");
        LogVectors(vectorsRead);
    }
    
    public static void RunSerialization(List<IVectorable> vectors)
    {
        string path = "../../../vectors.txt";
        string serializedPath = "../../../serialized.json";
        List<IVectorable> vectorsRead = new List<IVectorable>();
        
        if (File.Exists(serializedPath)) File.Delete(serializedPath);

        for (int i = 0; i < vectors.Count; i++)
        {
            // Serialize
            string serializedJson = JsonSerializer.Serialize(vectors[i]);
            File.WriteAllText(serializedPath, serializedJson);

            // Deserialize
            string deserializedJson = File.ReadAllText(serializedPath);
            if (vectors[i] is LinkedListVector)
            {
                vectorsRead.Add(JsonSerializer.Deserialize<LinkedListVector>(deserializedJson));
            }
            else
            {
                vectorsRead.Add(JsonSerializer.Deserialize<ArrayVector>(deserializedJson));
            }

            // Logging and comparison
            vectors[i].Log("Ваш вектор");
            vectorsRead[i].Log("Десереализованный вектор");

            if (vectors[i].Equals(vectorsRead[i]))
            {
                Console.WriteLine("Вектора равны");
            }
            else
            {
                Console.WriteLine("Вектора не равны");
            }
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
