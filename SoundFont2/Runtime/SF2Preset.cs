using System;
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

[PublicAPI]
public record SF2Sample
{
	public int Start { get; init; }
	public int End { get; init; }
	public int LoopStart { get; init; }
	public int LoopInt { get; init; }
	public IReadOnlyCollection<short> SampleData { get; init; }
}

/// <summary>
/// A keyboard full of sound. Typically the collection of samples and articulation data associated with a particular MIDI preset number
/// </summary>
/// <remarks>Runtime structure to simplify usage of presets.</remarks>
[PublicAPI]
public class SF2Preset(SF2 soundData, SF2PresetHeader header)
{
	public SF2 Sf2 { get; set; } = soundData;

	public SF2PresetHeader Header { get; set; } = header;

	public List<SF2PresetBag> Bags { get; } = new();

	public SF2Sample GetSample(int noteKey)
	{
		var bag = Bags.Find(b => b.Instrument != null
			// && b.Instrument.Header.KeyRange.Contains(noteKey)
			);

		if (bag == null)
			throw new Exception("No bag found for note key " + noteKey);

		// var sample = bag.Instrument!.Bags.Find(b => b.Header.GeneratorIndex == (int)SF2GeneratorType.SampleID);
		// if (sample == null)
		// 	throw new Exception("No sample found for note key " + noteKey);
		//
		// var sampleId = sample.Generators[0].GeneratorAmount.Amount;
		// var sampleHeader = Header.SF2.SampleHeaders[sampleId];
		return new SF2Sample
		{
		// 	LoopStart = sampleHeader.LoopStart,
		// 	LoopInt = sampleHeader.LoopEnd - sampleHeader.LoopStart,
		// 	SampleData = Header.SF2.SampleData.AsSpan(sampleHeader.Start, sampleHeader.End - sampleHeader.Start)
			// SampleData = Samples,
		};
	}
}
