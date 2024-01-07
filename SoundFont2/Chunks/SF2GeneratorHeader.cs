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

		public override string ToString() => "Preset " + base.ToString();
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

		public override string ToString() => "Instrument " + base.ToString();
	}

	/// <summary>
	/// Description of a single synthesizer parameter
	/// </summary>
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

		public override string ToString() =>
			KindOf(Generator) switch
			{
				GeneratorKind.Index => $"Generator = {Generator},\nIndex = {GeneratorAmount.UAmount}",
				GeneratorKind.Range => $"Generator = {Generator},\nRange = {GeneratorAmount.LowByte} .. {GeneratorAmount.HighByte}",
				GeneratorKind.Sample => $"Generator = {Generator},\nSample = {GeneratorAmount.UAmount}",
				_ => $"Generator = {Generator},\nGenerator amount = \"{GeneratorAmount}\"",
			};

		[PublicAPI]
		public enum GeneratorKind
		{
			/// <summary>Value Generators are generators whose value directly affects a signal processing parameter.</summary>
			Value,

			/// <summary>An Index Generator’s amount is an index into another data structure.</summary>
			Index,

			/// <summary>Defines a range of note-on parameters outside of which the zone is undefined.</summary>
			Range,

			/// <summary>Generators which directly affect a sample’s properties. These generators are illegal at the preset level.</summary>
			Sample,

			/// <summary>Substitution Generators are generators which substitute a value for a note-on parameter. These generators are illegal at the preset level.</summary>
			Substitution,
		}

		public static GeneratorKind KindOf(SF2GeneratorType type)
		{
			return type switch
			{
				SF2GeneratorType.Instrument => GeneratorKind.Index,
				SF2GeneratorType.SampleID => GeneratorKind.Index,

				SF2GeneratorType.KeyRange => GeneratorKind.Range,
				SF2GeneratorType.VelRange => GeneratorKind.Range,

				// these types are mentioned in the documentation but not present in enum
				// SF2GeneratorType.OverridingKeyNumber => SF2GeneratorType.Substitution,
				// SF2GeneratorType.OverridingVelocity => SF2GeneratorType.Substitution,

				SF2GeneratorType.OverridingRootKey => GeneratorKind.Sample,
				SF2GeneratorType.SampleModes => GeneratorKind.Sample,
				SF2GeneratorType.ExclusiveClass => GeneratorKind.Sample,

				SF2GeneratorType.StartAddrsOffset => GeneratorKind.Sample,
				SF2GeneratorType.  EndAddrsOffset => GeneratorKind.Sample,

				SF2GeneratorType.StartloopAddrsOffset => GeneratorKind.Sample,
				SF2GeneratorType.  EndloopAddrsOffset => GeneratorKind.Sample,

				SF2GeneratorType.StartAddrsCoarseOffset => GeneratorKind.Sample,
				SF2GeneratorType.  EndAddrsCoarseOffset => GeneratorKind.Sample,

				SF2GeneratorType.StartloopAddrsCoarseOffset => GeneratorKind.Sample,
				SF2GeneratorType.  EndloopAddrsCoarseOffset => GeneratorKind.Sample,

				_ => GeneratorKind.Value,
			};
		}
	}
}
