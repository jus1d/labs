namespace Lab05;

public class Fraction
{
    private int _numerator;
    private int _denominator;

    public Fraction(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }

    public Fraction()
    {
        Numerator = 1;
        Denominator = 2;
    }

    public int Numerator
    {
        get => _numerator;
        set => _numerator = value;
    }

    public int Denominator
    {
        get
        {
            return _denominator;
        }
        set
        {
            if (value == 0)
            {
                throw new DivideByZeroException();
            }

            _denominator = value;
        }
    }

    public Fraction Reduce()
    {
        for (int i = Math.Min(Denominator, Numerator); i > 0; i--)
        {
            if (Denominator % i != 0 || Numerator % i != 0) continue;
            
            Denominator /= i;
            Numerator /= i;
        }
        
        return this;
    }

    public Fraction Add(Fraction fraction)
    {
        int totalDenominator = Denominator * fraction.Denominator;
        int totalNumerator = Numerator * fraction.Denominator + fraction.Numerator * Denominator;

        Numerator = totalNumerator;
        Denominator = totalDenominator;

        Reduce();
        
        return this;
    }

    public Fraction Subtract(Fraction fraction)
    {
        int totalDenominator = Denominator * fraction.Denominator;
        int totalNumerator = Numerator * fraction.Denominator - fraction.Numerator * Denominator;

        Numerator = totalNumerator;
        Denominator = totalDenominator;
        
        Reduce();
        
        return this;
    }

    public Fraction Multiply(Fraction fraction)
    {
        Numerator *= fraction.Numerator;
        Denominator *= fraction.Denominator;
        
        Reduce();

        return this;
    }

    public Fraction Divide(Fraction fraction)
    {
        if (fraction.Numerator == 0)
        {
            throw new Exception("Деление на 0 запрещено");
        }
        
        Fraction revertFraction = new Fraction(fraction.Denominator, fraction.Numerator);
        
        Numerator *= fraction.Numerator;
        Denominator *= fraction.Denominator;
        
        Reduce();

        return this;
    }

    public static Fraction operator +(Fraction a, Fraction b) =>
        new Fraction(a.Numerator * b.Denominator + a.Denominator * b.Numerator, a.Denominator * b.Denominator).Reduce();
    
    public static Fraction operator -(Fraction a, Fraction b) => 
        new Fraction(a.Numerator * b.Denominator - a.Denominator * b.Numerator, a.Denominator * b.Denominator).Reduce();

    public static Fraction operator *(Fraction a, Fraction b) =>
        new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator).Reduce();
    
    public static Fraction operator /(Fraction a, Fraction b) 
    {
        if (b._numerator == 0)
        {
            throw new Exception("Деление на 0 запрещено");
        }

        return new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator).Reduce();
    }

    public static Fraction GetFromUserInput(string message = "Ввидите дробь: ")
    {
        int a, b;
        
        do
        {
            Console.Write(message);
            string fractionInput = Console.ReadLine();
            while (fractionInput.Length == 0 || fractionInput.Length < 3 || !fractionInput.Contains('/'))
            {
                Console.WriteLine("Некорректный ввод, введите дробь в формате: 5/7");
                fractionInput = Console.ReadLine();
            }
            
            a = Convert.ToInt32(fractionInput.Split('/')[0]);
            b = Convert.ToInt32(fractionInput.Split('/')[1]);
        } while (b == 0);

        return new Fraction(a, b);
    }

    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }

    public void Log()
    {
        Console.WriteLine(ToString());
    }
}