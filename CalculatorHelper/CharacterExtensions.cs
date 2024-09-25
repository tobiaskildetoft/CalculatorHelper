namespace CalculatorHelper;

internal static class CharacterExtensions
{
    internal static bool CanEndBlock(this char c)
    {
        return c == ')' || char.IsDigit(c);
    }

    internal static bool IsMultiplicationOrDivision(this char c)
    {
        return c == '*' || c == 'x' || c == 'X' || c == '/' || c == ':';
    }
}
