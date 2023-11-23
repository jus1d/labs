using VegetableStorage.Entity;

namespace VegetableStorage;

public static class Program
{
    public static void Main()
    {
        Storage storage;
        var storageFillMethodMenu = new Menu(new List<string>
        {
            "Заполнить склад случайно",
            "Самому заполнить склад"
        });

        int opt = storageFillMethodMenu.Choose();
        if (opt == 0)
        {
            storage = RandomFill();
        }
        else
        {
            (int containersLimit, float pricePerContainer) = GetStorageSettings();
        
            storage = new Storage(new List<Container>(), containersLimit, pricePerContainer);
        }

        while (true)
        {
            storage.Log();
            
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
                    storage = AddContainer(storage);
                    break;
                case 1:
                    storage = RemoveContainer(storage);
                    break;
                case 2:
                    Console.WriteLine("До скорых встреч!");
                    return;
            }
        }
    }

    public static Storage AddContainer(Storage storage)
    {
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
            container = AddBox(container);
            chosenAdd = menuAdd.Choose();
        }
                    
        storage.AddContainer(container);

        return storage;
    }

    public static Container AddBox(Container container)
    {
        float weight, price;
        string inp;
        
        LogUnderlined(Message.InputBoxWeight);
        
        inp = Console.ReadLine().Replace(".", ",");
        while (!float.TryParse(inp, out weight) || weight < 0)
        {
            LogUnderlined(Message.IncorrectBoxWeight);
            inp = Console.ReadLine().Replace(".", ",");
        }
                        
        LogUnderlined(Message.InputBoxFee);
        
        inp = Console.ReadLine().Replace(".", ",");
        while (!float.TryParse(inp, out price) || weight < 0)
        {
            LogUnderlined(Message.IncorrectBoxFee);
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
            LogRedUnderlined(Message.UnacceptableBoxWeight);
        }

        return container;
    }

    public static Storage RemoveContainer(Storage storage)
    {
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
            return storage;
        }

        string id = ids[chosenRemove].Replace("#", "");
        storage.RemoveContainerById(id);

        return storage;
    }
    
    public static void LogUnderlined(string message)
    {
        Console.Write($"\u001b[4m{message}\u001b[0m > ");
    }
    
    public static void LogRedUnderlined(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"\u001b[4m{message}\u001b[0m");
        Console.ResetColor();
    }

    public static (int containersLimit, float pricePerContainer) GetStorageSettings()
    {
        LogUnderlined(Message.InputContainerLimit);
        string inp = Console.ReadLine();
        int containersLimit;
        while (!int.TryParse(inp, out containersLimit) || containersLimit < 0)
        {
            LogUnderlined(Message.IncorrectContainerLimit);
            inp = Console.ReadLine();
        }
        
        LogUnderlined(Message.InputContainerFee);
        float pricePerContainer;
        inp = Console.ReadLine().Replace(".", ",");
        while (!float.TryParse(inp, out pricePerContainer) || pricePerContainer < 0)
        {
            LogUnderlined(Message.IncorrectContainerFee);
            inp = Console.ReadLine().Replace(".", ",");
        }
        
        return (containersLimit, pricePerContainer);
    }
    
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