namespace Lab02;

public class Vectors
{
    public static ArrayVector Sum(ArrayVector a, ArrayVector b)
    {
        if (a.Length != b.Length)
        {
            throw new Exception("Vectors norms are not equals");
        }

        ArrayVector vec = new ArrayVector(a.Length);
        for (int i = 0; i < vec.Length; i++)
        {
            vec[i] = a[i] + b[i];
        }

        return vec;
    }

    public static double ScalarMultiply(ArrayVector a, ArrayVector b)
    {
        if (a.Length != b.Length)
        {
            throw new Exception("Vectors norms are not equal");
        }

        int result = 0;
        for (int i = 0; i < a.Length; i++)
        {
            result += a[i] * b[i];
        }

        return result;
    }

    public static ArrayVector MultiplyByNumber(ArrayVector vector, int number)
    {
        for (int i = 0; i < vector.Length; i++)
        {
            vector[i] *= number;
        }

        return vector;
    }

    public static double GetNormSt(ArrayVector vector)
    {
        return vector.GetNorm();
    }
}