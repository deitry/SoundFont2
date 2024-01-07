using System.Collections;
using Kermalis.EndianBinaryIO;
using System.Collections.Generic;

namespace Kermalis.SoundFont2
{
	public sealed class BAGSubChunk : SF2Chunk, IList<SF2Bag>
	{
		private readonly List<SF2Bag> _bags = new();

		internal BAGSubChunk(SF2 inSf2, bool isPreset) : base(inSf2, isPreset ? "pbag" : "ibag") { }
		internal BAGSubChunk(SF2 inSf2, EndianBinaryReader reader) : base(inSf2, reader)
		{
			for (int i = 0; i < Size / SF2Bag.SIZE; i++)
			{
				_bags.Add(new SF2Bag(reader));
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

		public IEnumerator<SF2Bag> GetEnumerator() => _bags.GetEnumerator();

		public override string ToString()
		{
			return $"Bag Chunk - Name = \"{ChunkName}\"" +
				$",\nBag count = {Count}";
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(SF2Bag item)
		{
			_bags.Add(item);
			Size = (uint)Count * SF2Bag.SIZE;
			_sf2.UpdateSize();
		}

		public void Clear() => _bags.Clear();

		public bool Contains(SF2Bag item) => _bags.Contains(item);

		public void CopyTo(SF2Bag[] array, int arrayIndex) => _bags.CopyTo(array, arrayIndex);

		public bool Remove(SF2Bag item) => throw new System.NotImplementedException();

		public int Count => _bags.Count;
		public bool IsReadOnly => false;
		public int IndexOf(SF2Bag item) => _bags.IndexOf(item);

		public void Insert(int index, SF2Bag item) => throw new System.NotImplementedException();

		public void RemoveAt(int index) => throw new System.NotImplementedException();

		public SF2Bag this[int index]
		{
			get => _bags[index];
			set => _bags[index] = value;
		}
	}
}
