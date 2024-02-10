namespace Lab06;


public class MyArray
{
    private int[,] _array;
    private int _m;
    private int _n;

    public MyArray(int m, int n)
    {
        _m = m;
        _n = n;
        _array = new int[_m, _n];

        Fill();
    }

    public MyArray()
    {
        _m = 10;
        _n = 10;
        _array = new int[_m, _n];
        
        Fill();
    }

    private void Fill()
    {
        Random r = new Random();

        for (int i = 0; i < _m; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                _array[i, j] = r.Next(-100, 101);
            }
        }
    }

    public void Log()
    {
        for (int i = 0; i < _m; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                Console.Write(_array[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }

    public void SortColumnsAscending()
    {
        for (int j = 0; j < _n; j++)
        {
            for (int i = 0; i < _m - 1; i++)
            {
                for (int k = i + 1; k < _m; k++)
                {
                    if (GetColumnSum(i) > GetColumnSum(k))
                    {
                        SwapColumns(i, k);
                    }
                }
            }
        }
    }

    public void SortColumnsDescending()
    {
        for (int j = 0; j < _n; j++)
        {
            for (int i = 0; i < _m - 1; i++)
            {
                for (int k = i + 1; k < _m; k++)
                {
                    if (GetColumnSum(i) < GetColumnSum(k))
                    {
                        SwapColumns(i, k);
                    }
                }
            }
        }
    }

    private int GetColumnSum(int column)
    {
        int sum = 0;
        for (int i = 0; i < _m; i++)
        {
            sum += _array[i, column];
        }
        return sum;
    }

    private void SwapColumns(int column1, int column2)
    {
        for (int i = 0; i < _m; i++)
        {
            int temp = _array[i, column1];
            _array[i, column1] = _array[i, column2];
            _array[i, column2] = temp;
        }
    }
}
