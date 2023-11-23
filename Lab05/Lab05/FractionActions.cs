namespace Lab05;

public class FractionActions
{
    public static Fraction Sum(Fraction a, Fraction b)
    {
        int totalDenominator = a.Denominator * b.Denominator;
        int totalNumerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
        
        return new Fraction(totalNumerator, totalDenominator).Reduce();
    }
    
    public static Fraction Subtraction(Fraction a, Fraction b)
    {
        int totalDenominator = a.Denominator * b.Denominator;
        int totalNumerator = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
        
        return new Fraction(totalNumerator, totalDenominator).Reduce();
    }
    
    public static Fraction Multiplication(Fraction a, Fraction b)
    {
        int numerator = a.Numerator * b.Numerator;
        int denominator = a.Denominator * b.Denominator;

        return new Fraction(numerator, denominator).Reduce();
    }
    
    public static Fraction Division(Fraction a, Fraction b)
    {
        return new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator).Reduce();
    }
}