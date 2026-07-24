using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Drafts.SaveData
{
    public static class ReadWriteExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteCollection<T>(this BinaryWriter writer, Action<BinaryWriter, T> write, IReadOnlyCollection<T> list)
        {
            writer.Write(list.Count);

            foreach (var item in list)
            {
                writer.Write(item != null);
                if (item != null) write(writer, item);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> ReadCollection<T>(this BinaryReader reader, Func<BinaryReader, T> read)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
                yield return reader.ReadBoolean() ? read(reader) : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> ReadList<T>(this BinaryReader reader, Func<BinaryReader, T> read)
        {
            var result = new List<T>(reader.ReadInt32());
            for (var i = 0; i < result.Capacity; i++)
                result[i] = reader.ReadBoolean() ? read(reader) : default;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ReadArray<T>(this BinaryReader reader, Func<BinaryReader, T> read)
        {
            var result = new T[reader.ReadInt32()];
            for (var i = 0; i < result.Length; i++)
                result[i] = reader.ReadBoolean() ? read(reader) : default;
            return result;
        }

        private static readonly Action<BinaryWriter, bool> WBool = static (w, s) => w.Write(s);
        private static readonly Action<BinaryWriter, int> WInt = static (w, s) => w.Write(s);
        private static readonly Action<BinaryWriter, float> WFloat = static (w, s) => w.Write(s);
        private static readonly Action<BinaryWriter, long> WLong = static (w, s) => w.Write(s);
        private static readonly Action<BinaryWriter, uint> WUInt = static (w, s) => w.Write(s);
        private static readonly Action<BinaryWriter, string> WString = static (w, s) => w.Write(s);

        private static readonly Func<BinaryReader, bool> RBool = static r => r.ReadBoolean();
        private static readonly Func<BinaryReader, int> RInt = static r => r.ReadInt32();
        private static readonly Func<BinaryReader, float> RFloat = static r => r.ReadSingle();
        private static readonly Func<BinaryReader, long> RLong = static r => r.ReadInt64();
        private static readonly Func<BinaryReader, uint> RUInt = static r => r.ReadUInt32();
        private static readonly Func<BinaryReader, string> RString = static r => r.ReadString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this BinaryWriter w, IReadOnlyCollection<bool> c) => w.WriteCollection(WBool, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this BinaryWriter w, IReadOnlyCollection<int> c) => w.WriteCollection(WInt, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this BinaryWriter w, IReadOnlyCollection<float> c) => w.WriteCollection(WFloat, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this BinaryWriter w, IReadOnlyCollection<long> c) => w.WriteCollection(WLong, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this BinaryWriter w, IReadOnlyCollection<uint> c) => w.WriteCollection(WUInt, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this BinaryWriter w, IReadOnlyCollection<string> c) => w.WriteCollection(WString, c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool[] ReadBoolArray(this BinaryReader reader) => reader.ReadArray(RBool);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] ReadIntArray(this BinaryReader reader) => reader.ReadArray(RInt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] ReadFloatArray(this BinaryReader reader) => reader.ReadArray(RFloat);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long[] ReadLongArray(this BinaryReader reader) => reader.ReadArray(RLong);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint[] ReadUIntArray(this BinaryReader reader) => reader.ReadArray(RUInt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] ReadStringArray(this BinaryReader reader) => reader.ReadArray(RString);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<bool> ReadBoolList(this BinaryReader reader) => reader.ReadList(RBool);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<int> ReadIntList(this BinaryReader reader) => reader.ReadList(RInt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<float> ReadFloatList(this BinaryReader reader) => reader.ReadList(RFloat);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<long> ReadLongList(this BinaryReader reader) => reader.ReadList(RLong);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<uint> ReadUIntList(this BinaryReader reader) => reader.ReadList(RUInt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<string> ReadStringList(this BinaryReader reader) => reader.ReadList(RString);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<bool> ReadBoolCollection(this BinaryReader reader) => reader.ReadCollection(RBool);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> ReadIntCollection(this BinaryReader reader) => reader.ReadCollection(RInt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<float> ReadFloatCollection(this BinaryReader reader) => reader.ReadCollection(RFloat);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<long> ReadLongCollection(this BinaryReader reader) => reader.ReadCollection(RLong);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<uint> ReadUIntCollection(this BinaryReader reader) => reader.ReadCollection(RUInt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> ReadStringCollection(this BinaryReader reader) => reader.ReadCollection(RString);
    }
}