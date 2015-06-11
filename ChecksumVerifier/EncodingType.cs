using System.Text;

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
