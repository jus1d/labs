namespace Lab05_net6._0;

public interface IVectorable
{
    int this[int index] { get; set; }
    
    int Length { get; }
    
    double GetNorm();

    void Log(string message = "");
}