using JetBrains.Annotations;
using Kermalis.EndianBinaryIO;

namespace Kermalis.SoundFont2
{
	[PublicAPI]
	public sealed class SF2PresetHeader
	{
		public const uint SIZE = 38;

		/// <remarks>Length 20</remarks>
		[OriginalName("achPresetName")]
		public string PresetName { get; set; }

		[OriginalName("wPreset")]
		public ushort Preset { get; set; }

		[OriginalName("wBank")]
		public ushort Bank { get; set; }

		/// <summary>The first zone in a given preset</summary>
		/// <remarks>The number of zones in the preset is determined by the difference between the next preset’s wPresetBagNdx and the current wPresetBagNdx.</remarks>
		[OriginalName("wPresetBagNdx")]
		public ushort PresetBagIndex { get; set; }

		// Reserved for future implementations
		private readonly uint _library;
		private readonly uint _genre;
		private readonly uint _morphology;

		internal SF2PresetHeader(string name, ushort preset, ushort bank, ushort index)
		{
			PresetName = name;
			Preset = preset;
			Bank = bank;
			PresetBagIndex = index;
		}

		internal SF2PresetHeader(EndianBinaryReader reader)
		{
			PresetName = reader.ReadString_Count_TrimNullTerminators(20);
			Preset = reader.ReadUInt16();
			Bank = reader.ReadUInt16();
			PresetBagIndex = reader.ReadUInt16();
			_library = reader.ReadUInt32();
			_genre = reader.ReadUInt32();
			_morphology = reader.ReadUInt32();
		}

		internal void Write(EndianBinaryWriter writer)
		{
			writer.WriteChars_Count(PresetName, 20);
			writer.WriteUInt16(Preset);
			writer.WriteUInt16(Bank);
			writer.WriteUInt16(PresetBagIndex);
			writer.WriteUInt32(_library);
			writer.WriteUInt32(_genre);
			writer.WriteUInt32(_morphology);
		}

		public override string ToString()
		{
			return $"Preset Header - Bank = {Bank}" +
				$",\nPreset = {Preset}" +
				$",\nName = \"{PresetName}\"";
		}
	}
}
