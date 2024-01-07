using JetBrains.Annotations;
using Kermalis.EndianBinaryIO;

namespace Kermalis.SoundFont2
{
	[PublicAPI]
	public sealed class SF2PresetBagHeader : SF2BagHeader
	{
		public SF2PresetBagHeader(SF2 inSf2, bool isPresetBag) : base(inSf2, isPresetBag) { }

		public SF2PresetBagHeader(EndianBinaryReader reader) : base(reader) { }
	}

	[PublicAPI]
	public sealed class SF2InstrumentBagHeader : SF2BagHeader
	{
		public SF2InstrumentBagHeader(SF2 inSf2, bool isPresetBag) : base(inSf2, isPresetBag) { }

		public SF2InstrumentBagHeader(EndianBinaryReader reader) : base(reader) { }
	}

	/// <summary>A SoundFont data structure element containing a list of preset zones or instrument zones</summary>
	[OriginalName("sfPresetBag")]
	[OriginalName("sfInstBag")]
	public abstract class SF2BagHeader
	{
		public const uint SIZE = 4;

		public static T Create<T>(EndianBinaryReader reader) where T : SF2BagHeader
		{
			return typeof(T).Name switch
			{
				nameof(SF2PresetBagHeader) => (T)(SF2BagHeader)new SF2PresetBagHeader(reader),
				nameof(SF2InstrumentBagHeader) => (T)(SF2BagHeader)new SF2InstrumentBagHeader(reader),
				_ => throw new System.NotImplementedException()
			};
		}

		/// <summary>Index in list of generators</summary>
		/// <remarks>The preset zone’s wGenNdx points to the first generator for that preset zone. Unless the zone is a global zone, the last generator in the list is an “Instrument” generator, whose value is a pointer to the instrument associated with that zone. If a “key range” generator exists for the preset zone, it is always the first generator in the list for that preset zone. If a “velocity range” generator exists for the preset zone, it will only be preceded by a key range generator. If any generators follow an Instrument generator, they will be ignored. </remarks>
		[OriginalName("wGenNdx")]
		public ushort GeneratorIndex { get; set; }

		/// <summary>Index in list of modulators</summary>
		[OriginalName("wModNdx")]
		public ushort ModulatorIndex { get; set; }

		internal SF2BagHeader(SF2 inSf2, bool isPresetBag)
		{
			if (isPresetBag)
			{
				GeneratorIndex = (ushort)inSf2.HydraChunk.PGENSubChunk.Count;
				ModulatorIndex = (ushort)inSf2.HydraChunk.PMODSubChunk.Count;
			}
			else
			{
				GeneratorIndex = (ushort)inSf2.HydraChunk.IGENSubChunk.Count;
				ModulatorIndex = (ushort)inSf2.HydraChunk.IMODSubChunk.Count;
			}
		}
		internal SF2BagHeader(EndianBinaryReader reader)
		{
			GeneratorIndex = reader.ReadUInt16();
			ModulatorIndex = reader.ReadUInt16();
		}

		internal void Write(EndianBinaryWriter writer)
		{
			writer.WriteUInt16(GeneratorIndex);
			writer.WriteUInt16(ModulatorIndex);
		}

		public override string ToString()
		{
			return $"Bag - Generator index = {GeneratorIndex}" +
				$",\nModulator index = {ModulatorIndex}";
		}
	}
}
