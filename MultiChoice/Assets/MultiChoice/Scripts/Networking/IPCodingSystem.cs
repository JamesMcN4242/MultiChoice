////////////////////////////////////////////////////////////
/////   JoinState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System;
using System.Net;
using System.Net.Sockets;
using static UnityEngine.Debug;

public static class IPCodingSystem
{
    public static string[] CalculateConnectionCode()
    {
#if EXTERNAL_IP
        string ipAddress = new WebClient().DownloadString("http://icanhazip.com").Trim();
#else
        string ipAddress = GetLocalIPAddressAsString();
#endif
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
            int digit = DecodeIPSegment(code[i]);

            ip += digit.ToString();
            if (i < code.Length - 1)
            {
                ip += ".";
            }
        }

        return ip;
    }
    
    public static int DecodeIPSegment(string codeSegment)
    {
        return (codeSegment[1] - 48) + (codeSegment[0] - 65) * 10;
    }

    public static string GetLocalIPAddressAsString()
    {
        IPAddress iPAddress = GetLocalIPAddress();
        Assert(iPAddress != null, "IP Address returned was null");
        return iPAddress?.ToString();        
    }

    public static IPAddress GetLocalIPAddress()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip;
            }
        }

        LogError("Couldn't find the local IP");
        return null;
    }

    private static string GetCodeForDigit(int digit)
    {
        string code = string.Empty;

        code += (char)(65 + digit / 10);
        code += (digit %= 10).ToString();

        return code;
    }
}
