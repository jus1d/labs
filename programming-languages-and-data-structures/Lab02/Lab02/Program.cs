namespace Lab02;

public static class Program
{
    public static void Main(string[] args)
    {
        var list = new LinkedListVector();
        list.Log();
        list.DeleteByIndex(2);
        list.Log();
    }
}