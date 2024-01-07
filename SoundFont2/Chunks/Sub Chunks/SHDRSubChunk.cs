using System.Collections;
using Kermalis.EndianBinaryIO;
using System.Collections.Generic;

namespace Kermalis.SoundFont2
{
	public sealed class SHDRSubChunk : SF2Chunk, IList<SF2SampleHeader>
	{
		private readonly List<SF2SampleHeader> _samples = new();

		public void Add(SF2SampleHeader item) => AddSample(item);

		public void Clear() => throw new System.NotImplementedException();

		public bool Contains(SF2SampleHeader item) => _samples.Contains(item);

		public void CopyTo(SF2SampleHeader[] array, int arrayIndex) => throw new System.NotImplementedException();

		public bool Remove(SF2SampleHeader item) => throw new System.NotImplementedException();

		public int Count => _samples.Count;
		public bool IsReadOnly => false;

		internal SHDRSubChunk(SF2 inSf2) : base(inSf2, "shdr") { }
		internal SHDRSubChunk(SF2 inSf2, EndianBinaryReader reader) : base(inSf2, reader)
		{
			for (int i = 0; i < Size / SF2SampleHeader.SIZE; i++)
			{
				_samples.Add(new SF2SampleHeader(reader));
			}
		}

		internal uint AddSample(SF2SampleHeader sample)
		{
			_samples.Add(sample);
			Size = (uint)Count * SF2SampleHeader.SIZE;
			_sf2.UpdateSize();
			return (uint) Count - 1;
		}

		internal override void Write(EndianBinaryWriter writer)
		{
			base.Write(writer);
			for (int i = 0; i < Count; i++)
			{
				_samples[i].Write(writer);
			}
		}

		public IEnumerator<SF2SampleHeader> GetEnumerator() => _samples.GetEnumerator();

		public override string ToString()
		{
			return $"Sample Header Chunk - Sample header count = {Count}";
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int IndexOf(SF2SampleHeader item) => _samples.IndexOf(item);

		public void Insert(int index, SF2SampleHeader item) => throw new System.NotImplementedException();

		public void RemoveAt(int index) => throw new System.NotImplementedException();

		public SF2SampleHeader this[int index]
		{
			get => _samples[index];
			set => _samples[index] = value;
		}
	}
}
