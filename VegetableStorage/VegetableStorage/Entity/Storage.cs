namespace VegetableStorage.Entity;

/// <summary>
/// Storage implements main instance of Vegetable Storage.
/// </summary>
public class Storage
{
    private List<Container> _containers;
    private int _containersLimit;
    private float _pricePerContainer;

    public List<Container> Containers => _containers;

    public Storage(List<Container> containers, int containersLimit, float pricePerContainer)
    {
        _containersLimit = containersLimit;
        _pricePerContainer = pricePerContainer;
        _containers = containers;
    }
    
    /// <summary>
    /// Add container to containers list of current storage, if it cost-effective.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public bool AddContainer(Container container)
    {
        if (!IsContainerCostEffective(container)) // Проверка контейнера на рентабельность
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Контейнер не был добавлен на склад, так как он не рентабелен");
            Console.ResetColor();
            
            return false;
        }
        
        if (_containers.Count == _containersLimit)
        {
            _containers = _containers.Slice(1, _containers.Count - 1);
        }
        
        _containers.Add(container);

        return true;
    }

    /// <summary>
    /// Check is container cost-effective for current storage or not.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public bool IsContainerCostEffective(Container container)
    {
        float damageRate = new Random().Next(0, 50) / 10;
        float containerPrice = 0;
        for (int i = 0; i < container.Boxes.Count; i++)
        {
            var box = container.Boxes[i];
            containerPrice += box.Price - box.Price * damageRate;
        }

        // return containerPrice <= _pricePerContainer; // Мне кажется так правильно
        return containerPrice > _pricePerContainer; // Как в задании
    }
    
    /// <summary>
    /// Remove a container from storage by it's ID.
    /// </summary>
    /// <param name="id"></param>
    public void RemoveContainerById(string id)
    {
        _containers.RemoveAll(c => c.ID == id);
    }
    
    /// <summary>
    /// Write main information about current storage, includes information about containers inside, and about boxes inside the containers.
    /// </summary>
    public void Log()
    {
        Console.Clear();
        string format = "\x1b[36m";
        string reset = "\x1b[0m";
        
        Console.WriteLine($"{format}STORAGE:{reset} {_containers.Count}/{_containersLimit}, {_pricePerContainer}р/контейнер");

        for (int i = 0; i < _containers.Count; i++)
        {
            var container = _containers[i];
            string treeChar;
            
            if (i == _containers.Count - 1)
                treeChar = "\u2514\u2500\u2500";
            else
                treeChar = "\u251c\u2500\u2500";
            
            Console.WriteLine($"{treeChar} {format}CONTAINER #{container.ID}:{reset} коробок: {container.Boxes.Count()}, {container.CurrentWeight}/{container.WeightLimit}кг");
            for (int j = 0; j < _containers[i].Boxes.Count; j++)
            {
                var box = container.Boxes[j];
                if (i != _containers.Count - 1)
                {
                    if (j == container.Boxes.Count - 1)
                    {
                        Console.WriteLine($"\u2502\u00a0\u00a0 \u2514\u2500\u2500 {format}BOX #{box.ID}:{reset} {box.Weight}кг, {box.Price}р/кг");
                    }
                    else
                    {
                        Console.WriteLine($"\u2502\u00a0\u00a0 \u251c\u2500\u2500 {format}BOX #{box.ID}:{reset} {box.Weight}кг, {box.Price}р/кг");
                    }
                }
                else
                {
                    if (j == container.Boxes.Count - 1)
                    {
                        Console.WriteLine($"    \u2514\u2500\u2500 {format}BOX #{box.ID}:{reset} {box.Weight}кг, {box.Price}р/кг");
                    }
                    else
                    {
                        Console.WriteLine($"    \u251c\u2500\u2500 {format}BOX #{box.ID}:{reset} {box.Weight}кг, {box.Price}р/кг");
                    }
                }
            }
        }
    }
}