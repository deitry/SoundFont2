using System;
using JetBrains.Annotations;

namespace Kermalis.SoundFont2;

/// <summary>
/// Helper class to indicate original name as listed in format specification
/// </summary>
[PublicAPI]
public class OriginalNameAttribute : Attribute
{
    public string Name { get; }

    public OriginalNameAttribute(string name)
    {
        Name = name;
    }
}
