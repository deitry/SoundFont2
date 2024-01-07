using System;
using JetBrains.Annotations;

namespace Kermalis.SoundFont2;

/// <summary>
/// Helper class to indicate original name as listed in format specification
/// </summary>
[PublicAPI]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class OriginalNameAttribute : Attribute
{
    public string Name { get; }

    public OriginalNameAttribute(string name)
    {
        Name = name;
    }
}
