namespace Lab03;

public interface IVectorable
{
    int this[int index] { get; }
    
    int Length { get; }
    
    double GetNorm();
}