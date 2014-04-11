using System;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Timelapse_API
{
    public class ThumbHandler : IDisposable
    {
        private string TmpFile = Path.Combine(ProjectManager.ApplicationPath, "Thumb.tmp");
        private FixedSizedQueue<BufferEntry> Buffer;
        private List<DataEntry> Entries = new List<DataEntry>();
        private FileStream MainStream;
        private BinaryFormatter formatter = new BinaryFormatter();
        private BufferEntry tmpBuff;
        private BitmapEx tmpBmp;

        /// <summary>
        /// Initializes the ThumbHandler and opens stream to temporary file
        /// </summary>
        /// <param name="Buffersize">Number of buffered thumbs</param>
        public ThumbHandler(uint Buffersize)
        {
            Buffer = new FixedSizedQueue<BufferEntry>(Buffersize);
            MainStream = new FileStream(TmpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 80000000, FileOptions.RandomAccess);    //80MB buffer
        }

        /// <summary>
        /// Gets a thumb from the list
        /// </summary>
        /// <param name="index">Index of the thumb</param>
        /// <param name="UseBuffer">If true, it will add the file to the buffer</param>
        /// <returns>The thumb or null if not found</returns>
        public BitmapEx this[int index, bool UseBuffer = true]
        {
            get
            {
                if (index < 0 || index >= Entries.Count || !MainStream.CanRead) return null;

                tmpBuff = Buffer.FirstOrDefault(t => t.Index == index);
                if (tmpBuff != null) return tmpBuff.Value;

                MainStream.Position = Entries[index].Begin;
                if (UseBuffer)
                {
                    tmpBmp = (BitmapEx)formatter.Deserialize(MainStream);
                    Buffer.Enqueue(new BufferEntry(index, tmpBmp));
                    return tmpBmp;
                }
                else return (BitmapEx)formatter.Deserialize(MainStream);
            }
            set
            {
                if (index >= 0 && index < Entries.Count && MainStream.CanWrite)
                {
                    MainStream.Position = Entries[index].Begin;
                    formatter.Serialize(MainStream, value);
                }
            }
        }

        /// <summary>
        /// Closes all open streams and removes all data
        /// </summary>
        public void Dispose()
        {
            if (MainStream != null)
            {
                MainStream.Close();
                MainStream.Dispose();
            }
            Entries.Clear();
            Buffer = null;
            FileHandle.DeleteFile(TmpFile);
        }

        /// <summary>
        /// Adds a thumb to the list
        /// </summary>
        /// <param name="thumb">The thumb to be added</param>
        public void AddThumb(BitmapEx thumb)
        {
            if (MainStream.CanWrite)
            {
                if (thumb.IsPinned) thumb.UnlockBits();
                MainStream.Position = MainStream.Length;
                long begin = MainStream.Length;
                formatter.Serialize(MainStream, thumb);
                Entries.Add(new DataEntry(begin, MainStream.Length));
            }
        }

        private class DataEntry
        {
            public long Begin { get; private set; }
            public long End { get; private set; }
            public long Length { get { return End - Begin; } }

            public DataEntry(long Begin, long End)
            {
                this.Begin = Begin;
                this.End = End;
            }
        }

        private class BufferEntry
        {
            public int Index { get; private set; }
            public BitmapEx Value { get; private set; }

            public BufferEntry(int Index, BitmapEx Value)
            {
                this.Index = Index;
                this.Value = Value;
            }
        }

        private class FixedSizedQueue<T> : ConcurrentQueue<T>
        {
            public uint Size { get; private set; }
            private T outObj;

            public FixedSizedQueue(uint size)
            {
                Size = size;
            }

            public new void Enqueue(T obj)
            {
                base.Enqueue(obj);
                lock (this) { while (base.Count > Size && base.TryDequeue(out outObj)) { } }
            }
        }
    }
}
