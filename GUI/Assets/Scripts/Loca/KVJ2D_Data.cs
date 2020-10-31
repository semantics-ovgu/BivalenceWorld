using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomFramework.KVJ2D
{
    [System.Serializable]
    public class KVJ2D_Data
    {
        public string Key;
        public string Value;

        public KVJ2D_Data(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public override string ToString()
        {
            return Key + ":" + Value;
        }
    }
}
