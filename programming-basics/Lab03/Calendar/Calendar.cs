namespace Lab03;

public class Calendar
{
    private static int _year;

    private static string[] _months =
    {
        "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", 
        "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
    };
    
    public Calendar(int year)
    {
        _year = year;
    }

    public void LogMonth(int month)
    {
        DateTime date = new DateTime(_year, month, 1);
        
        Console.WriteLine($"{_months[month - 1]} {_year}");
        
        Console.WriteLine("Пн Вт Ср Чт Пт Сб Вс");

        int days = DateTime.DaysInMonth(_year, month);
        int weekDay = (int)date.DayOfWeek - 1;
        if (weekDay < 0)
        {
            weekDay += 7;
        }

        int currentDay = 1;

        for (int i = 0; i < weekDay; i++)
        {
            Console.Write("   ");
        }

        while (currentDay <= days)
        {
            Console.Write($"{currentDay, 2} ");
            currentDay++;

            if (++weekDay > 6)
            {
                weekDay = 0;
                Console.WriteLine();
            }
        }
        
        Console.WriteLine();
    }
}