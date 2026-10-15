using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Drafts.SaveData
{
    public static class BinaryFileParserExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSave<T>(this BinaryWriter writer, T data) where T : IBinarySave
        {
            writer.Write(data != null);
            data?.Save(writer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadSave<T>(this BinaryReader reader) where T : IBinarySave, new()
        {
            if (!reader.ReadBoolean()) 
                return default;
            var data = new T();
            data.Load(reader);
            return data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSaveCollection<T>(this BinaryWriter writer, IReadOnlyCollection<T> data)
            where T : IBinarySave => writer.WriteCollection(WriteSave, data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ReadSaveArray<T>(this BinaryReader reader)
            where T : IBinarySave, new() => reader.ReadArray(ReadSave<T>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> ReadSaveList<T>(this BinaryReader reader)
            where T : IBinarySave, new() => reader.ReadList(ReadSave<T>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> ReadSaveCollection<T>(this BinaryReader reader)
            where T : IBinarySave, new() => reader.ReadCollection(ReadSave<T>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadSaveFixed<T>(this BinaryReader reader, IList<T> list)
            where T : IBinarySave, new() => reader.ReadFixed(ReadSave<T>, list);
    }
}