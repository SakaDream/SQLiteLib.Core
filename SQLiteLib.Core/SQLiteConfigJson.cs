using Newtonsoft.Json;
using SQLiteLib.Core.Security;
using System.IO;
using System.Text;

namespace SQLiteLib.Core
{
    public static class SQLiteConfigJson
    {
        public static string path { get; } = "config.json";
        private static string tempFile = Path.GetTempFileName();

        public static void save(SQLiteConfig config, bool encrypt)
        {
            if (!File.Exists(path))
            {
                using (var stream = File.Create(path));
            }
            else
            {
                File.WriteAllText(path, string.Empty);
            }
            TextWriter tw = new StreamWriter(File.Open(path, FileMode.Open));
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            if (encrypt)
            {
                tw.Write(Encryption.Encrypt(json));
            }
            else
            {
                tw.Write(json);
            }
            tw.Dispose();
        }
        public static SQLiteConfig open()
        {
            try
            {
                return JsonConvert.DeserializeObject<SQLiteConfig>(File.ReadAllText(path, Encoding.UTF8));
            }
            catch
            {
                return JsonConvert.DeserializeObject<SQLiteConfig>(Encryption.Decrypt(File.ReadAllText(path, Encoding.UTF8)));
            }
        }
        public static void saveTemp(SQLiteConfig config, bool encrypt)
        {
            if (!File.Exists(tempFile))
            {
                FileInfo fileInfo = new FileInfo(tempFile);
                fileInfo.Attributes = FileAttributes.Temporary;
            }
            else
            {
                File.WriteAllText(tempFile, string.Empty);
            }
            TextWriter tw = new StreamWriter(File.Open(path, FileMode.Open));
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            if (encrypt)
            {
                tw.Write(Encryption.Encrypt(json));
            }
            else
            {
                tw.Write(json);
            }
            tw.Dispose();
        }
        public static SQLiteConfig openTemp()
        {
            try
            {
                return JsonConvert.DeserializeObject<SQLiteConfig>(File.ReadAllText(tempFile, Encoding.UTF8));
            }
            catch
            {
                return JsonConvert.DeserializeObject<SQLiteConfig>(Encryption.Decrypt(File.ReadAllText(tempFile, Encoding.UTF8)));
            }
        }
    }
}