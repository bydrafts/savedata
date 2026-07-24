using System;
using System.IO;
using Drafts.SaveData;

namespace Drafts.SaveData
{
    public interface IBinarySave
    {
        void Save(BinaryWriter writer);
        void Load(BinaryReader reader);
    }

    public class BinaryFileParser : ISaveDataParser
    {
        public void Save(string path, in object data)
        {
            if (data is not IBinarySave sd) throw new Exception($"{data.GetType().Name} is not ISaveData");
            
            path = Path.Combine(path, data.GetType().Name + ".sav");
            var dir = Path.GetDirectoryName(path) ?? throw new Exception();
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            
            using var file = File.Create(path);
            using var writer = new BinaryWriter(file);
            sd.Save(writer);
        }

        public object Load(string path, Type type)
        {
            path = Path.Combine(path, type.Name + ".sav");
            var obj = (IBinarySave)Activator.CreateInstance(type);
            if (!File.Exists(path)) return obj;
            using var file = File.OpenRead(path);
            using var reader = new BinaryReader(file);
            obj.Load(reader);
            return obj;
        }
    }
}