namespace Lab01;

public class ArrayVector
{
    private List<int> _vector;
    
    public int this[int idx]
    {
        get
        {
            if (idx < 0 || idx >= _vector.Count)
            {
                throw new IndexOutOfRangeException("Vector index out of range");
            }
            return _vector[idx];
        }
        set
        {
            if (idx < 0 || idx >= _vector.Count)
            {
                throw new IndexOutOfRangeException("Vector index out of range");
            }
            _vector[idx] = value;
        }
    }

    public int Length => _vector.Count();

    public ArrayVector(int length)
    {
        _vector = new List<int>();
        for (int i = 0; i < length; i++)
        {
            var r = new Random();
            _vector.Add(r.Next(100));
        }
    }

    public ArrayVector()
    {
        int length = 5;
        _vector = new List<int>();
        for (int i = 0; i < length; i++)
        {
            var r = new Random();
            _vector.Add(r.Next(100));
        }
    }

    public int GetNorm()
    {
        return _vector.Count;
    }

    public int SumPositivesWithEvenIndex()
    {
        int acc = 0;
        for (int i = 0; i < _vector.Count; i += 2)
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
        if (_vector.Count == 0)
        {
            return 0;
        }
        
        int average = 0;
        for (int i = 0; i < _vector.Count; i++)
        {
            average += Math.Abs(_vector[i]);
        }

        average /= _vector.Count;
        
        
        int acc = 0;
        for (int i = 1; i < _vector.Count; i += 2)
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
        for (int i = 0; i < _vector.Count; i++)
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
        for (int i = 0; i < _vector.Count; i++)
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
        int n = _vector.Count;
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
        int n = _vector.Count;
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
        Console.Write("[");
        for (int i = 0; i < _vector.Count; i++)
        {
            if (i == _vector.Count - 1)
            {
                Console.Write(_vector[i]);
            }
            else
            {
                Console.Write(_vector[i] + ", ");
            }
        }
        Console.WriteLine("]");
    }
}