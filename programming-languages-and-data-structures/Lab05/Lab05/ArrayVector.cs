namespace Lab05;

[Serializable]
public class ArrayVector : IVectorable, IComparable, ICloneable
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
                throw new IndexOutOfRangeException("Индекс за границами вектора");
            }
            return vector[idx];
        }
        set
        {
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
        for (int i = 0; i < Length; i++)
        {
            acc += Math.Pow(vector[i], 2);
        }

        return Math.Sqrt(acc);
    }

    public int SumPositivesWithEvenIndex()
    {
        int acc = 0;
        for (int i = 1; i < Length; i += 2)
        {
            if (vector[i] > 0)
            {
                acc += vector[i];
            }
        }

        return acc;
    }

    public int SumLessAverageAbsoluteWithOddIndex()
    {
        if (Length == 0)
        {
            return 0;
        }
        
        int average = 0;
        for (int i = 0; i < Length; i++)
        {
            average += Math.Abs(vector[i]);
        }

        average /= Length;
        
        
        int acc = 0;
        for (int i = 0; i < Length; i += 2)
        {
            if (vector[i] < average)
            {
                acc += vector[i];
            }
        }

        return acc;
    }

    public int MultiplyEven()
    {
        int result = 0;
        for (int i = 1; i < Length; i+=2)
        {
            if (vector[i] > 0 && vector[i] % 2 == 0)
            {
                if (result == 0) result = 1;
                result *= vector[i];
            }
        }

        return result;
    }
    
    public int MultiplyOdd()
    {
        int result = 0;
        for (int i = 0; i < Length; i+=2)
        {
            if (vector[i] % 2 != 0 && vector[i] % 3 != 0)
            {
                if (result == 0) result = 1;
                result *= vector[i];
            }
        }

        return result;
    }

    public void SortUp()
    {
        int n = Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (vector[j] > vector[j + 1])
                {
                    (vector[j], vector[j + 1]) = (vector[j + 1], vector[j]);
                }
            }
        }
    }

    public void SortDown()
    {
        int n = Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (vector[j] < vector[j + 1])
                {
                    (vector[j], vector[j + 1]) = (vector[j + 1], vector[j]);
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
        Console.WriteLine(ToString());
    }
    
    public override string ToString()
    {
        string s = Length.ToString() + ' ';

        for (int i = 0; i < Length; i++)
        {
            s += this[i].ToString();
            if (i != Length - 1) s += ' ';
        }
        return s;
    }

    public static ArrayVector GetFromUserInput()
    {

        int length;
        do
        {
            Console.Write("Введите длину вектора: ");
        } while (!int.TryParse(Console.ReadLine(), out length) || length <= 0);
        
        ArrayVector vec = new ArrayVector(length);
        
        Random r = new Random();
        for (int i = 0; i < length; i++)
        {
            vec[i] = r.Next(100);
        }

        return vec;
    }
    
    public int CompareTo(object? obj)
    {
        if (!(obj is IVectorable))
        {
            throw new Exception("Можно сравнить только объекты типа IVectorable");
        }
        
        IVectorable other = obj as IVectorable;

        if (Length < other.Length) return -1;
        if (Length > other.Length) return 1;
        return 0;
    }
    
    public override bool Equals(object? obj)
    {
        if (!(obj is IVectorable))
        {
            throw new Exception("Можно сравнивать только объекты типа IVectorable");
        }
        
        IVectorable other = obj as IVectorable;

        if (Length != other.Length) return false;

        for (int i = 0; i < Length; i++)
        {
            if (this[i] != other[i]) return false;
        }

        return true;
    }

    public object Clone()
    {
        ArrayVector clone = new ArrayVector(Length);
        
        for (int i = 0; i < Length; i++)
        {
            clone[i] = this[i];
        }

        return clone;
    }
}