namespace Lab01;

public class ArrayVector
{
    private int[] _vector;
    
    public int this[int idx]
    {
        get
        {
            if (idx < 0 || idx >= _vector.Length)
            {
                throw new IndexOutOfRangeException("Vector index out of range");
            }
            return _vector[idx];
        }
        set
        {
            if (idx < 0 || idx >= _vector.Length)
            {
                throw new IndexOutOfRangeException("Vector index out of range");
            }
            _vector[idx] = value;
        }
    }

    public int Length => _vector.Length;

    public ArrayVector(int length)
    {
        _vector = new int[length];
    }

    public ArrayVector()
    {
        _vector = new int[5];
    }

    public double GetNorm()
    {
        double acc = 0;
        for (int i = 0; i < _vector.Length; i++)
        {
            acc += Math.Pow(_vector[i], 2);
        }

        return Math.Sqrt(acc);
    }

    public int SumPositivesWithEvenIndex()
    {
        int acc = 0;
        for (int i = 0; i < _vector.Length; i += 2)
        {
            if (_vector[i] > 0)
            {
                acc += _vector[i];
            }
        }

        return acc;
    }

    public int SumLessAverageAbsoluteWithOddIndex()
    {
        if (_vector.Length == 0)
        {
            return 0;
        }
        
        int average = 0;
        for (int i = 0; i < _vector.Length; i++)
        {
            average += Math.Abs(_vector[i]);
        }

        average /= _vector.Length;
        
        
        int acc = 0;
        for (int i = 1; i < _vector.Length; i += 2)
        {
            if (_vector[i] < average)
            {
                acc += _vector[i];
            }
        }

        return acc;
    }

    public int MultiplyEven()
    {
        int result = 1;
        for (int i = 0; i < _vector.Length; i++)
        {
            if (_vector[i] > 0 && _vector[i] % 2 == 0)
            {
                result *= _vector[i];
            }
        }

        return result;
    }
    
    public int MultiplyOdd()
    {
        int result = 1;
        for (int i = 0; i < _vector.Length; i++)
        {
            if (_vector[i] % 2 != 0 && _vector[i] % 3 != 0)
            {
                result *= _vector[i];
            }
        }

        return result;
    }

    public void SortUp()
    {
        int n = _vector.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (_vector[j] > _vector[j + 1])
                {
                    (_vector[j], _vector[j + 1]) = (_vector[j + 1], _vector[j]);
                }
            }
        }
    }

    public void SortDown()
    {
        int n = _vector.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (_vector[j] < _vector[j + 1])
                {
                    (_vector[j], _vector[j + 1]) = (_vector[j + 1], _vector[j]);
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
        for (int i = 0; i < _vector.Length; i++)
        {
            if (i == _vector.Length - 1)
            {
                Console.Write(_vector[i]);
            }
            else
            {
                Console.Write(_vector[i] + ", ");
            }
        }
        Console.WriteLine("}");
    }

    public static ArrayVector GetFromUserInput()
    {
        string? inp;
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

        var vec = new ArrayVector(length);

        if (inp == "1")
        {
            var r = new Random();
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
                    Console.Write($"Введите значение координаты {{{i}}}: ");
                    inp = Console.ReadLine();
                } while (!int.TryParse(inp, out value));

                vec[i] = value;
            }

            return vec;
        }
    }
}