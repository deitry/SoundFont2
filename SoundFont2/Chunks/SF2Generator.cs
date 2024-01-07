using JetBrains.Annotations;
using Kermalis.EndianBinaryIO;

namespace Kermalis.SoundFont2
{
	[PublicAPI]
	[OriginalName("sfGenList")]
	public sealed class SF2Generator
	{
		public const uint SIZE = 4;

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

		internal SF2Generator() { }
		internal SF2Generator(SF2GeneratorType generator, SF2GeneratorAmount amount)
		{
			Generator = generator;
			GeneratorAmount = amount;
		}
		internal SF2Generator(EndianBinaryReader reader)
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
