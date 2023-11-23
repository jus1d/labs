namespace Calculator;

public class Calculator
{
    private static Dictionary<char, int> _operationPrecedence = new()
    {
        { '(', 0 },
        { '+', 1 },
        { '-', 1 },
        { '*', 2 },
        { '/', 2 }
    };

    private static double ApplyOperator(double a, double b, char op)
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

    public static double Calculate(string expression)
    {
        expression = expression.Replace(" ", "");
        expression = expression.Replace(".", ",");
        
        if (expression[0] == '-')
        {
            expression = '0' + expression;
        }
        
        Stack<double> values = new Stack<double>();
        Stack<char> operations = new Stack<char>();

        for (int i = 0; i < expression.Length; i++)
        {
            char c = expression[i];
            char prev;
            if (i == 0)
            {
                prev = ' ';
            }
            else
            {
                prev = expression[i - 1];
            }

            if (char.IsDigit(c))
            {
                string operand = c.ToString();
                while (i + 1 < expression.Length && (char.IsDigit(expression[i + 1]) || expression[i + 1] == ','))
                {
                    operand += expression[i + 1];
                    i++;
                }
                values.Push(double.Parse(operand));
            }
            else if (c == '(')
            {
                if (char.IsDigit(prev))
                {
                    operations.Push('*');
                }
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
}