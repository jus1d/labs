namespace VegetableStorage.Entity;

/// <summary>
/// Container implements the CONTAINER instance of Vegetable Storage.
/// </summary>
public class Container
{
    public List<Box> Boxes { get; private set; }

    public float CurrentWeight { get; private set; }
    public int WeightLimit { get; private set; }

    public string ID { get; private set; }

    public Container(List<Box> boxes)
    {
        Boxes = boxes;
        WeightLimit = new Random().Next(10, 101);
        CurrentWeight = 0;

        for (int i = 0; i < Boxes.Count; i++)
        {
            CurrentWeight += Boxes[i].Weight;
        }

        ID = Id.New();
    }
    
    /// <summary>
    /// Add the box to boxes list of current container. If box won't fir by weight, it will not be added.
    /// </summary>
    /// <param name="box"></param>
    /// <returns></returns>
    public bool AddBox(Box box)
    {
        if (CurrentWeight + box.Weight > WeightLimit)
        {
            return false;
        }

        Boxes.Add(box);
        CurrentWeight += box.Weight;

        return true;
    }
    
    /// <summary>
    /// Write to console main information about container. Includes information about boxes inside current container.
    /// </summary>
    public void Log()
    {
        Console.Clear();
        string format = "\x1b[36m";
        string reset = "\x1b[0m";

        Console.WriteLine($"{format}CONTAINER #{ID}:{reset} {Boxes.Count} коробок, {CurrentWeight}/{WeightLimit}кг");
        for (int i = 0; i < Boxes.Count; i++)
        {
            var box = Boxes[i];
            if (i == Boxes.Count - 1)
            {
                Console.WriteLine($"\u2514\u2500\u2500 {format}BOX #{box.ID}:{reset} {box.Weight}кг, {box.Price}р/кг");
            }
            else
            {
                Console.WriteLine($"\u251c\u2500\u2500 {format}BOX #{box.ID}:{reset} {box.Weight}кг, {box.Price}р/кг");
            }
        }
    }
}