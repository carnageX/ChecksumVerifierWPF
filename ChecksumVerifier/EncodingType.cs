using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecksumVerifier
{
    class EncodingType
    {
        public string Name { get; set; }
        public Encoding Type { get; set; }

        public EncodingType(string name, Encoding type)
        {
            Name = name;
            Type = type;
        }
    }
}
