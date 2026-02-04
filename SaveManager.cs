using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Drafts.SaveData
{
    public class SaveManager
    {
        private ISaveDataParser Parser { get; }
        private Dictionary<Type, object> pairs = new();

        public string Root { get; private set; }
        public string Folder { get; private set; }
        public string FullPath => GetFullPath(Folder);
        public string GetFullPath(string saveNave) => Path.Combine(Root, saveNave);
        public event Action<SaveManager> OnLoaded;
        public event Action<SaveManager> OnSaving;

        public SaveManager(string root, string saveName, ISaveDataParser parser = null)
            : this(root, parser) => Folder = saveName;

        public SaveManager(string root, ISaveDataParser parser = null)
        {
            Root = root;
            Parser = parser ?? new JsonFileParser();
        }

        public T Get<T>() => (T)Get(typeof(T));

        public object Get(Type type)
        {
            if (Folder == null) throw new SaveNotLoadedException();
            if (!pairs.TryGetValue(type, out var data))
            {
                data = Parser.Load(FullPath, type);
                if (data is ISaveData sd) sd.LoadParse();
                pairs[type] = data;
            }

            return data;
        }

        public void Set<T>(T data) => Set(typeof(T), data);
        public void Set(Type type, object data) => pairs[type] = data;

        public void New(string saveName)
        {
            Folder = saveName;
            pairs.Clear();
        }

        public void Save()
        {
            if (Folder == null) throw new SaveNotLoadedException();
            OnSaving?.Invoke(this);
            foreach (var item in pairs)
            {
                if (item.Value is ISaveData sd) sd.SaveParse();
                Parser.Save(FullPath, item);
            }
        }

        public void Load() => Load(Folder);

        public void Load(string saveName)
        {
            New(saveName);
            OnLoaded?.Invoke(this);
        }

        public void Clear()
        {
            if (Folder == null) throw new SaveNotLoadedException();
            pairs.Clear();
            Delete(Folder);
        }

        public IEnumerable<string> GetNames() => Directory.EnumerateDirectories(Root).Select(Path.GetFileName);

        public IEnumerable<(string saveName, T data)> GetFromAll<T>()
        {
            foreach (var name in GetNames())
            {
                var data = Parser.Load(Path.Combine(Root, name), typeof(T));
                if (data is ISaveData sd) sd.LoadParse();
                yield return (name, (T)data);
            }
        }

        public object GetFrom(Type type, string saveName)
        {
            if (!Exists(saveName)) return null;
            var data = Parser.Load(Path.Combine(Root, saveName), type);
            if (data is ISaveData sd) sd.LoadParse();
            return data;
        }

        public bool Exists(string saveName) => Directory.Exists(Path.Combine(Root, saveName));

        public bool Delete(string saveName)
        {
            try
            {
                foreach (var item in Directory.EnumerateFiles(GetFullPath(saveName))) File.Delete(item);
                foreach (var item in Directory.EnumerateDirectories(GetFullPath(saveName))) Directory.Delete(item, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}