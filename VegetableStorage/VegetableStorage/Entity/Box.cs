namespace VegetableStorage.Entity;

/// <summary>
/// Box implements the instance BOX in Vegetable Storage.
/// </summary>
public class Box
{
    public float Weight { get; private set; }
    public float Price { get; private set; }
    public string ID { get; private set;  }

    public Box(float weight, float price)
    {
        if (weight < 0 || price < 0)
        {
            throw new Exception("box weight and price can't be less than 0");
        }

        Weight = weight;
        Price = price;
        ID = Id.New();
    }
    
    /// <summary>
    /// Write to console main info about this box.
    /// </summary>
    public void Log()
    {
        string format = "\x1b[36m";
        string reset = "\x1b[0m";
        
        Console.WriteLine($"{format}BOX #{ID}:{reset} {Weight}кг, {Price}р/кг");
    }
}