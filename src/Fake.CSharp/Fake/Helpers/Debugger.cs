using System.Diagnostics;

namespace Fake.CSharp.Fake.Helpers;
public static class Debugger
{
    public static void WriteLine(string head, string message)
    {
        Debug.Write($"[{head}]: {message}.");
    }
}
