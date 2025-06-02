using System;
using System.IO;
using Newtonsoft.Json;

namespace Crypter
{
    internal class Settings
    {
        private static string savepath = AppDomain.CurrentDomain.BaseDirectory + "\\bin\\settings.json";

        internal static SettingsObject Load()
        {
            if (!File.Exists(savepath))
            {
                return JsonConvert.DeserializeObject<SettingsObject>(File.ReadAllText(savepath));
            }
            return null;
        }

        internal static void Save(SettingsObject obj) => File.WriteAllText(savepath, JsonConvert.SerializeObject(obj, Formatting.Indented));
    }

    internal class SettingsObject
    {
        public string inputfile { get; set; }
        public bool antiVM { get; set; }
        public bool antiDebug { get; set; }
        public bool amsiBypass { get; set; }
        public bool etwBypass { get; set; }
        public bool obfuscation { get; set; }
        public bool runas { get; set; }
        public bool usePolymorphicAes { get; set; }
        public bool useArmdot { get; set; }
        public bool processMasquerading { get; set; }
        public bool useEvilbyteIndirectSyscalls { get; set; }
        public bool winREPersistence { get; set; }
    }
}