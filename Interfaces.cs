using System;

namespace Drafts.SaveData
{
    public class SaveNotLoadedException : Exception { }

    public interface ISaveDataParser
    {
        object Load(string path, Type type);
        public void Save(string path, in object data);
    }

    public interface ISaveData
    {
        void LoadParse() { }
        void SaveParse() { }
    }
}