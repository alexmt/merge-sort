using System.IO;

namespace TestTask.MergeSort.IO
{
    public class BinaryIntFormatter : IStreamFormatter<int, BinaryReader, BinaryWriter>
    {
        public BinaryReader CreateReader(Stream stream)
        {
            return new BinaryReader(stream);
        }

        public bool ParseNext(BinaryReader reader, out int element)
        {
            if(reader.BaseStream.Position < reader.BaseStream.Length)
            {
                element = reader.ReadInt32();
                return true;
            }
            element = 0;
            return false;
        }

        public BinaryWriter CreateWriter(Stream stream)
        {
            return new BinaryWriter(stream);
        }

        public void Write(BinaryWriter writer, int element)
        {
            writer.Write(element);
        }
    }
}