using System.Numerics;

namespace Lab04;

public class Program
{
    public static void Main()
    {
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
}