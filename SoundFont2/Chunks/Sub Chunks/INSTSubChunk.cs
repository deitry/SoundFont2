using System.Collections;
using Kermalis.EndianBinaryIO;
using System.Collections.Generic;

namespace Kermalis.SoundFont2
{
	public sealed class INSTSubChunk : SF2Chunk, IList<SF2InstrumentHeader>
	{
		private readonly List<SF2InstrumentHeader> _instruments = new();

		public void Clear() => throw new System.NotImplementedException();

		public bool Contains(SF2InstrumentHeader item) => _instruments.Contains(item);

		public void CopyTo(SF2InstrumentHeader[] array, int arrayIndex) => throw new System.NotImplementedException();

		public bool Remove(SF2InstrumentHeader item) => throw new System.NotImplementedException();

		public int Count => _instruments.Count;
		public bool IsReadOnly => false;

		internal INSTSubChunk(SF2 inSf2) : base(inSf2, "inst") { }
		internal INSTSubChunk(SF2 inSf2, EndianBinaryReader reader) : base(inSf2, reader)
		{
			for (int i = 0; i < Size / SF2InstrumentHeader.SIZE; i++)
			{
				_instruments.Add(new SF2InstrumentHeader(reader));
			}
		}

		public void Add(SF2InstrumentHeader instrumentHeader)
		{
			_instruments.Add(instrumentHeader);
			Size = (uint)Count * SF2InstrumentHeader.SIZE;
			_sf2.UpdateSize();
			// return Count - 1;
		}
		public uint AddInstrument(SF2InstrumentHeader instrumentHeader)
		{
			var cnt = (uint)Count;
			_instruments.Add(instrumentHeader);
			Size = cnt * SF2InstrumentHeader.SIZE;
			_sf2.UpdateSize();
			return cnt - 1;
		}

		internal override void Write(EndianBinaryWriter writer)
		{
			base.Write(writer);
			for (int i = 0; i < Count; i++)
			{
				_instruments[i].Write(writer);
			}
		}

		public IEnumerator<SF2InstrumentHeader> GetEnumerator() => _instruments.GetEnumerator();

		public override string ToString()
		{
			return $"Instrument Chunk - Instrument count = {Count}";
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int IndexOf(SF2InstrumentHeader item) => _instruments.IndexOf(item);

		public void Insert(int index, SF2InstrumentHeader item) => throw new System.NotImplementedException();

		public void RemoveAt(int index) => throw new System.NotImplementedException();

		public SF2InstrumentHeader this[int index]
		{
			get => _instruments[index];
			set => _instruments[index] = value;
		}
	}
}
