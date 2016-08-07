using System.ComponentModel;
using System.Text;

namespace ChecksumVerifier
{
    public enum EncType
    {
        [Description("Default (System)")]
        Default,
        [Description("ASCII")]
        ASCII,
        [Description("Big Endian Unicode")]
        BigEndianUnicode,
        [Description("Unicode")]
        Unicode,
        [Description("UTF7")]
        UTF7,
        [Description("UTF8")]
        UTF8,
        [Description("UTF32")]
        UTF32
    }

    public class EncodingType
    {
        public string Name { get { return Encoding.Description(); }  }
        public EncType Encoding { get; set; }
        public Encoding Type { get; set; }

        public EncodingType(EncType encoding, Encoding type)
        {
            Encoding = encoding;
            Type = type;
        }
    }
}
