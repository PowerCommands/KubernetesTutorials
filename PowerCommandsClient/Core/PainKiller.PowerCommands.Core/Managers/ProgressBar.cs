namespace PainKiller.PowerCommands.Core.Managers;

using System;

/// <summary>
/// A simple console progress bar.
/// https://github.com/jingedawang/ProgressBar
/// </summary>
/// <example>
/// There are three modes can be used.
/// Mode 1: Percentage mode
/// <code>
///     ProgressBar progressBar = new ProgressBar();
///     progressBar.Show();
///     progressBar.Update(0.01);
/// </code>
/// Mode 2: Item count mode
/// <code>
///     ProgressBar progressBar = new ProgressBar(100);
///     progressBar.Show();
///     progressBar.Update(1);
/// </code>
/// Mode 3: Increase mode
/// <code>
///     ProgressBar progressBar = new ProgressBar(100);
///     progressBar.Show();
///     progressBar.UpdateOnce();
/// </code>
/// </example>
public class ProgressBar
{
    // A flag used for preventing multiple initialization.
    private bool _enabled;
    // The row index of the progress bar.
    private int _cursorTop;
    // The column index of the progress bar.
    private int _cursorLeft;
    // The percentage of the progress.
    private int _percentage;
    // The current count of the progress.
    private long _count;
    // Total item number that to be processed.
    private readonly long _total;

    private readonly ConsoleColor _color;

    // The text width on the left side of the progress bar.
    private const int TextWidth = 6;

    /// <summary>
    /// Create a default progress bar.
    /// </summary>
    /// <remarks>
    /// The progress bar created this way can only updated by percent.
    /// </remarks>
    public ProgressBar() => _color = Console.ForegroundColor;

    /// <summary>
    /// Create a progress bar with specified total number.
    /// </summary>
    /// <param name="total">Total number which indicates the 100% progress.</param>
    public ProgressBar(long total)
    {
        _total = total;
        _color = Console.ForegroundColor;
    }

    /// <summary>
    /// Create a progress bar with specified total number and a specified color.
    /// </summary>
    /// <param name="total">Total number which indicates the 100% progress.</param>
    /// <param name="color">The color used for the progressbar</param>
    public ProgressBar(long total, ConsoleColor color)
    {
        _total = total;
        _color = color;
    }

    /// <summary>
    /// Show the progress bar.
    /// </summary>
    public void Show()
    {
        if (_enabled == false)
        {
            _enabled = true;
            _cursorTop = Console.CursorTop;
            Console.WriteLine("0%");
        }
    }

    /// <summary>
    /// Update the progress by one item.
    /// </summary>
    public void UpdateOnce()
    {
        Update(++_count * 1.0 / _total);
    }

    /// <summary>
    /// Update the progress bar by count.
    /// </summary>
    /// <param name="count">The processed item count.</param>
    public void Update(long count)
    {
        this._count = count;
        Update(count * 1.0 / _total);
    }

    /// <summary>
    /// Update the progress by percent.
    /// </summary>
    /// <param name="percent">The processed percentage.</param>
    public void Update(double percent)
    {
        if (_enabled == false)
        {
            return;
        }
        // Update only when percentage reaches a higher integer.
        if (Math.Round(percent * 100) <= this._percentage)
        {
            return;
        }
        var originCursorTop = Console.CursorTop;
        var originCursorLeft = Console.CursorLeft;
        var originBackgroundColor = Console.BackgroundColor;
        var originForegroundColor = Console.ForegroundColor;

        int width = Console.WindowWidth - TextWidth;
        _percentage = (int)Math.Round(percent * 100);

        // Write percentage text.
        Console.SetCursorPosition(0, _cursorTop);
        Console.Write(new string(' ', TextWidth));
        Console.SetCursorPosition(0, _cursorTop);
        Console.Write($"{_percentage}%");

        // Print progress bar.
        Console.BackgroundColor = _color;
        var newCursorLeft = (int)Math.Round(percent * width);
        for (var cursor = _cursorLeft; cursor < newCursorLeft; cursor++)
        {
            Console.SetCursorPosition(TextWidth + cursor, _cursorTop);
            Console.Write(' ');
        }

        // Restore original color and cursor position.
        Console.BackgroundColor = originBackgroundColor;
        Console.ForegroundColor = originForegroundColor;
        Console.CursorTop = originCursorTop;
        Console.CursorLeft = originCursorLeft;

        // Record the drawn position.
        _cursorLeft = newCursorLeft;
    }
}