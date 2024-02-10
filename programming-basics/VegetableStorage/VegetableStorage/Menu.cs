namespace VegetableStorage;

/// <summary>
/// Generate a console menu, which you can control with arrow buttons.
/// </summary>
public class Menu
{
    private List<string> _options;
    private int _selectedItemIndex;
    
    public Menu(List<string> options)
    {
        _options = options;
        _selectedItemIndex = 0;
    }
    
    /// <summary>
    /// Write to console your menu and returns chosen option's index.
    /// </summary>
    /// <returns></returns>
    public int Choose()
    {
        ConsoleKeyInfo key;
        Show();
        do
        {
            var (left, top) = Console.GetCursorPosition();
            Console.SetCursorPosition(0, top - _options.Count);
            Show();
            key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    _selectedItemIndex = _selectedItemIndex == 0 ? 0 : _selectedItemIndex - 1;
                    break;
                case ConsoleKey.DownArrow:
                    _selectedItemIndex = _selectedItemIndex == _options.Count - 1 ? _options.Count - 1 : _selectedItemIndex + 1;
                    break;
            }
        } while (key.Key != ConsoleKey.Enter);

        return _selectedItemIndex;
    }
    
    /// <summary>
    /// Write menu to console.
    /// </summary>
    private void Show()
    {
        for (int i = 0; i < _options.Count; i++)
        {
            if (i == _selectedItemIndex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                ApplyBoldUnderlined();
            }

            Console.WriteLine($"{i + 1}. {_options[i]}");
            
            Console.ResetColor();
            ResetAnsi();
        }
    }
    
    /// <summary>
    /// Writes reset symbol in ANSI escape.
    /// </summary>
    public static void ResetAnsi()
    {
        Console.Write("\x1b[0m");
    }
    
    /// <summary>
    /// Writes ANSI escape symbols for output format.
    /// </summary>
    public static void ApplyBoldUnderlined()
    {
        Console.Write("\x1b[1m");
        Console.Write("\x1b[4m");
    }
}