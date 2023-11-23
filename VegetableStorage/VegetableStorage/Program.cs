using VegetableStorage.Entity;

namespace VegetableStorage;

public static class Program
{
    public static void Main()
    {
        FormattedInputRequest(Message.InputContainerLimit);
        string inp = Console.ReadLine();
        int containersLimit;
        while (!int.TryParse(inp, out containersLimit) || containersLimit < 0)
        {
            FormattedInputRequest(Message.IncorrectContainerLimit);
            inp = Console.ReadLine();
        }
        
        FormattedInputRequest(Message.InputContainerFee);
        float pricePerContainer;
        inp = Console.ReadLine().Replace(".", ",");
        while (!float.TryParse(inp, out pricePerContainer) || pricePerContainer < 0)
        {
            FormattedInputRequest(Message.IncorrectContainerFee);
            inp = Console.ReadLine().Replace(".", ",");
        }
        
        var storage = new Storage(new List<Container>(), containersLimit, pricePerContainer);
        // storage = RandomFill();
        
        storage.Log();

        while (true)
        {
            var menu = new Menu(new List<string>
            {
                "Добавить контейнер на склад", 
                "Удалить контейнер со склада", 
                "Выход"
            });
            int chosen = menu.Choose();
            switch (chosen)
            {
                case 0:
                    var container = new Container(new List<Box>());
                    container.Log();
                    var menuAdd = new Menu(new List<string>
                    {
                        "Добавить коробку в контейнер", 
                        "Продолжить"
                    });
                    var boxes = new List<Box>();
                    
                    int chosenAdd = menuAdd.Choose();
                    
                    while (chosenAdd == 0)
                    {
                        float weight, price;
                        FormattedInputRequest(Message.InputBoxWeight);
                        inp = Console.ReadLine().Replace(".", ",");
                        while (!float.TryParse(inp, out weight) || weight < 0)
                        {
                            FormattedInputRequest(Message.IncorrectBoxWeight);
                            inp = Console.ReadLine().Replace(".", ",");
                        }
                        
                        FormattedInputRequest(Message.InputBoxFee);
                        inp = Console.ReadLine().Replace(".", ",");
                        while (!float.TryParse(inp, out price) || weight < 0)
                        {
                            FormattedInputRequest(Message.IncorrectBoxFee);
                            inp = Console.ReadLine().Replace(".", ",");
                        }
            
                        var box = new Box(weight, price);

                        bool boxAdded = container.AddBox(box);
                        if (boxAdded)
                        {
                            container.Log();
                        }
                        else
                        {
                            container.Log();
                            LogError(Message.UnacceptableBoxWeight);
                        }
                        chosenAdd = menuAdd.Choose();
                    }

                    if (storage.IsContainerCostEffective(container))
                    {
                        storage.AddContainer(container);
                        storage.Log();
                    }
                    else
                    {
                        storage.Log();
                        LogError(Message.UnacceptableContainer);
                    }
                    break;
                case 1:
                    storage.Log();
                    Console.WriteLine(Message.ChooseContainerToDelete);
                    List<string> ids = new List<string>();
                    for (int i = 0; i < storage.Containers.Count; i++)
                    {
                        var c = storage.Containers[i];
                        ids.Add("#" + c.ID);
                    }
                    ids.Add("Отмена");
                    
                    var menuRemove = new Menu(ids);
                    int chosenRemove = menuRemove.Choose();
                    if (chosenRemove == ids.Count - 1)
                    {
                        storage.Log();
                        break;
                    }

                    string id = ids[chosenRemove].Replace("#", "");
                    storage.RemoveContainerById(id);
                    storage.Log();
                    break;
                case 2:
                    return;
            }
        }
    }

    public static void FormattedOutput(string message)
    {
        Console.WriteLine($"\u001b[4m{message}\u001b[0m");
    }

    public static void FormattedInputRequest(string message)
    {
        Console.Write($"\u001b[4m{message}\u001b[0m > ");
    }

    public static void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"\u001b[4m{message}\u001b[0m");
        Console.ResetColor();
    }
    
    /// <summary>
    /// Create and return object of Storage class, filled with random-generated data.
    /// </summary>
    /// <returns></returns>
    public static Storage RandomFill()
    {
        var r = new Random();
        var containers = new List<Container>();

        int limitContainers = r.Next(2, 7);
        int containersAmount = r.Next(1, limitContainers + 1);
        for (int i = 0; i < containersAmount; i++)
        {
            var boxes = new List<Box>();

            for (int j = 0; j < r.Next(2, 5); j++)
            {
                var box = new Box(r.Next(2, 10), r.Next(10, 20));
                boxes.Add(box);
            }

            var container = new Container(boxes);
            containers.Add(container);
        }
        
        
        var storage = new Storage(containers, limitContainers, r.Next(5, 20));
        
        return storage;
    }
}