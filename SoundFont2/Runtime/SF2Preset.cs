using System.Collections.Generic;
using JetBrains.Annotations;

namespace Kermalis.SoundFont2.Runtime;

/// <remarks>Runtime structure to simplify usage of preset bags.</remarks>
[PublicAPI]
public class SF2PresetBag
{
	public List<SF2Generator> Generators { get; } = new();
	public SF2Instrument? Instrument { get; set; }
}

/// <summary>
/// A keyboard full of sound. Typically the collection of samples and articulation data associated with a particular MIDI preset number
/// </summary>
/// <remarks>Runtime structure to simplify usage of presets.</remarks>
[PublicAPI]
public class SF2Preset
{
	public SF2Preset(SF2PresetHeader header)
	{
		Header = header;
	}

	public SF2PresetHeader Header { get; set; }

	public List<SF2PresetBag> Bags { get; } = new();
}
