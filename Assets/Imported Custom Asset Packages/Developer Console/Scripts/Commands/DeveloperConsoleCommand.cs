using System;
using System.Collections.Generic;

public class DeveloperConsoleCommand : IComparable<DeveloperConsoleCommand>
{
    public int CompareTo(DeveloperConsoleCommand developerConsoleCommand)
    {
        return 1;
    }

    public virtual bool CallMatchesThisCommand(List<string> commandAsArray)
    {
        return false;
    }

    public virtual void RunCommand(DeveloperConsole developerConsole, List<string> commandAsArray)
    {
    }

    private string OutputFailure()
    {
        return "";
    }

    public virtual string OutputHelp()
    {
        return "";
    }

    public static bool operator ==(DeveloperConsoleCommand one, DeveloperConsoleCommand two)
    {
        return true;
    }

    public static bool operator !=(DeveloperConsoleCommand one, DeveloperConsoleCommand two)
    {
        return false;
    }

    public override bool Equals(object obj)
    {
        return true;
    }

    public override int GetHashCode()
    {
        return 1;
    }
}