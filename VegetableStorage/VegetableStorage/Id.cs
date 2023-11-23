namespace VegetableStorage;

/// <summary>
/// Generates random and unique ID strings.
/// </summary>
public class Id
{
    /// <summary>
    /// Generates and returns an unique ID string.
    /// </summary>
    /// <returns></returns>
    public static string New()
    {
        return new Random().Next(4369, 65536).ToString("X").ToLower();
    }
}