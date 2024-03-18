namespace Lab04;

public class Comparer : IComparer<IVectorable>
{
    public int Compare(IVectorable a, IVectorable b)
    {
        if (a.GetNorm() < b.GetNorm()) return -1;
        else if (a.GetNorm() > b.GetNorm()) return 1;
        else return 0;
    }
}