namespace Lab01;

public class ArrayVector
{
    private List<int> _field;
    
    public int this[int idx]
    {
        get
        {
            if (idx < 0 || idx >= _field.Count)
            {
                throw new IndexOutOfRangeException("Index out of range");
            }
            return _field[idx];
        }
        set
        {
            if (idx < 0 || idx >= _field.Count)
            {
                throw new IndexOutOfRangeException("Index out of range");
            }
            _field[idx] = value;
        }
    }

    public ArrayVector(int length)
    {
        _field = new List<int>();
        for (int i = 0; i < length; i++)
        {
            var r = new Random();
            _field.Add(r.Next(100));
        }
    }

    public ArrayVector()
    {
        int length = 5;
        _field = new List<int>();
        for (int i = 0; i < length; i++)
        {
            var r = new Random();
            _field.Add(r.Next(100));
        }
    }

    public int GetNorm()
    {
        return _field.Count;
    }

    public int SumPositivesWithEvenIndex()
    {
        int acc = 0;
        for (int i = 0; i < _field.Count; i += 2)
        {
            if (_field[i] > 0)
            {
                acc += _field[i];
            }
        }

        return acc;
    }

    public int SumLessAverageAbsoluteWithOddIndex()
    {
        if (_field.Count == 0)
        {
            return 0;
        }
        
        int average = 0;
        for (int i = 0; i < _field.Count; i++)
        {
            average += Math.Abs(_field[i]);
        }

        average /= _field.Count;
        
        
        int acc = 0;
        for (int i = 1; i < _field.Count; i += 2)
        {
            if (_field[i] < average)
            {
                acc += _field[i];
            }
        }

        return acc;
    }

    public int MultiplyEven()
    {
        int result = 1;
        for (int i = 0; i < _field.Count; i++)
        {
            if (_field[i] > 0 && _field[i] % 2 == 0)
            {
                result *= _field[i];
            }
        }

        return result;
    }
    
    public int MultiplyOdd()
    {
        int result = 1;
        for (int i = 0; i < _field.Count; i++)
        {
            if (_field[i] % 2 != 0 && _field[i] % 3 != 0)
            {
                result *= _field[i];
            }
        }

        return result;
    }

    public void SortUp()
    {
        int n = _field.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (_field[j] > _field[j + 1])
                {
                    (_field[j], _field[j + 1]) = (_field[j + 1], _field[j]);
                }
            }
        }
    }

    public void SortDown()
    {
        int n = _field.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (_field[j] < _field[j + 1])
                {
                    (_field[j], _field[j + 1]) = (_field[j + 1], _field[j]);
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
        for (int i = 0; i < _field.Count; i++)
        {
            if (i == _field.Count - 1)
            {
                Console.Write(_field[i]);
            }
            else
            {
                Console.Write(_field[i] + ", ");
            }
        }
        Console.WriteLine("]");
    }
}