using Kermalis.EndianBinaryIO;

namespace Kermalis.SoundFont2
{
	public sealed class PdtaListChunk : SF2ListChunk
	{
		/// <summary>
		/// Lists all presets within the SoundFont
		/// </summary>
		public PHDRSubChunk PHDRSubChunk { get; }

		/// <summary>
		/// Lists all preset zones within the SoundFont
		/// </summary>
		public BAGSubChunk PBAGSubChunk { get; }

		/// <summary>Lists all preset zone modulators within the SoundFont</summary>
		public MODSubChunk PMODSubChunk { get; }

		/// <summary>Contains a list of preset zone generators for each preset zone within the SoundFont</summary>
		public GENSubChunk PGENSubChunk { get; }

		/// <summary>Lists all instruments within the SoundFont</summary>
		public INSTSubChunk INSTSubChunk { get; }

		/// <summary>Lists all instrument zones within the SoundFont</summary>
		public BAGSubChunk IBAGSubChunk { get; }

		/// <summary>Lists all instrument zone modulators within the SoundFont</summary>
		public MODSubChunk IMODSubChunk { get; }

		/// <summary>Contains a list of zone generators for each instrument zone within the SoundFont</summary>
		public GENSubChunk IGENSubChunk { get; }

		/// <summary>Lists all samples within the smpl sub-chunk and any referenced ROM samples</summary>
		public SHDRSubChunk SHDRSubChunk { get; }

		internal PdtaListChunk(SF2 inSf2) : base(inSf2, "pdta")
		{
			PHDRSubChunk = new PHDRSubChunk(inSf2);
			PBAGSubChunk = new BAGSubChunk(inSf2, true);
			PMODSubChunk = new MODSubChunk(inSf2, true);
			PGENSubChunk = new GENSubChunk(inSf2, true);
			INSTSubChunk = new INSTSubChunk(inSf2);
			IBAGSubChunk = new BAGSubChunk(inSf2, false);
			IMODSubChunk = new MODSubChunk(inSf2, false);
			IGENSubChunk = new GENSubChunk(inSf2, false);
			SHDRSubChunk = new SHDRSubChunk(inSf2);
			inSf2.UpdateSize();
		}
		internal PdtaListChunk(SF2 inSf2, EndianBinaryReader reader) : base(inSf2, reader)
		{
			PHDRSubChunk = new PHDRSubChunk(inSf2, reader);
			PBAGSubChunk = new BAGSubChunk(inSf2, reader);
			PMODSubChunk = new MODSubChunk(inSf2, reader);
			PGENSubChunk = new GENSubChunk(inSf2, reader);
			INSTSubChunk = new INSTSubChunk(inSf2, reader);
			IBAGSubChunk = new BAGSubChunk(inSf2, reader);
			IMODSubChunk = new MODSubChunk(inSf2, reader);
			IGENSubChunk = new GENSubChunk(inSf2, reader);
			SHDRSubChunk = new SHDRSubChunk(inSf2, reader);
		}

		internal override uint UpdateSize()
		{
			return Size = 4
				+ PHDRSubChunk.Size + 8
				+ PBAGSubChunk.Size + 8
				+ PMODSubChunk.Size + 8
				+ PGENSubChunk.Size + 8
				+ INSTSubChunk.Size + 8
				+ IBAGSubChunk.Size + 8
				+ IMODSubChunk.Size + 8
				+ IGENSubChunk.Size + 8
				+ SHDRSubChunk.Size + 8;
		}

		internal override void Write(EndianBinaryWriter writer)
		{
			base.Write(writer);
			PHDRSubChunk.Write(writer);
			PBAGSubChunk.Write(writer);
			PMODSubChunk.Write(writer);
			PGENSubChunk.Write(writer);
			INSTSubChunk.Write(writer);
			IBAGSubChunk.Write(writer);
			IMODSubChunk.Write(writer);
			IGENSubChunk.Write(writer);
			SHDRSubChunk.Write(writer);
		}

		public override string ToString()
		{
			return $"Hydra List Chunk";
		}
	}
}
