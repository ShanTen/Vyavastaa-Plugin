using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VyavastaPlugin
{
    public class VyavastaConfig
    {
        public Dictionary<string, string> schoolLookup = new Dictionary<string, string>();
        public ulong UpperHouseRole, LowerHouseRole;
        const string filePath = "./VyavastaConfig.json";

        public static VyavastaConfig load()
        {
            VyavastaConfig vc = new VyavastaConfig();

            JsonConvert.PopulateObject(File.ReadAllText(filePath),vc);

            return vc;
        }

        public void Save()
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }

}
