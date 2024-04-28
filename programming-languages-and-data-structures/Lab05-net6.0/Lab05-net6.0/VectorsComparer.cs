namespace Lab05_net6._0;

public class VectorsComparer : IComparer<IVectorable>
{
    public int Compare(IVectorable a, IVectorable b)
    {
        if (a.GetNorm() < b.GetNorm()) return -1;
        if (a.GetNorm() > b.GetNorm()) return 1;
        return 0;
    }
}