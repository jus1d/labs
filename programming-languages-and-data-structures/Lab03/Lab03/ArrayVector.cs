namespace Lab03;

public class ArrayVector : IVectorable
{
    private int[] vector;
    
    public ArrayVector(int length)
    {
        vector = new int[length];
    }

    public ArrayVector()
    {
        vector = new int[5];
    }
    
    public int this[int idx]
    {
        get
        {
            idx--;
            if (idx < 0 || idx >= Length)
            {
                throw new IndexOutOfRangeException("Индекс за границами вектора");
            }
            return vector[idx];
        }
        set
        {
            idx--;
            if (idx < 0 || idx >= Length)
            {
                throw new IndexOutOfRangeException("Индекс за границами вектора");
            }
            vector[idx] = value;
        }
    }

    public int Length
    {
        get
        {
            return vector.Length;
        }
    }

    public double GetNorm()
    {
        double acc = 0;
        for (int i = 1; i <= Length; i++)
        {
            acc += Math.Pow(this[i], 2);
        }

        return Math.Sqrt(acc);
    }

    public override string ToString()
    {
        string s = $"{Length}: {{";

        for (int i = 1; i <= Length; i++)
        {
            if (i == Length) s += this[i].ToString();
            else s += $"{this[i]}, ";
        }
        s += "}";
        
        return s;
    }

    public int SumPositivesWithEvenIndex()
    {
        int acc = 0;
        for (int i = 2; i <= Length; i += 2)
        {
            if (this[i] > 0)
            {
                acc += this[i];
            }
        }

        return acc;
    }

    public int SumLessAverageAbsoluteWithOddIndex()
    {
        if (Length == 0) return 0;
        
        int average = 0;
        for (int i = 1; i <= Length; i++)
        {
            average += Math.Abs(this[i]);
        }

        average /= Length;
        
        
        int acc = 0;
        for (int i = 1; i <= Length; i += 2)
        {
            if (this[i] < average)
            {
                acc += this[i];
            }
        }

        return acc;
    }

    public int MultiplyEven()
    {
        int result = 0;
        for (int i = 1; i <= Length; i+=2)
        {
            if (this[i] > 0 && this[i] % 2 == 0)
            {
                if (result == 0) result = 1;
                result *= this[i];
            }
        }

        return result;
    }
    
    public int MultiplyOdd()
    {
        int result = 0;
        for (int i = 1; i <= Length; i+=2)
        {
            if (this[i] % 2 != 0 && this[i] % 3 != 0)
            {
                if (result == 0) result = 1;
                result *= this[i];
            }
        }

        return result;
    }

    public void SortUp()
    {
        int n = Length;
        for (int i = 1; i <= n - 1; i++)
        {
            for (int j = 1; j <= n - i - 1; j++)
            {
                if (this[j] > this[j + 1])
                {
                    (this[j], this[j + 1]) = (this[j + 1], this[j]);
                }
            }
        }
    }

    public void SortDown()
    {
        int n = Length;
        for (int i = 1; i <= n - 1; i++)
        {
            for (int j = 1; j <= n - i - 1; j++)
            {
                if (this[j] < this[j + 1])
                {
                    (this[j], this[j + 1]) = (this[j + 1], this[j]);
                }
            }
        }
    }

    public void Log(string message = "")
    {
        if (message != "")
        {
            Console.Write(message + ": ");
        }
        Console.Write("{");
        for (int i = 1; i <= Length; i++)
        {
            if (i == Length - 1)
            {
                Console.Write(this[i]);
            }
            else
            {
                Console.Write(this[i] + ", ");
            }
        }
        Console.WriteLine("}");
    }

    public static ArrayVector GetFromUserInput()
    {
        string inp;
        do
        {
            Console.WriteLine("Выберете как хотите заполнить вектор:\n" +
                              "1 - Случайно\n" +
                              "2 - Ручной ввод");
            inp = Console.ReadLine();
        } while (inp != "1" && inp != "2");

        int length;
        do
        {
            Console.Write("Введите длину вектора: ");
        } while (!int.TryParse(Console.ReadLine(), out length) || length <= 0);

        ArrayVector vec = new ArrayVector(length);

        if (inp == "1")
        {
            Random r = new Random();
            for (int i = 1; i <= length; i++)
            {
                vec[i] = r.Next(100);
            }

            return vec;
        }
        else
        {
            for (int i = 1; i <= length; i++)
            {
                int value;
                do
                {
                    Console.Write($"Введите значение координаты {{{i+1}}}: ");
                    inp = Console.ReadLine();
                } while (!int.TryParse(inp, out value));

                vec[i] = value;
            }

            return vec;
        }
    }
}