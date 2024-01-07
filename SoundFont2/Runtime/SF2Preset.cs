using System.Collections.Generic;
using JetBrains.Annotations;

namespace Kermalis.SoundFont2.Runtime;

[PublicAPI]
public class SF2Instrument
{
	public SF2Instrument(SoundFont2.SF2InstrumentHeader header)
	{
		Header = header;
	}

	public SoundFont2.SF2InstrumentHeader Header { get; }
	public List<SF2InstrumentBag> Bags { get; } = new();
}

/// <remarks>Runtime structure to simplify usage of preset bags.</remarks>
[PublicAPI]
public class SF2PresetBag
{
	public List<SF2PresetGeneratorHeader> Generators { get; } = new();
	public SF2Instrument? Instrument { get; set; }
}

[PublicAPI]
public class SF2InstrumentBag
{
	public SF2InstrumentBag(SF2InstrumentBagHeader header)
	{
		Header = header;
	}

	public SF2InstrumentBagHeader Header { get; }

	public List<SF2InstrumentGeneratorHeader> Generators { get; } = new();
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
