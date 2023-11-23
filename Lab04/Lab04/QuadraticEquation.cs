namespace Lab04;

public class QuadraticEquation
{
    private double _a;
    private double _b;
    private double _c;

    public QuadraticEquation()
    {
        Console.Write("Введите коэффициент a: ");
        _a = Convert.ToInt32(Console.ReadLine());
        
        Console.Write("Введите коэффициент b: ");
        _b = Convert.ToInt32(Console.ReadLine());
        
        Console.Write("Введите коэффициент c: ");
        _c = Convert.ToInt32(Console.ReadLine());
    }

    public void Log()
    {
        if (_a != 0)
        {
            Console.Write($"{_a}x^2");
        }

        if (_b != 0)
        {
            if (_b > 0)
            {
                Console.Write($" + ");
            }
            else
            {
                Console.Write($" - ");
            }
            Console.Write($"{Math.Abs(_b)}x");
        }
        if (_c != 0)
        {
            if (_c > 0)
            {
                Console.Write($" + ");
            }
            else
            {
                Console.Write($" - ");
            }
            Console.Write($"{Math.Abs(_c)}");
        }

        if (_a == 0 && _b == 0 && _c == 0)
        {
            Console.Write("0");
        }
        Console.WriteLine(" = 0");
    }

    public void Solve()
    {
        double x1, x2;
        if (_a == 0)
        {
            if (_b == 0)
            {
                Console.WriteLine($"x = 0");
            }
            Console.WriteLine($"x = {(0 - _c) / _b}");
            return;
        }
        double discriminant = Math.Pow(_b, 2) - 4 * _a * _c;
        if (discriminant < 0)
        {
            Console.WriteLine("Квадратное уравнение не имеет корней");
        }
        else if (discriminant > 0)
        {
            x1 = (-_b + Math.Sqrt(discriminant)) / (2 * _a);
            x2 = (-_b - Math.Sqrt(discriminant)) / (2 * _a);
            Console.WriteLine($"x1 = {x1}; x2 = {x2}");
        }
        else
        {
            x1 = -_b / (2 * _a);
            if (double.IsNaN(x1))
            {
                Console.WriteLine("x \u2208 R");
            }
            else
            {
                Console.WriteLine($"x = {x1}");
            }
        }
    }
}