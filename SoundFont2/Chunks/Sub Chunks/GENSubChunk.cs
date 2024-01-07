using System.Collections;
using Kermalis.EndianBinaryIO;
using System.Collections.Generic;

namespace Kermalis.SoundFont2
{
	public sealed class GENSubChunk<T> : SF2Chunk, IList<T>
		where T : SF2GeneratorHeader
	{
		private readonly List<T> _generators = new();
		public int Count => _generators.Count;

		internal GENSubChunk(SF2 inSf2, bool isPreset) : base(inSf2, isPreset ? "pgen" : "igen") { }
		internal GENSubChunk(SF2 inSf2, EndianBinaryReader reader) : base(inSf2, reader)
		{
			for (int i = 0; i < Size / SF2GeneratorHeader.SIZE; i++)
			{
				_generators.Add(SF2GeneratorHeader.Create<T>(reader));
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

		public IEnumerator<T> GetEnumerator() => _generators.GetEnumerator();

		public override string ToString()
		{
			return $"Generator Chunk - Name = \"{ChunkName}\"" +
				$",\nGenerator count = {Count}";
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(T item)
		{
			_generators.Add(item);
			Size = (uint)Count * SF2GeneratorHeader.SIZE;
			_sf2.UpdateSize();
		}

		public void Clear() => _generators.Clear();

		public bool Contains(T item) => _generators.Contains(item);

		public void CopyTo(T[] array, int arrayIndex) => _generators.CopyTo(array, arrayIndex);

		public bool Remove(T item) => throw new System.NotImplementedException();

		public bool IsReadOnly => false;

		public int IndexOf(T item) => _generators.IndexOf(item);

		public void Insert(int index, T item) => throw new System.NotImplementedException();
		public void RemoveAt(int index) => throw new System.NotImplementedException();

		public T this[int index]
		{
			get => _generators[index];
			set => _generators[index] = value;
		}
	}
}
