using System.Globalization;

namespace CalculatorHelper;

public static class CalculatorHelper
{
    private const NumberStyles numberStyle = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
    private static readonly CultureInfo cultureInfoCommaSeparator = CultureInfo.CreateSpecificCulture("da-DK");
    private static readonly CultureInfo cultureInfoDotSeparator = CultureInfo.CreateSpecificCulture("en-GB");
    public static float Calculate(string input)
    {
        input = input.Replace(" ", string.Empty);

        if (input.Length == 0)
        {
            return 0;
        }

        if (input[0] == '(' && input.FindIndexOfEndParanthesis(0) == input.Length - 1)
        {
            return Calculate(input[1..(input.Length - 1)]);
        }

        // TODO: find smoother way to decide operation. Maybe build into other methods
        // TODO: Implement base class and extensions using template/strategy pattern
        if (input.TryFindOuterAdditionIndex(out var index))
        {
            if (input[index] == '+')
            {
                return AddResults(input.SplitAt(index));
            }
            if (input[index] == '-')
            {
                return SubtractResults(input.SplitAt(index));
            }
        }

        if (input.TryFindOuterMultiplicationIndex(out var indexMult))
        {
            if (input[indexMult] == '*' || input[indexMult] == 'x' || input[indexMult] == 'X')
            {
                return MultiplyResults(input.SplitAt(indexMult));
            }
            if (input[indexMult] == '/' || input[indexMult] == ':')
            {
                return DivideResults(input.SplitAt(indexMult));
            }
            if (input[indexMult] == '(')
            {
                var splitInput = input.SplitAt(indexMult);
                if (splitInput.Item1 == string.Empty || splitInput.Item1 == "-")
                {
                    splitInput.Item1 = splitInput.Item1 + "1";
                }
                splitInput.Item2 = '(' + splitInput.Item2;
                return MultiplyResults((splitInput.Item1, splitInput.Item2));
            }
            if (input[indexMult] == ')')
            {
                var splitInput = input.SplitAt(indexMult);
                splitInput.Item1 = splitInput.Item1 + ')';
                return MultiplyResults((splitInput.Item1, splitInput.Item2));
            }
        }
        
        if (float.TryParse(input, numberStyle, cultureInfoCommaSeparator, out float resultComma))
        {
            return resultComma;
        }

        if (float.TryParse(input, numberStyle, cultureInfoDotSeparator, out float resultDot))
        {
            return resultDot;
        }

        throw new ArgumentException($"Input section {input} does not represent a float");
    }

    private static bool TryFindOuterAdditionIndex(this string input, out int index)
    {
        index = 0;

        while (index < input.Length)
        {
            if ((input[index] == '+' || input[index] == '-') && index != 0 && input[index - 1].CanEndBlock())
            {
                return true;
            }
            if (input[index] == '(')
            {
                index = input.FindIndexOfEndParanthesis(index) + 1;
                continue;
            }
            index++;
        }
        return false;
    }

    private static bool TryFindOuterMultiplicationIndex(this string input, out int index)
    {
        index = 0;

        while (index < input.Length)
        {
            if (input[index].IsMultiplicationOrDivision() && index != 0 && input[index - 1].CanEndBlock())
            {
                return true;
            }
            if (input[index] == '(')
            {
                if (index == 0)
                {
                    index = input.FindIndexOfEndParanthesis(index);
                }
                return true;
            }
            index++;
        }
        return false;
    }

    private static int FindIndexOfEndParanthesis(this string input, int startIndex)
    {
        var paranthesisBalance = 1;
        for (int i = startIndex + 1; i < input.Length; i++)
        {
            if (input[i] == '(')
            {
                paranthesisBalance++;
            }
            if (input[i] == ')')
            {
                paranthesisBalance--;
            }
            if (paranthesisBalance == 0)
            {
                return i;
            }
        }
        throw new ArgumentException($"Input string {input} has no matching paranthesis for start paranthesis at index {startIndex}");
    }

    private static (string, string) SplitAt(this string input, int index)
    {
        if (index == input.Length - 1)
        {
            throw new ArgumentException($"Input string {input} was about to be split at last character");
        }
        return (input[..index], input[(index + 1)..]);
    }

    private static float AddResults((string, string) input)
    {
        return Calculate(input.Item1) + Calculate(input.Item2);
    }

    private static float SubtractResults((string, string) input)
    {
        return Calculate(input.Item1) - Calculate(input.Item2);
    }

    private static float MultiplyResults((string, string) input)
    {
        return Calculate(input.Item1) * Calculate(input.Item2);
    }

    private static float DivideResults((string, string) input)
    {
        return Calculate(input.Item1) / Calculate(input.Item2);
    }
}
