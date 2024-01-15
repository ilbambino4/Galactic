using System.Diagnostics;

public static class Console
{
    public static void log(string message)
    {
        System.Diagnostics.Debug.WriteLine(message);
    }
}
