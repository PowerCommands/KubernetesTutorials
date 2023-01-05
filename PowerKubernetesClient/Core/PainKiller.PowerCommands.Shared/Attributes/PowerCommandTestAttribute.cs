using System.ComponentModel;

namespace PainKiller.PowerCommands.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PowerCommandTestAttribute : Attribute
{
    [Description("Disable the test for this command")]
    public bool Disabled { get; }
    [Description("Separate items with |, if you begin with // the value will be displayed as an comment row in test report.")]
    public string Tests { get; }
    [Description("Log is disabled by default during the test, it will be enabled after test is done.")]
    public bool DisableLog { get; }

    public PowerCommandTestAttribute(string tests, bool disableLog = true, bool disabled = false)
    {
        Disabled = disabled;
        Tests = tests;
        DisableLog = disableLog;
    }

    public PowerCommandTestAttribute()
    {
        Disabled = true;
        Tests = "tests";
        DisableLog = false;
    }
}