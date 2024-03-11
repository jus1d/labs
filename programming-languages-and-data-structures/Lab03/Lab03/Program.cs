namespace Lab03;

public static class Program
{
    public static void Main()
    {
        List<IVectorable> vectors = new List<IVectorable>();
        string inp;
        
        vectors.Add(GetVectorFromUserInput());
        LogVectors(vectors);

        while (true)
        {
            Console.WriteLine("Выберете действие:\n\n" +
                              "\t1 - Сумма векторов\n" +
                              "\t2 - Скалярное умножение\n" +
                              "\t3 - Умножение на число\n" +
                              "\t4 - Рассчитать модуль вектора\n" +
                              "\t5 - Добавить вектор в список\n" +
                              "\t6 - Удалить вектор из списка\n" +
                              "\t0 - Выход\n");

            inp = Console.ReadLine();

            switch (inp)
            {
                case "0":
                    Console.WriteLine("Выход из программы...");
                    return;
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    vectors.Add(GetVectorFromUserInput());
                    LogVectors(vectors);
                    break;
                case "6":
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
                default:
                    Console.WriteLine("Нет такого пункта в меню");
                    break;
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
            vectors[i].Log($"Вектор {i + 1}");
        }
    }
}
