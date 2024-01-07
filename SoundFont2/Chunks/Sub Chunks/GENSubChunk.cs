using System.Collections;
using Kermalis.EndianBinaryIO;
using System.Collections.Generic;

namespace Kermalis.SoundFont2
{
	public sealed class GENSubChunk : SF2Chunk, IList<SF2Generator>
	{
		private readonly List<SF2Generator> _generators = new();
		public int Count => _generators.Count;

		internal GENSubChunk(SF2 inSf2, bool isPreset) : base(inSf2, isPreset ? "pgen" : "igen") { }
		internal GENSubChunk(SF2 inSf2, EndianBinaryReader reader) : base(inSf2, reader)
		{
			for (int i = 0; i < Size / SF2Generator.SIZE; i++)
			{
				_generators.Add(new SF2Generator(reader));
			}
		}

		internal override void Write(EndianBinaryWriter writer)
		{
			base.Write(writer);
			for (int i = 0; i < Count; i++)
			{
				_generators[i].Write(writer);
			}
		}

		public IEnumerator<SF2Generator> GetEnumerator() => _generators.GetEnumerator();

		public override string ToString()
		{
			return $"Generator Chunk - Name = \"{ChunkName}\"" +
				$",\nGenerator count = {Count}";
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(SF2Generator item)
		{
			_generators.Add(item);
			Size = (uint)Count * SF2Generator.SIZE;
			_sf2.UpdateSize();
		}

		public void Clear() => _generators.Clear();

		public bool Contains(SF2Generator item) => _generators.Contains(item);

		public void CopyTo(SF2Generator[] array, int arrayIndex) => _generators.CopyTo(array, arrayIndex);

		public bool Remove(SF2Generator item) => throw new System.NotImplementedException();

		public bool IsReadOnly => false;

		public int IndexOf(SF2Generator item) => _generators.IndexOf(item);

		public void Insert(int index, SF2Generator item) => throw new System.NotImplementedException();
		public void RemoveAt(int index) => throw new System.NotImplementedException();

		public SF2Generator this[int index]
		{
			get => _generators[index];
			set => _generators[index] = value;
		}
	}
}
