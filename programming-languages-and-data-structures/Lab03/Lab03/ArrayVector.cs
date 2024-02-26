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
            if (idx < 0 || idx >= Length)
            {
                throw new IndexOutOfRangeException("Vector index out of range");
            }
            return vector[idx];
        }
        set
        {
            if (idx < 0 || idx >= Length)
            {
                throw new IndexOutOfRangeException("Vector index out of range");
            }
            vector[idx] = value;
        }
    }

    public int Length => vector.Length;

    public double GetNorm()
    {
        double acc = 0;
        for (int i = 0; i < Length; i++)
        {
            acc += Math.Pow(vector[i], 2);
        }

        return Math.Sqrt(acc);
    }

    public override string ToString()
    {
        string s = $"{Length}: {{";
        for (int i = 0; i < Length; i++)
        {
            if (i == Length - 1) s += vector[i].ToString();
            else s += $"{vector[i]}, ";
        }
        s += "}";
        return s;
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
            for (int i = 0; i < length; i++)
            {
                vec[i] = r.Next(100);
            }

            return vec;
        }
        else
        {
            for (int i = 0; i < length; i++)
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