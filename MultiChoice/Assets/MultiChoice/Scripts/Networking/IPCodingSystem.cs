////////////////////////////////////////////////////////////
/////   JoinState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System;
using System.Net;

public static class IPCodingSystem
{
    public static string[] CalculateConnectionCode()
    {
        string ipAddress = new WebClient().DownloadString("http://icanhazip.com").Trim();
        UnityEngine.Debug.Log("IP is: " + ipAddress);
        string[] digits = ipAddress.Split('.');
        string[] connectionCode = new string[digits.Length];

        for (int i = 0; i < digits.Length; i++)
        {
            int digit = Convert.ToInt32(digits[i]);
            string code = GetCodeForDigit(digit);
            connectionCode[i] = code;
        }

        return connectionCode;
    }

    public static string GetIPFromCode(string[] code)
    {
        string ip = string.Empty;

        for (int i = 0; i < code.Length; i++)
        {
            string codeSegment = code[i];

            int digit = (codeSegment[1] - 48);
            digit += (codeSegment[0] - 65) * 10;

            ip += digit.ToString();
            if (i < code.Length - 1)
            {
                ip += ".";
            }
        }

        return ip;
    }

    private static string GetCodeForDigit(int digit)
    {
        string code = string.Empty;

        code += (char)(65 + digit / 10);
        code += (digit %= 10).ToString();

        return code;
    }
}
