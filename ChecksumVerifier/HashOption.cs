using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecksumVerifier
{
    public enum HashOption
    {
        [Description("MD5")]
        MD5,
        [Description("MD5-CNG")]
        MD5CNG,
        [Description("SHA-1")]
        SHA1,
        [Description("SHA-1-Managed")]
        SHA1Managed,
        [Description("SHA-1-CNG")]
        SHA1CNG,
        [Description("SHA-256")]
        SHA256,
        [Description("SHA-256-Managed")]
        SHA256Managed,
        [Description("SHA-256-CNG")]
        SHA256CNG,
        [Description("SHA-384")]
        SHA384,
        [Description("SHA-384-Managed")]
        SHA384Managed,
        [Description("SHA-384-CNG")]
        SHA384CNG,
        [Description("SHA-512")]
        SHA512,
        [Description("SHA-512-Managed")]
        SHA512Managed,
        [Description("SHA-512-CNG")]
        SHA512CNG,
        [Description("RIPEMD160")]
        RIPEMD160,
        [Description("RIPEMD160-Managed")]
        RIPEMD160Managed,
        [Description("CRC16")]
        CRC16,
        [Description("CRC32")]
        CRC32
    }
}
