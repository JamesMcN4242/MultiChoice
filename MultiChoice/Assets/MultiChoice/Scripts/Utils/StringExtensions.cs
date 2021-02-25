////////////////////////////////////////////////////////////
/////   ClientConnectedState.cs
/////   James McNeil - 2021
////////////////////////////////////////////////////////////

public static class StringExtensions
{
    public static bool IsOnlyUpper(this string input)
    {
        for(int i = 0; i < input.Length; i++)
        {
            if(input[i] >= 97 && input[i] <= 122)
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsOnlyLower(this string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] >= 65 && input[i] <= 90)
            {
                return false;
            }
        }

        return true;
    }
}