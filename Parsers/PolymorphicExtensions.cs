using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Drafts.SaveData;

namespace Noites.Systems
{
    public static class PolymorphicSaveLoad
    {
        public static void WritePolymorphic<T>(this BinaryWriter writer, T data) where T : IBinarySave
        {
            writer.Write(data?.GetType().AssemblyQualifiedName ?? "");
            data?.Save(writer);
        }

        public static T ReadPolymorphic<T>(this BinaryReader reader) where T : IBinarySave
        {
            var typeName = reader.ReadString();
            if (string.IsNullOrEmpty(typeName)) return default;
            var type = Type.GetType(typeName);
            var data = (T)Activator.CreateInstance(type!);
            data.Load(reader);
            return data;
        }

        public static void WritePolymorphicCollection<T>(this BinaryWriter writer, IReadOnlyCollection<T> data)
            where T : IBinarySave => writer.WriteCollection(WritePolymorphic, data);

        public static T[] ReadPolymorphicArray<T>(this BinaryReader reader)
            where T : IBinarySave => reader.ReadArray(ReadPolymorphic<T>);

        public static List<T> ReadPolymorphicList<T>(this BinaryReader reader)
            where T : IBinarySave => reader.ReadList(ReadPolymorphic<T>);

        public static IEnumerable<T> ReadPolymorphicCollection<T>(this BinaryReader reader)
            where T : IBinarySave => reader.ReadCollection(ReadPolymorphic<T>);

        public static void ReadPolymorphicFixed<T>(this BinaryReader reader, IList<T> @fixed)
            where T : IBinarySave => reader.ReadFixed(ReadPolymorphic<T>, @fixed);
    }
}