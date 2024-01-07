using Kermalis.EndianBinaryIO;
using System;
using System.IO;
using JetBrains.Annotations;
using Kermalis.SoundFont2.Runtime;

namespace Kermalis.SoundFont2
{
	[PublicAPI]
	public sealed class SF2
	{
		private uint _size;

		/// <summary>Gives basic information about the SoundFont compatible bank that is contained in the file.</summary>
		public InfoListChunk InfoChunk { get; }

		/// <summary>Contains a single optional smpl sub-chunk which contains all the RAM based sound data associated with the SoundFont compatible bank</summary>
		public SdtaListChunk SoundChunk { get; }

		/// <summary>The articulation data within a SoundFont 2</summary>
		public PdtaListChunk HydraChunk { get; }

		/// <summary>For creating</summary>
		private SF2()
		{
			InfoChunk = new InfoListChunk(this);
			SoundChunk = new SdtaListChunk(this);
			HydraChunk = new PdtaListChunk(this);
		}

		public static SF2 Create() => new SF2();

		/// <summary>For reading</summary>
		public SF2(string path)
		{
			using (FileStream stream = File.Open(path, FileMode.Open))
			{
				var reader = new EndianBinaryReader(stream, ascii: true);
				string str = reader.ReadString_Count(4);
				if (str != "RIFF")
				{
					throw new InvalidDataException("RIFF header was not found at the start of the file.");
				}

				_size = reader.ReadUInt32();
				str = reader.ReadString_Count(4);
				if (str != "sfbk")
				{
					throw new InvalidDataException("sfbk header was not found at the expected offset.");
				}

				InfoChunk = new InfoListChunk(this, reader);
				SoundChunk = new SdtaListChunk(this, reader);
				HydraChunk = new PdtaListChunk(this, reader);
			}
		}

		public void Save(string path)
		{
			using (FileStream stream = File.Open(path, FileMode.Create))
			{
				var writer = new EndianBinaryWriter(stream, ascii: true);
				AddTerminals();

				writer.WriteChars_Count("RIFF", 4);
				writer.WriteUInt32(_size);
				writer.WriteChars_Count("sfbk", 4);

				InfoChunk.Write(writer);
				SoundChunk.Write(writer);
				HydraChunk.Write(writer);
			}
		}


		/// <summary>Returns sample index</summary>
		public uint AddSample(ReadOnlySpan<short> pcm16, string name, bool bLoop, uint loopPos, uint sampleRate, byte originalKey, sbyte pitchCorrection)
		{
			uint start = SoundChunk.SMPLSubChunk.AddSample(pcm16, bLoop, loopPos);
			// If the sample is looped the standard requires us to add the 8 bytes from the start of the loop to the end
			uint end, loopEnd, loopStart;

			uint len = (uint)pcm16.Length;
			if (bLoop)
			{
				end = start + len + 8;
				loopStart = start + loopPos; loopEnd = start + len;
			}
			else
			{
				end = start + len;
				loopStart = 0; loopEnd = 0;
			}

			return AddSampleHeader(name, start, end, loopStart, loopEnd, sampleRate, originalKey, pitchCorrection);
		}
		/// <summary>Returns instrument index</summary>
		public uint AddInstrument(string name)
		{
			return HydraChunk.INSTSubChunk.AddInstrument(new SF2Instrument(name, (ushort)HydraChunk.IBAGSubChunk.Count));
		}
		public void AddInstrumentBag()
		{
			HydraChunk.IBAGSubChunk.Add(new SF2Bag(this, false));
		}
		public void AddInstrumentModulator()
		{
			HydraChunk.IMODSubChunk.AddModulator(new SF2ModulatorList());
		}
		public void AddInstrumentGenerator()
		{
			HydraChunk.IGENSubChunk.Add(new SF2Generator());
		}
		public void AddInstrumentGenerator(SF2GeneratorType generator, SF2GeneratorAmount amount)
		{
			HydraChunk.IGENSubChunk.Add(new SF2Generator(generator, amount));
		}
		public void AddPreset(string name, ushort preset, ushort bank)
		{
			HydraChunk.PHDRSubChunk.Add(new SF2PresetHeader(name, preset, bank, (ushort)HydraChunk.PBAGSubChunk.Count));
		}
		public void AddPresetBag()
		{
			HydraChunk.PBAGSubChunk.Add(new SF2Bag(this, true));
		}
		public void AddPresetModulator()
		{
			HydraChunk.PMODSubChunk.AddModulator(new SF2ModulatorList());
		}
		public void AddPresetGenerator()
		{
			HydraChunk.PGENSubChunk.Add(new SF2Generator());
		}
		public void AddPresetGenerator(SF2GeneratorType generator, SF2GeneratorAmount amount)
		{
			HydraChunk.PGENSubChunk.Add(new SF2Generator(generator, amount));
		}

		private uint AddSampleHeader(string name, uint start, uint end, uint loopStart, uint loopEnd, uint sampleRate, byte originalKey, sbyte pitchCorrection)
		{
			return HydraChunk.SHDRSubChunk.AddSample(new SF2SampleHeader(name, start, end, loopStart, loopEnd, sampleRate, originalKey, pitchCorrection));
		}
		private void AddTerminals()
		{
			AddSampleHeader("EOS", 0, 0, 0, 0, 0, 0, 0);
			AddInstrument("EOI");
			AddInstrumentBag();
			AddInstrumentGenerator();
			AddInstrumentModulator();
			AddPreset("EOP", 0xFF, 0xFF);
			AddPresetBag();
			AddPresetGenerator();
			AddPresetModulator();
		}

		internal void UpdateSize()
		{
			if (InfoChunk is null || SoundChunk is null || HydraChunk is null)
			{
				return; // The object may not be finished constructing yet
			}
			_size = 4
				+ InfoChunk.UpdateSize() + 8
				+ SoundChunk.UpdateSize() + 8
				+ HydraChunk.UpdateSize() + 8;
		}

		public SF2Preset? GetPreset(string presetName)
		{
			var header = HydraChunk.PHDRSubChunk[presetName];
			if (header is null)
				return null;

			var pHeaderNdx = HydraChunk.PHDRSubChunk.IndexOf(header);
			var nextPHeader = pHeaderNdx < HydraChunk.PHDRSubChunk.Count - 1
				? HydraChunk.PHDRSubChunk[pHeaderNdx + 1]
				: null;

			var lastPBagNdx = nextPHeader?.PresetBagIndex - 1 ?? HydraChunk.PBAGSubChunk.Count - 1;
			// var pBagCnt = lastPBagNdx - header.PresetBagIndex + 1;

			var preset = new SF2Preset(header);
			for (var i = header.PresetBagIndex; i <= lastPBagNdx; i++)
			{
				var pBag = HydraChunk.PBAGSubChunk[header.PresetBagIndex];

				var pGenNdx = pBag.GeneratorIndex;
				var nextPBag = i + 1 < HydraChunk.PBAGSubChunk.Count ? HydraChunk.PBAGSubChunk[i + 1] : null;
				var lastPGenNdx = nextPBag?.GeneratorIndex - 1 ?? HydraChunk.PGENSubChunk.Count - 1;

				var bag = new Runtime.SF2PresetBag();

				for (var j = pGenNdx; j <= lastPGenNdx; j++)
				{
					var pGen = HydraChunk.PGENSubChunk[j];
					bag.Generators.Add(pGen);

					if (pGen.Generator == SF2GeneratorType.Instrument)
					{
						var inst = HydraChunk.INSTSubChunk[pGen.GeneratorAmount.Amount];
						if (inst is null)
							throw new InvalidDataException("Instrument not found");

						bag.Instrument = inst;
					}
				}

				preset.Bags.Add(bag);
			}

			return preset;
		}
	}
}
