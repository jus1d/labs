namespace Lab07;

public static class Program
{
    public static void Main(string[] args)
    {
        Greeting();

        while (true)
        {
            Console.WriteLine("Выберете действие со строкой:\n\n" +
                              "\t1 - Посчитать в строке число букв\n" +
                              "\t2 - Посчитать среднюю длину слова в строке\n" +
                              "\t3 - Заменить все слова на другое\n" +
                              "\t4 - Посчитать вхождения подстроки\n" +
                              "\t5 - Проверить является ли строка палиндромом\n" +
                              "\t6 - Проверить является ли строка датой\n" +
                              "\t7 - Выход из программы");
            string inp = Console.ReadLine();

            string s;

            switch (inp)
            {
                case "1":
                    Console.Write("Введите строку для подсчета букв: ");
                    s = Console.ReadLine();
                    
                    int letters = Strings.CountLetters(s);
                    
                    Console.WriteLine($"В строке {letters} букв");
                    break;
                case "2":
                    Console.Write("Введите строку для подсчета средней длины слова: ");
                    s = Console.ReadLine();
                    
                    double avgWordLength = Strings.CountAverageWordLength(s);
                    
                    Console.WriteLine($"Средняя длина слова в строке: {avgWordLength}");
                    break;
                case "3":
                    Console.Write("Введите строку для замены слова: ");
                    s = Console.ReadLine();
                    
                    Console.Write("Введите через пробел слово которое хотите заменить и слово на которое хотите заменить: ");
                    string[] words = Console.ReadLine().Split(" ");
                    
                    Console.WriteLine("Новая строка: ");
                    Console.WriteLine(Strings.ReplaceWordsByVeronikaBreneva(s, words[0], words[1]));
                    break;
                case "4":
                    do
                    {
                        Console.Write("Введите строку для подсчета вхождений подстроки: ");
                        s = Console.ReadLine();
                    } while (s == "");

                    string substring;
                    do
                    {
                        Console.Write("Введите подстроку: ");
                        substring = Console.ReadLine();
                    } while (substring == "");
                    
                    Console.WriteLine($"В исходной строке подстрока '{substring}' встречается {Strings.CountSubstrings(s, substring)} раз");
                    break;
                case "5":
                    Console.Write("Введите строку для проверки на палиндром: ");
                    s = Console.ReadLine();
                    
                    bool isPalindrome = Strings.IsPalindrome(s);

                    if (isPalindrome)
                    {
                        Console.WriteLine($"Строка '{s}' является палиндромом");
                    }
                    else
                    {
                        Console.WriteLine($"Строка '{s}' не является палиндромом");
                    }
                    break;
                case "6":
                    Console.Write("Введите строку для проверки на дату: ");
                    s = Console.ReadLine();
                    
                    bool isDate = Strings.IsDate(s);

                    if (isDate)
                    {
                        Console.WriteLine($"Строка '{s}' является датой");
                    }
                    else
                    {
                        Console.WriteLine($"Строка '{s}' не является датой");
                    }
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Нет такого пункта в меню");
                    break;
            }
        }
    }

    public static void Greeting()
    {
        Console.WriteLine("Лабораторная работа #7\n" +
                          "Выполнил студент группы 6101-020302D - Фадеев Артем");
    }
}