using System.Collections.Generic;
using System.IO;

namespace Drafts.SaveData
{
    public static class BinaryFileParserExtensions
    {
        public static void Write<T>(this BinaryWriter writer, IReadOnlyCollection<T> list) where T : IBinarySave
        {
            writer.Write(list.Count);

            foreach (var item in list)
            {
                writer.Write(item != null);
                item?.Save(writer);
            }
        }

        public static void Read<T>(this BinaryReader reader, IList<T> list) where T : IBinarySave, new()
        {
            var index = 0;

            foreach (var item in ReadCollection<T>(reader))
            {
                if (index >= list.Count) continue;
                list[index++] = item;
            }
        }

        public static IEnumerable<T> ReadCollection<T>(this BinaryReader reader) where T : IBinarySave, new()
        {
            var count = reader.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                var item = reader.ReadBoolean() ? new T() : default;
                item?.Load(reader);
                yield return item;
            }
        }

        public static List<T> ReadList<T>(this BinaryReader reader) where T : IBinarySave, new()
        {
            return new List<T>(ReadCollection<T>(reader));
        }

        public static T[] ReadArray<T>(this BinaryReader reader) where T : IBinarySave, new()
        {
            var array = new T[reader.ReadInt32()];

            for (var i = 0; i < array.Length; i++)
            {
                array[i] = reader.ReadBoolean() ? new T() : default;
                array[i]?.Load(reader);
            }

            return array;
        }
    }
}