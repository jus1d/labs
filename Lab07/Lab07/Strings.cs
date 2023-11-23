using System.Text;

namespace Lab07;

public static class Strings
{
    public static int CountLetters(string s)
    {
        int lettersCounter = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (char.IsLetter(s[i]))
            {
                lettersCounter++;
            }
        }

        return lettersCounter;
    }

    public static double CountAverageWordLength(string s)
    {
        if (s.Length == 0)
        {
            return 0;
        }
        string[] words = s.Split(new char[] { ' ', ',', '.', '"', '!', '?', ':', ';', '-' }, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
        {
            return 0;
        }
        
        double avg = 0;

        for (int i = 0; i < words.Length; i++)
        {
            avg += words[i].Length;
        }

        avg /= words.Length;

        return avg;
    }

    public static string ReplaceWordsByVeronikaBreneva(string s, string oldWord, string newWord)
    {
        string[] words = s.ToLower().Split();

        for (int i = 0; i < words.Length; i++)
        {
            if (words[i] == oldWord)
                words[i] = newWord;
            if (words[i] == oldWord + ",")
                words[i] = newWord + ",";
            if (words[i] == oldWord + ":")
                words[i] = newWord + ":";
            if (words[i] == oldWord + "!")
                words[i] = newWord + "!";
            if (words[i] == oldWord + "?")
                words[i] = newWord + "?";
            if (words[i] == oldWord + ".")
                words[i] = newWord + ".";
        }

        return String.Join(" ", words);
    }
    
    private static bool IsWordBoundary(string text, int position, int wordLength)
    {
        return (position == 0 || !char.IsLetterOrDigit(text[position - 1]))
               && (position + wordLength >= text.Length || !char.IsLetterOrDigit(text[position + wordLength]));
    }

    public static int CountSubstrings(string s, string substring)
    {
        s = s.ToLower();
        
        if (s.Length < substring.Length)
        {
            return 0;
        }

        int c = 0;
        for (int i = 0; i < s.Length - substring.Length; i++)
        {
            bool isEquals = true;
            for (int j = 0; j < substring.Length; j++)
            {
                if (s[i + j] != substring[j]) isEquals = false;
            }

            if (isEquals)
            {
                c++;
            }
        }

        return c;
    }

    public static bool IsPalindrome(string s)
    {
        s = s.ToLower().Replace(",", "").Replace(".", "").Replace("-", "").Replace("=", "").Replace("!", "")
            .Replace("?", "").Replace("(", "").Replace(")", "").Replace(";", "").Replace(":", "").Replace(" ", "");
        
        Console.WriteLine(s);
        for (int i = 0; i < s.Length / 2; i++)
        {
            if (s[i] != s[s.Length - i - 1])
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsDate(string s)
    {
        string[] parts = s.Split('.');
        if (parts.Length != 3)
            return false;

        if (parts[0].Length != 2 || parts[1].Length != 2 || (parts[2].Length != 4 && parts[2].Length != 2))
            return false;

        if (!int.TryParse(parts[0], out int day) || !int.TryParse(parts[1], out int month) || !int.TryParse(parts[2], out int year))
            return false;

        if (year < 1 || month < 1 || month > 12 || day < 1 || day > 31)
            return false;

        if ((month == 4 || month == 6 || month == 9 || month == 11) && day > 30)
            return false;

        if (month == 2)
        {
            if (year % 4 == 0)
            {
                if (day > 29)
                    return false;
            }
            else
            {
                if (day > 28)
                    return false;
            }
        }

        return true;
    }
}