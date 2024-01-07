using JetBrains.Annotations;
using Kermalis.EndianBinaryIO;

namespace Kermalis.SoundFont2
{
	[PublicAPI]
	public sealed class SF2SampleHeader
	{
		public const uint SIZE = 46;

		/// <summary>Length 20</summary>
		public string SampleName { get; set; }

		/// <summary>
		/// Contains the index, in sample data points, from the beginning of the sample data field to the first data point of this sample.
		/// </summary>
		public uint Start { get; set; }

		/// <summary>
		/// Contains the index, in sample data points, from the beginning of the sample data field to the first of the set of 46 zero valued data points following this sample.
		/// </summary>
		public uint End { get; set; }

		/// <summary>
		/// Contains the index, in sample data points, from the beginning of the sample data field to the first data point in the loop of this sample.
		/// </summary>
		public uint LoopStart { get; set; }

		/// <summary>
		/// Contains the index, in sample data points, from the beginning of the sample data field to the first data point following the loop of this sample.
		/// </summary>
		/// <remarks>Note that this is the data point “equivalent to” the first loop data point, and that to produce portable artifact free loops, the eight proximal data points surrounding both the Startloop and Endloop points should be identical.</remarks>
		public uint LoopEnd { get; set; }

		/// <summary>
		/// Contains the sample rate, in hertz, at which this sample was acquired or to which it was most recently converted
		/// </summary>
		/// <remarks>
		/// Values of greater than 50000 or less than 400 may not be reproducible by some hardware platforms and should be avoided.
		/// A value of zero is illegal. If an illegal or impractical value is encountered, the nearest practical value should be used.
		/// </remarks>
		public uint SampleRate { get; set; }

		/// <summary>Contains the MIDI key number of the recorded pitch of the sample</summary>
		/// <remarks>
		/// For example, a recording of an instrument playing middle C (261.62 Hz) should receive a value of 60.
		/// This value is used as the default “root key” for the sample, so that in the example, a MIDI key-on
		/// command for note number 60 would reproduce the sound at its original pitch. For unpitched sounds,
		/// a conventional value of 255 should be used. Values between 128 and 254 are illegal.
		/// Whenever an illegal value or a value of 255 is encountered, the value 60 should be used
		/// </remarks>
		[OriginalName("byOriginalPitch")]
		public byte OriginalKey { get; set; }

		/// <summary>Contains a pitch correction in cents that should be applied to the sample on playback</summary>
		/// <remarks>
		/// The purpose of this field is to compensate for any pitch errors during the sample recording process.
		/// The correction value is that of the correction to be applied. For example, if the sound is 4 cents sharp,
		/// a correction bringing it 4 cents flat is required; thus the value should be -4.
		/// </remarks>
		public sbyte PitchCorrection { get; set; }

		/// <remarks>
		/// If sfSampleType indicates a mono sample, then wSampleLink is undefined and its value should be conventionally zero, but
		/// will be ignored regardless of value.<p/>
		/// If sfSampleType indicates a left or right sample, then wSampleLink is the sample
		/// header index of the associated right or left stereo sample respectively. Both samples should be played entirely syncrhonously,
		/// with their pitch controlled by the right sample’s generators. All non-pitch generators should apply as normal; in particular
		/// the panning of the individual samples to left and right should be accomplished via the pan generator.
		/// Left-right pairs should always be found within the same instrument. Note also that no instrument should be designed in which it is possible to activate more than one instance of a particular stereo pair. The linked sample type is not currently fully defined in the SoundFont 2 specification, but will ultimately support a circularly linked list of samples using wSampleLink. Note that this enumeration is two bytes in length.
		/// </remarks>
		public ushort SampleLink { get; set; }

		[OriginalName("sfSampleType")]
		public SF2SampleLink SampleType { get; set; }

		internal SF2SampleHeader(string name, uint start, uint end, uint loopStart, uint loopEnd, uint sampleRate,
			byte originalKey, sbyte pitchCorrection)
		{
			SampleName = name;
			Start = start;
			End = end;
			LoopStart = loopStart;
			LoopEnd = loopEnd;
			SampleRate = sampleRate;
			OriginalKey = originalKey;
			PitchCorrection = pitchCorrection;
			SampleType = SF2SampleLink.MonoSample;
		}

		internal SF2SampleHeader(EndianBinaryReader reader)
		{
			SampleName = reader.ReadString_Count_TrimNullTerminators(20);
			Start = reader.ReadUInt32();
			End = reader.ReadUInt32();
			LoopStart = reader.ReadUInt32();
			LoopEnd = reader.ReadUInt32();
			SampleRate = reader.ReadUInt32();
			OriginalKey = reader.ReadByte();
			PitchCorrection = reader.ReadSByte();
			SampleLink = reader.ReadUInt16();
			SampleType = reader.ReadEnum<SF2SampleLink>();
		}

		internal void Write(EndianBinaryWriter writer)
		{
			writer.WriteChars_Count(SampleName, 20);
			writer.WriteUInt32(Start);
			writer.WriteUInt32(End);
			writer.WriteUInt32(LoopStart);
			writer.WriteUInt32(LoopEnd);
			writer.WriteUInt32(SampleRate);
			writer.WriteByte(OriginalKey);
			writer.WriteSByte(PitchCorrection);
			writer.WriteUInt16(SampleLink);
			writer.WriteEnum(SampleType);
		}

		public override string ToString()
		{
			return $"Sample - Name = \"{SampleName}\"" + $",\nType = {SampleType}";
		}
	}
}
