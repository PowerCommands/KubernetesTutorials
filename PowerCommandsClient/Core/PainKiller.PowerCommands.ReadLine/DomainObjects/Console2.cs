using System.Runtime.Versioning;
using PainKiller.PowerCommands.ReadLine.Contracts;
namespace PainKiller.PowerCommands.ReadLine.DomainObjects;
internal class Console2 : IConsole
{
    public int CursorLeft => Console.CursorLeft;
    public int CursorTop => Console.CursorTop;
    public int BufferWidth => Console.BufferWidth;
    public int BufferHeight => Console.BufferHeight;
    public bool PasswordMode { get; set; }
    [SupportedOSPlatformGuard("windows")]
    public void SetBufferSize(int width, int height)
    {
        if(OperatingSystem.IsWindows()) Console.SetBufferSize(width, height);
    }
    public void SetCursorPosition(int left, int top)
    {
        if (!PasswordMode)
            Console.SetCursorPosition(left, top);
    }
    public void Write(string value)
    {
        if (PasswordMode)
            value = new(default, value.Length);

        Console.Write(value);
    }
    public void WriteLine(string value) => Console.WriteLine(value);
}