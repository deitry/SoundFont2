using System.Collections;
using Kermalis.EndianBinaryIO;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Kermalis.SoundFont2
{
	[PublicAPI]
	public sealed class PHDRSubChunk : SF2Chunk, IList<SF2PresetHeader>
	{
		private readonly List<SF2PresetHeader> _presets = new();

		public void Add(SF2PresetHeader item)
		{
			_presets.Add(item);
			Size = (uint) Count * SF2PresetHeader.SIZE;
			_sf2.UpdateSize();
		}

		public void Clear() => throw new System.NotImplementedException();

		public bool Contains(SF2PresetHeader item) => _presets.Contains(item);

		public void CopyTo(SF2PresetHeader[] array, int arrayIndex) => throw new System.NotImplementedException();

		public bool Remove(SF2PresetHeader item) => throw new System.NotImplementedException();

		public int Count => _presets.Count;

		public bool IsReadOnly => false;

		internal PHDRSubChunk(SF2 inSf2) : base(inSf2, "phdr") { }
		internal PHDRSubChunk(SF2 inSf2, EndianBinaryReader reader) : base(inSf2, reader)
		{
			for (int i = 0; i < Size / SF2PresetHeader.SIZE; i++)
			{
				_presets.Add(new SF2PresetHeader(reader));
			}
		}

		public SF2PresetHeader? GetByName(string presetName) => _presets.FirstOrDefault(p => p.PresetName == presetName);

		internal override void Write(EndianBinaryWriter writer)
		{
			base.Write(writer);
			for (int i = 0; i < Count; i++)
			{
				_presets[i].Write(writer);
			}
		}

		public IEnumerator<SF2PresetHeader> GetEnumerator() => _presets.GetEnumerator();

		public override string ToString()
		{
			return $"Preset Header Chunk - Preset count = {Count}";
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int IndexOf(SF2PresetHeader item) => _presets.IndexOf(item);

		public void Insert(int index, SF2PresetHeader item) => throw new System.NotImplementedException();

		public void RemoveAt(int index) => throw new System.NotImplementedException();

		public SF2PresetHeader this[int index]
		{
			get => _presets[index];
			set => _presets[index] = value;
		}
		public SF2PresetHeader? this[string presetName]
		{
			get => _presets.FirstOrDefault(p => p.PresetName == presetName);
		}
	}
}
