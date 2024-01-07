using System.Collections;
using Kermalis.EndianBinaryIO;
using System.Collections.Generic;

namespace Kermalis.SoundFont2
{
	public sealed class BAGSubChunk<T> : SF2Chunk, IList<T>
		where T : SF2BagHeader
	{
		private readonly List<T> _bags = new();

		internal BAGSubChunk(SF2 inSf2, bool isPreset) : base(inSf2, isPreset ? "pbag" : "ibag") { }
		internal BAGSubChunk(SF2 inSf2, EndianBinaryReader reader) : base(inSf2, reader)
		{
			for (int i = 0; i < Size / SF2BagHeader.SIZE; i++)
			{
				_bags.Add(SF2BagHeader.Create<T>(reader));
			}
		}

		internal override void Write(EndianBinaryWriter writer)
		{
			base.Write(writer);
			for (int i = 0; i < Count; i++)
			{
				_bags[i].Write(writer);
			}
		}

		public IEnumerator<T> GetEnumerator() => _bags.GetEnumerator();

		public override string ToString()
		{
			return $"Bag Chunk - Name = \"{ChunkName}\"" +
				$",\nBag count = {Count}";
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(T item)
		{
			_bags.Add(item);
			Size = (uint)Count * SF2BagHeader.SIZE;
			_sf2.UpdateSize();
		}

		public void Clear() => _bags.Clear();

		public bool Contains(T item) => _bags.Contains(item);

		public void CopyTo(T[] array, int arrayIndex) => _bags.CopyTo(array, arrayIndex);

		public bool Remove(T item) => throw new System.NotImplementedException();

		public int Count => _bags.Count;
		public bool IsReadOnly => false;
		public int IndexOf(T item) => _bags.IndexOf(item);

		public void Insert(int index, T item) => throw new System.NotImplementedException();

		public void RemoveAt(int index) => throw new System.NotImplementedException();

		public T this[int index]
		{
			get => _bags[index];
			set => _bags[index] = value;
		}
	}
}
