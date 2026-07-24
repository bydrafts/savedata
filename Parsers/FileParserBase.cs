using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Drafts.SaveData
{
    public abstract class FileParserBase : ISaveDataParser
    {
        protected abstract string Extension { get; }
        public abstract void Save(string key, in object data);
        public abstract object Load(string key, Type type);

        public virtual bool KeyExists(string key) => File.Exists(key);
        public virtual string GetKey(params string[] path) => Path.Combine(path) + Extension;
        public virtual IEnumerable<string> GetSaveNames(string root) => Directory.EnumerateDirectories(root).Select(Path.GetFileName);

        protected void AssurePath(string path)
        {
            var dir = Path.GetDirectoryName(path) ?? throw new Exception();
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }

        public virtual bool Delete(string[] paths)
        {
            var path = Path.Combine(paths);

            try
            {
                foreach (var item in Directory.EnumerateFiles(path)) File.Delete(item);
                foreach (var item in Directory.EnumerateDirectories(path)) Directory.Delete(item, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}