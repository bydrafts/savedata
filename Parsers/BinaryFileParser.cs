using System;
using System.IO;

namespace Drafts.SaveData
{
    public interface IBinarySave
    {
        void Save(BinaryWriter writer);
        void Load(BinaryReader reader);
    }

    public class BinaryFileParser : FileParserBase
    {
        protected override string Extension => ".binsav";

        public override void Save(string key, in object data)
        {
            if (data is not IBinarySave sd) throw new Exception($"{data?.GetType().Name} is not ISaveData");
            AssurePath(key);
            using var file = File.Create(key);
            using var writer = new BinaryWriter(file);
            sd.Save(writer);
        }

        public override object Load(string key, Type type)
        {
            var obj = Activator.CreateInstance(type);
            if (obj is not IBinarySave sd) throw new Exception($"{type.Name} is not ISaveData");
            if (!File.Exists(key)) return obj;

            using var file = File.OpenRead(key);
            using var reader = new BinaryReader(file);
            sd.Load(reader);
            return obj;
        }
    }
}