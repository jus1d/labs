namespace Lab02;

public class ArrayVector
{
    private List<int> _vector;

    public ArrayVector()
    {
        _vector = new List<int>();
        var r = new Random();
        for (int i = 0; i < 5; i++)
        {
            _vector.Add(r.Next(100));
        }
    }
    
    public ArrayVector(int length)
    {
        _vector = new List<int>();
        var r = new Random();
        for (int i = 0; i < length; i++)
        {
            _vector.Add(r.Next(100));
        }
    }

    public int this[int idx]
    {
        get
        {
            if (idx < 0 || idx >= _vector.Count)
            {
                throw new Exception("Vector index out of range");
            }

            return _vector[idx];
        }
        set
        {
            if (idx < 0 || idx >= _vector.Count)
            {
                throw new Exception("Vector index out of range");
            }

            _vector[idx] = value;
        }
    }

    public int Length => _vector.Count;

    public double GetNorm()
    {
        double acc = 0;
        for (int i = 0; i < _vector.Count; i++)
        {
            acc += Math.Pow(_vector[i], 2);
        }

        return Math.Sqrt(acc);
    }

    public void Log(string message = "")
    {
        if (message != "")
        {
            Console.Write(message + ": ");
        }
        Console.Write("{");
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
        Console.Write("}");
    }
}