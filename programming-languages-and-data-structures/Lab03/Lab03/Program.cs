namespace Lab03;

public static class Program
{
    public static void Main()
    {
        IVectorable vector = ArrayVector.GetFromUserInput();
        Console.WriteLine(vector.ToString());
        Console.WriteLine($"{vector.GetNorm():0.00}");

        IVectorable list = new LinkedListVector(7);
        Console.WriteLine(list.ToString());
        Console.WriteLine($"{list.GetNorm():0.00}");
    }
}