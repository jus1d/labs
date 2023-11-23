namespace Lab06;

public static class Sorting
{
    public static void CreateArrayFromUserInput(int[] array)
    {
        Console.Write($"Введите массив из {array.Length} элементов: ");
        string inp = Console.ReadLine();
        string[] inpArray = inp.Split(' ');

        for (int i = 0; i < inpArray.Length; i++)
        {
            array[i] = Convert.ToInt32(inpArray[i]);
        }
    }

    public static int[] CreateRandomArray(int length, int minBound, int maxBound)
    {
        Random rnd = new Random();
        int[] array = new int[length];

        for (int i = 0; i < array.Length; i++)
        {
            array[i] = rnd.Next(minBound, maxBound);
        }

        return array;
    }
        
    public static int[] Bubble(int[] a)
    {
        var array = a;
        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (array[j] > array[j + 1])
                {
                    int temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }

        return array;
    }

    public static int[] Shell(int[] a)
    {
        var array = a;
        int n = array.Length;

        for (int gap = n / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < n; i++)
            {
                int temp = array[i];
                int j;

                for (j = i; j >= gap && array[j - gap] > temp; j -= gap)
                {
                    array[j] = array[j - gap];
                }

                array[j] = temp;
            }
        }

        return array;
    }

    public static int[] Shake(int[] a)
    {
        var array = a;
        int left = 0;
        int right = array.Length - 1;
        bool swapped;

        do
        {
            swapped = false;

            // Similar to the Bubble sort, move the largest element to the right
            for (int i = left; i < right; i++)
            {
                if (array[i] > array[i + 1])
                {
                    Swap(array, i, i + 1);
                    swapped = true;
                }
            }

            if (!swapped)
                break;

            swapped = false;

            // Move the smallest element to the left
            right--;

            for (int i = right; i > left; i--)
            {
                if (array[i] < array[i - 1])
                {
                    Swap(array, i, i - 1);
                    swapped = true;
                }
            }

            left++;
        }
        while (swapped);

        return array;
    }

    public static int[] Insert(int[] a)
    {
        var array = a;
        int n = array.Length;
        for (int i = 1; i < n; i++)
        {
            int currentElement = array[i];
            int j = i - 1;

            while (j >= 0 && array[j] > currentElement)
            {
                array[j + 1] = array[j];
                j--;
            }

            array[j + 1] = currentElement;
        }

        return array;
    }

    private static void Swap(int[] array, int i, int j)
    {
        int temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }

    public static void LogArray(int[] array)
    {
        if (array.Length == 0)
        {
            Console.WriteLine("[]");
            return;
        }
        
        Console.Write($"[{array[0]}");
        for (int i = 1; i < array.Length; i++)
        {
            Console.Write($", {array[i]}");
        }
        Console.WriteLine("]");
    }
}