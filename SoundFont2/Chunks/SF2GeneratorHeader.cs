using JetBrains.Annotations;
using Kermalis.EndianBinaryIO;

namespace Kermalis.SoundFont2
{
	[PublicAPI]
	public sealed class SF2PresetGeneratorHeader : SF2GeneratorHeader
	{
		internal SF2PresetGeneratorHeader() { }
		internal SF2PresetGeneratorHeader(SF2GeneratorType generator, SF2GeneratorAmount amount)
		{
			Generator = generator;
			GeneratorAmount = amount;
		}
		internal SF2PresetGeneratorHeader(EndianBinaryReader reader)
		{
			Generator = reader.ReadEnum<SF2GeneratorType>();
			GeneratorAmount = new SF2GeneratorAmount { Amount = reader.ReadInt16() };
		}

		public override string ToString()
		{
			return $"Preset Generator = {Generator}" +
				$",\nAmount = \"{GeneratorAmount}\"";
		}
	}

	[PublicAPI]
	public sealed class SF2InstrumentGeneratorHeader : SF2GeneratorHeader
	{
		internal SF2InstrumentGeneratorHeader() { }
		internal SF2InstrumentGeneratorHeader(SF2GeneratorType generator, SF2GeneratorAmount amount)
		{
			Generator = generator;
			GeneratorAmount = amount;
		}
		internal SF2InstrumentGeneratorHeader(EndianBinaryReader reader)
		{
			Generator = reader.ReadEnum<SF2GeneratorType>();
			GeneratorAmount = new SF2GeneratorAmount { Amount = reader.ReadInt16() };
		}

		public override string ToString()
		{
			return $"Instrument Generator = {Generator}" +
				$",\nAmount = \"{GeneratorAmount}\"";
		}
	}

	[PublicAPI]
	[OriginalName("sfGenList")]
	public abstract class SF2GeneratorHeader
	{
		public const uint SIZE = 4;

		public static T Create<T>(EndianBinaryReader reader) where T : SF2GeneratorHeader
		{
			return typeof(T).Name switch
			{
				nameof(SF2PresetGeneratorHeader) => (T)(SF2GeneratorHeader)new SF2PresetGeneratorHeader(reader),
				nameof(SF2InstrumentGeneratorHeader) => (T)(SF2GeneratorHeader)new SF2InstrumentGeneratorHeader(reader),
				_ => throw new System.NotImplementedException(),
			};
		}

		/// <summary>
		/// A value of one of the SFGenerator enumeration type values. Unknown or undefined values are ignored.
		/// This value indicates the type of generator being indicated. Note that this enumeration is two bytes in length.
		/// </summary>
		[OriginalName("sfGenOper")]
		public SF2GeneratorType Generator { get; set; }

		/// <summary>
		/// The value to be assigned to the specified generator.
		/// </summary>
		/// <remarks>
		/// Note that this can be of three formats.
		/// Certain generators specify a range of MIDI key numbers of MIDI velocities, with a minimum and maximum value.
		/// Other generators specify an unsigned WORD value. Most generators, however, specify a signed 16 bit SHORT value.
		/// </remarks>
		public SF2GeneratorAmount GeneratorAmount { get; set; }

		internal SF2GeneratorHeader() { }
		internal SF2GeneratorHeader(SF2GeneratorType generator, SF2GeneratorAmount amount)
		{
			Generator = generator;
			GeneratorAmount = amount;
		}
		internal SF2GeneratorHeader(EndianBinaryReader reader)
		{
			Generator = reader.ReadEnum<SF2GeneratorType>();
			GeneratorAmount = new SF2GeneratorAmount { Amount = reader.ReadInt16() };
		}

		public void Write(EndianBinaryWriter writer)
		{
			writer.WriteEnum(Generator);
			writer.WriteInt16(GeneratorAmount.Amount);
		}

		public override string ToString()
		{
			return $"Generator List - Generator = {Generator}" +
				$",\nGenerator amount = \"{GeneratorAmount}\"";
		}
	}
}
