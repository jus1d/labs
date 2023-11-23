namespace Lab04;

public class Counter
{
    private int _counter;
    private int _minimalBound;
    private int _maximalBound;

    public Counter(int startValue, int minimalBound, int maximalBound)
    {
        _counter = startValue;
        _minimalBound = minimalBound;
        _maximalBound = maximalBound;
        
        Console.WriteLine($"Счетчик создан с начальным значением: {_counter} и границами: [{_minimalBound}, {_maximalBound}]");
    }
    
    public Counter()
    {
        _counter = 0;
        _minimalBound = -10;
        _maximalBound = 10;
        
        Console.WriteLine($"Счетчик создан с начальным значением: {_counter} и границами: [{_minimalBound}, {_maximalBound}]");
    }

    public int SetMinimalBound(int bound)
    {
        if (bound >= _maximalBound)
        {
            throw new Exception("Минимальная граница счетчика должна быть меньше максимальной");
        }
        _minimalBound = bound;
        return _minimalBound;
    }

    public int SetMaximalBound(int bound)
    {
        if (bound <= _minimalBound)
        {
            throw new Exception("Максимальная граница должна быть больше минимальной");
        }
        _maximalBound = bound;
        return _maximalBound;
    }

    public int Increase()
    {
        if (_counter == _maximalBound)
        {
            _counter = _minimalBound;
        }
        else
        {
            _counter += 1;
        }
        
        return _counter;
    }

    public int Decrease()
    {
        if (_counter == _minimalBound)
        {
            _counter = _maximalBound;
        }
        else
        {
            _counter -= 1;
        }
        
        return _counter;
    }

    public void Log()
    {
        Console.WriteLine($"Текущее значение счетчика {_counter}\nГраницы: [{_minimalBound}, {_maximalBound}]");
    }
}