using System.Text;

namespace RomanCalculator;

public static class RomanCalculator
{
    private static Dictionary<char, int> _dictionaryRomanToInt = new ()
    {
        { 'M', 1000},
        { 'D', 500},
        { 'C', 100},
        { 'L', 50},
        { 'X', 10},
        { 'V', 5},
        { 'I', 1},
    };

    private static Dictionary<int, string> _dictionaryIntToRoman = new()
    {
        { 1000, "M" },
        { 900, "CM" },
        { 500, "D" },
        { 400, "CD" },
        { 100, "C" },
        { 90, "XC" },
        { 50, "L" },
        { 40, "XL" },
        { 10, "X" },
        { 9, "IX" },
        { 5, "V" },
        { 4, "IV" },
        { 1, "I" }
    };
    
    private static Dictionary<char, int> _operationPrecedence = new()
    {
        { '(', 0 },
        { '+', 1 },
        { '-', 1 },
        { '*', 2 },
        { '/', 2 }
    };
    
    public static int ConvertRomanToInt(string romanNumber)
    {
        romanNumber = romanNumber.ToUpper();
        
        int result = 0;
        int prevValue = 0;

        for (int i = romanNumber.Length - 1; i >= 0; i--)
        {
            int currentValue = _dictionaryRomanToInt[romanNumber[i]];

            if (currentValue < prevValue)
            {
                result -= currentValue;
            }
            else
            {
                result += currentValue;
            }

            prevValue = currentValue;
        }
        
        return result;
    }

    public static string ConvertIntToRoman(int number)
    {
        var values = _dictionaryIntToRoman.Keys.ToArray();
        int i = 0;
        StringBuilder result = new StringBuilder();
        while (number > 0 && i <= values.Length)
        {
            if (number - values[i] >= 0)
            {
                result.Append(_dictionaryIntToRoman[values[i]]);
                number -= values[i];
            }
            else
            {
                i++;
            }
        }
        return result.ToString();   
    }

    private static int ApplyOperator(int a, int b, char op)
    {
        switch (op)
        {
            case '+':
                return a + b;
            case '-':
                return b - a;
            case '*':
                return a * b;
            case '/':
                if (a == 0)
                {
                    throw new DivideByZeroException("Деление на 0 запрещено");
                }
                return b / a;
            default:
                return 0;
        }
    }

    public static int Calculate(string expression)
    {
        expression = expression.Replace(" ", "");
        
        if (expression[0] == '-')
        {
            expression = '0' + expression;
        }
        
        Stack<int> values = new Stack<int>();
        Stack<char> operations = new Stack<char>();

        for (int i = 0; i < expression.Length; i++)
        {
            char c = expression[i];

            if (IsRomanNumber(c))
            {
                string operand = c.ToString();
                while (i + 1 < expression.Length && IsRomanNumber(expression[i + 1]))
                {
                    operand += expression[i + 1];
                    i++;
                }
                values.Push(ConvertRomanToInt(operand));
            }
            else if (c == '(')
            {
                operations.Push(c);
            }
            else if (c == ')')
            {
                while (operations.Count > 0 && operations.Peek() != '(')
                {
                    values.Push(ApplyOperator(values.Pop(), values.Pop(), operations.Pop()));
                }

                operations.Pop();
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                while (operations.Count > 0 && _operationPrecedence[operations.Peek()] >= _operationPrecedence[c])
                {
                    values.Push(ApplyOperator(values.Pop(), values.Pop(), operations.Pop()));
                }
                operations.Push(c);
            }
        }

        while (operations.Count > 0)
        {
            values.Push(ApplyOperator(values.Pop(), values.Pop(), operations.Pop()));
        }

        return values.Peek();
    }

    private static bool IsRomanNumber(string s)
    {
        string[] romans = { "I", "V", "X", "L", "C", "D", "M" };
        return romans.Contains(s);
    }

    private static bool IsRomanNumber(char c)
    {
        string[] romans = { "I", "V", "X", "L", "C", "D", "M" };
        return romans.Contains(c.ToString());
    }
}