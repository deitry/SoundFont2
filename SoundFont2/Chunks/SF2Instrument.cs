using Kermalis.EndianBinaryIO;
using Microsoft.VisualBasic.CompilerServices;

namespace Kermalis.SoundFont2
{
	/// <summary>
	/// A collection of zones which represents the sound of a single musical instrument or sound effect set.
	/// </summary>
	public sealed class SF2Instrument
	{
		public const uint SIZE = 22;

		/// <summary>Length 20</summary>
		public string InstrumentName { get; set; }

		/// <summary>
		/// An index to the instrument’s zone list in the IBAG sub-chunk.
		/// </summary>
		[OriginalName("wInstBagNdx")]
		public ushort InstrumentBagIndex { get; set; }

		internal SF2Instrument(string name, ushort index)
		{
			InstrumentName = name;
			InstrumentBagIndex = index;
		}
		internal SF2Instrument(EndianBinaryReader reader)
		{
			InstrumentName = reader.ReadString_Count_TrimNullTerminators(20);
			InstrumentBagIndex = reader.ReadUInt16();
		}

		internal void Write(EndianBinaryWriter writer)
		{
			writer.WriteChars_Count(InstrumentName, 20);
			writer.WriteUInt16(InstrumentBagIndex);
		}

		public override string ToString()
		{
			return $"Instrument - Name = \"{InstrumentName}\"";
		}
	}
}
