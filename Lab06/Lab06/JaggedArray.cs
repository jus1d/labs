namespace Lab06;

public class JaggedArray
{
    private int[][] _array;

    public JaggedArray(int length)
    {
        _array = new int[length][];
    }

    public void Sort()
    {
        int length = 0;
        for (int i = 0; i < _array.Length; i++)
        {
            length += _array[i].Length;
        }
        
        int[] firstDimArray = new int[length];

        int n = 0;
        for (int i = 0; i < _array.Length; i++)
        {
            for (int j = 0; j < _array[i].Length; j++)
            {
                firstDimArray[n] = _array[i][j];
                n++;
            }
        }

        Sorting.Shake(firstDimArray);

        n = 0;
        for (int i = 0; i < _array.Length; i++)
        {
            for (int j = 0; j < _array[i].Length; j++)
            {
                _array[i][j] = firstDimArray[n];
                n++;
            }
        }
    }

    public void FillArrayFromUserInput()
    {
        for (int i = 0; i < _array.Length; i++)
        {
            Console.Write($"Введите {i} массив ступенчатого массива: ");
            string inp = Console.ReadLine();
            int[] nums;

            if (inp.Replace(" ", "") == "")
            {
                nums = new int[0];
            }
            else
            {
                nums = Array.ConvertAll(inp.Split(" "), Convert.ToInt32);
            }
            _array[i] = nums;
        }
    }

    public void FillWithRandomData()
    {
        for (int i = 0; i < _array.Length; i++)
        {

            Random r = new Random();
            _array[i] = new int[r.Next(1, 15)];
            for (int j = 0; j < _array[i].Length; j++)
            {
                _array[i][j] = r.Next(-100, 100);
            }
        }
    }

    public void Log()
    {
        Console.WriteLine("[");

        for (int i = 0; i < _array.Length; i++)
        {
            Console.Write("\t[ ");
            for (int j = 0; j < _array[i].Length; j++)
            {
                Console.Write($"{_array[i][j]}");
                if (j == _array[i].Length - 1)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write(", ");
                }
            }
            Console.Write("]\n");
        }
        
        Console.WriteLine("]");
    }
}