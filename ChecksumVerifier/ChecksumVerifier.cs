using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ChecksumVerifier
{
    class ChecksumVerifierLogic
    {
        #region GetHash Methods
        /// <summary>Generates checksum hash of a given file using a given algorithm.</summary>
        /// <param name="filename">File path and name to generate checksum.</param>
        /// <param name="hashOption">Checksum Algorithm.  Options are: MD5, SHA-1, SHA-256, SHA-384, and SHA-512</param>
        /// <returns>Hash string to be returned.</returns>
        public static string GetHash(string filename, HashOption hashOption)
        {
            try
            {
                return ComputeHashAlgorithm(filename, hashOption);
            }//try
            catch
            {
                throw;
            }//catch
        }//GetHash

        public static string GetHash(string clearText, HashOption hashOption, Encoding encodingType)
        {
            try
            {
                return ComputeHashAlgorithm(clearText, hashOption, encodingType);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Generates checksum hash of a given file using a given algorithm.</summary>
        /// <param name="filename">File path and name to generate checksum.</param>
        /// <param name="hashOption">Checksum Algorithm.  Options are: MD5, SHA-1, SHA-256, SHA-384, and SHA-512</param>
        /// <returns>Hash string to be returned.</returns>
        public static string GetHash_Threaded(string filename, HashOption hashOption, int threadCount)
        {
            try
            {
                return ComputeHashAlgorithm(filename, hashOption);
            }//try
            catch
            {
                throw;
            }//catch
        }//GetHash

        /// <summary>Generates checksum hash of a given file using a list of given algorithms.</summary>
        /// <param name="filename">File path and name to generate checksum.</param>
        /// <param name="hashOption">List of Checksum Algorithms.  Options are: MD5, SHA-1, SHA-256, SHA-384, and SHA-512</param>
        /// <returns>List of hash strings to be returned.</returns>
        public static List<string> GetHash(string filename, List<HashOption> hashOption)
        {
            List<string> returnHashes = new List<string>();
            try
            {
                foreach (var h in hashOption)
                {
                    returnHashes.Add(ComputeHashAlgorithm(filename, h));
                }//foreach
            }//try
            catch
            {
                throw;
            }//catch
            return returnHashes;
        }//GetHash

        /// <summary>Generates checksum hash of a given file using a list of given algorithms.</summary>
        /// <param name="filename">File path and name to generate checksum.</param>
        /// <param name="hashOption">List of Checksum Algorithms.  Options are: MD5, SHA-1, SHA-256, SHA-384, and SHA-512</param>
        /// <returns>List of hash strings to be returned.</returns>
        public static List<string> GetHash_Threaded(string filename, List<HashOption> hashOption, int threadCount)
        {
            List<string> returnHashes = new List<string>();
            try
            {
                foreach (var h in hashOption)
                {
                    returnHashes.Add(ComputeHashAlgorithm(filename, h));
                }//foreach
            }//try
            catch
            {
                throw;
            }//catch
            return returnHashes;
        }//GetHash
        #endregion

        #region Compute Methods
        #region Compute From File
        /// <summary>Gets the computed checksum for the provided file using the provided algorithm</summary>
        /// <param name="filename">File to read</param>
        /// <param name="hashOption">Hash algorithm to use when generating checksum
        ///     Options are: "MD5", "MD5-CNG", "SHA-1", "SHA-1-Managed", "SHA-1-CNG", "SHA-256", "SHA-256-Managed", "SHA-256-CNG", 
        ///     "SHA-384", "SHA-384-Managed", "SHA-384-CNG", "SHA-512", "SHA-512-Managed", "SHA-512-CNG", 
        ///     "RIPEMD160", "RIPEMD160-Managed", "CRC16", or "CRC32"</param>
        /// <returns>Computed hash</returns>
        /// <remarks>Non-managed / non-CNG implementations of algorithms use CryptoServiceProvider implementation by default (unmanaged, uses CryptoAPI, FIPS certified). 
        /// Managed implementations use the Managed code libraries, and are non-FIPS certified (may also be slower).
        /// CNG (Cryptography Next Generation) are newer implementations by Microsoft with Win2k8/Vista and newer, which is also FIPS certified.
        /// Source: http://codeissue.com/issues/i34dda6deaad90a/difference-between-sha1-sha1cryptoserviceprovider-sha1managed-and-sha1cng </remarks>
        private static string ComputeHashAlgorithm(string filename, HashOption hashOption)
        {
            string computedHash = String.Empty;

            switch(hashOption)
            {
                case HashOption.MD5:
                case HashOption.MD5CNG:
                    computedHash = ComputeMD5Algorithm(filename, hashOption);
                    break;
                case HashOption.SHA1:
                case HashOption.SHA1CNG:
                case HashOption.SHA1Managed:
                    computedHash = ComputeSHA1Algorithm(filename, hashOption);
                    break;
                case HashOption.SHA256:
                case HashOption.SHA256CNG:
                case HashOption.SHA256Managed:
                    computedHash = ComputeSHA256Algorithm(filename, hashOption);
                    break;
                case HashOption.SHA384:
                case HashOption.SHA384CNG:
                case HashOption.SHA384Managed:
                    computedHash = ComputeSHA384Algorithm(filename, hashOption);
                    break;
                case HashOption.SHA512:
                case HashOption.SHA512CNG:
                case HashOption.SHA512Managed:
                    computedHash = ComputeSHA512Algorithm(filename, hashOption);
                    break;
                case HashOption.RIPEMD160:
                case HashOption.RIPEMD160Managed:
                    computedHash = ComputeRIPEMD160Algorithm(filename, hashOption);
                    break;
                case HashOption.CRC16:
                    computedHash = ComputeCrc16Algorithm(filename);
                    break;
                case HashOption.CRC32:
                    computedHash = ComputeCrc32Algorithm(filename);
                    break;
                default:
                    computedHash = "Not a valid algorithm option!";
                    break;
            }

            return computedHash;
        }//ComputeHashAlgorithm

        /// <summary>Computes the selected MD5 hash for the given file. 
        /// </summary>
        /// <param name="filename">File to read</param>
        /// <param name="hashOption">MD5 algorithm to generate checksum. 
        /// Options are: "MD5" (using CryptoServiceProvider) or "MD5-CNG"</param>
        /// <returns>Computed hash</returns>
        private static string ComputeMD5Algorithm(string filename, HashOption hashOption)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.MD5)
            {
                MD5 md5Checksum = MD5.Create();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(md5Checksum.ComputeHash(fs));
                }//using
            }//if
            else if (hashOption == HashOption.MD5CNG)
            {
                MD5Cng md5CNGChecksum = new MD5Cng();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(md5CNGChecksum.ComputeHash(fs));
                }//using
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }

            return computedHash;
        }

        /// <summary>Computes the selected SHA-1 hash for the given file. 
        /// </summary>
        /// <param name="filename">File to read</param>
        /// <param name="hashOption">SHA-1 algorithm to generate checksum. 
        /// Options are: "SHA-1" (using CryptoServiceProvider), "SHA-1-Managed", or "SHA-1-CNG"</param>
        /// <returns>Computed hash</returns>
        private static string ComputeSHA1Algorithm(string filename, HashOption hashOption)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.SHA1)
            {
                SHA1 sha1Checksum = SHA1.Create();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha1Checksum.ComputeHash(fs));
                }//using 
            }//else if
            else if (hashOption == HashOption.SHA1Managed)
            {
                SHA1Managed sha1MngChecksum = new SHA1Managed();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha1MngChecksum.ComputeHash(fs));
                }//using
            }
            else if (hashOption == HashOption.SHA1CNG)
            {
                SHA1Cng sha1CNGChecksum = new SHA1Cng();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha1CNGChecksum.ComputeHash(fs));
                }//using
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }//else

            return computedHash;
        }

        /// <summary>Computes the selected SHA-256 hash for the given file. 
        /// </summary>
        /// <param name="filename">File to read</param>
        /// <param name="hashOption">SHA-256 algorithm to generate checksum. 
        /// Options are: "SHA-256" (using CryptoServiceProvider), "SHA-256-Managed", or "SHA-256-CNG"</param>
        /// <returns>Computed hash</returns>
        private static string ComputeSHA256Algorithm(string filename, HashOption hashOption)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.SHA256)
            {
                SHA256 sha256Checksum = SHA256.Create();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha256Checksum.ComputeHash(fs));
                }//using 
            }//else if
            else if (hashOption == HashOption.SHA256Managed)
            {
                SHA256Managed sha256MngChecksum = new SHA256Managed();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha256MngChecksum.ComputeHash(fs));
                }//using
            }
            else if (hashOption == HashOption.SHA256CNG)
            {
                SHA256Cng sha256CNGChecksum = new SHA256Cng();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha256CNGChecksum.ComputeHash(fs));
                }//using
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }//else

            return computedHash;
        }

        /// <summary>Computes the selected SHA-384 hash for the given file. 
        /// </summary>
        /// <param name="filename">File to read</param>
        /// <param name="hashOption">SHA-384 algorithm to generate checksum. 
        /// Options are: "SHA-384" (using CryptoServiceProvider), "SHA-384-Managed", or "SHA-384-CNG"</param>
        /// <returns>Computed hash</returns>
        private static string ComputeSHA384Algorithm(string filename, HashOption hashOption)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.SHA384)
            {
                SHA384 sha384Checksum = SHA384.Create();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha384Checksum.ComputeHash(fs));
                }//using 
            }//else if
            else if (hashOption == HashOption.SHA384Managed)
            {
                SHA384Managed sha384MngChecksum = new SHA384Managed();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha384MngChecksum.ComputeHash(fs));
                }//using
            }
            else if (hashOption == HashOption.SHA384CNG)
            {
                SHA384Cng sha384CNGChecksum = new SHA384Cng();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha384CNGChecksum.ComputeHash(fs));
                }//using
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }//else

            return computedHash;
        }

        /// <summary>Computes the selected SHA-512 hash for the given file. 
        /// </summary>
        /// <param name="filename">File to read</param>
        /// <param name="hashOption">SHA-512 algorithm to generate checksum. 
        /// Options are: "SHA-512" (using CryptoServiceProvider), "SHA-512-Managed", or "SHA-512-CNG"</param>
        /// <returns>Computed hash</returns>
        private static string ComputeSHA512Algorithm(string filename, HashOption hashOption)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.SHA512)
            {
                SHA512 sha512Checksum = SHA512.Create();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha512Checksum.ComputeHash(fs));
                }//using 
            }//else if
            else if (hashOption == HashOption.SHA512Managed)
            {
                SHA512Managed sha512MngChecksum = new SHA512Managed();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha512MngChecksum.ComputeHash(fs));
                }//using
            }
            else if (hashOption == HashOption.SHA512CNG)
            {
                SHA512Cng sha512CNGChecksum = new SHA512Cng();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(sha512CNGChecksum.ComputeHash(fs));
                }//using
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }//else

            return computedHash;
        }

        /// <summary>Computes the selected RIPEMD160 hash for the given file. 
        /// </summary>
        /// <param name="filename">File to read</param>
        /// <param name="hashOption">RIPEMD160 algorithm to generate checksum. 
        /// Options are: "RIPEMD160" (using CryptoServiceProvider) or "RIPEMD160-Managed"</param>
        /// <returns>Computed hash</returns>
        private static string ComputeRIPEMD160Algorithm(string filename, HashOption hashOption)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.RIPEMD160)
            {
                RIPEMD160 ripemd160Checksum = RIPEMD160.Create();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(ripemd160Checksum.ComputeHash(fs));
                }//using
            }//else if
            else if (hashOption == HashOption.RIPEMD160Managed)
            {
                RIPEMD160Managed ripemd160MgChecksum = new RIPEMD160Managed();
                using (FileStream fs = File.Open(filename, FileMode.Open))
                {
                    computedHash = FormatHashChecksum(ripemd160MgChecksum.ComputeHash(fs));
                }//using
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }//else

            return computedHash;
        }

        /// <summary>Computes the selected CRC16 hash for the given file.
        /// </summary>
        /// <param name="filename">File to read</param>
        /// <returns>Computed hash</returns>
        private static string ComputeCrc16Algorithm(string filename)
        {
            string computedHash = String.Empty;
            CRC16 crc16Checksum = new CRC16();
            using (FileStream fs = File.Open(filename, FileMode.Open))
            {
                foreach (byte b in crc16Checksum.ComputeHash(fs))
                {
                    computedHash = computedHash.Insert(0, b.ToString("x2").ToLower());
                }//foreach
            }//using
             //throws arithmetic overflow when choosing crc16
            return computedHash;
        }

        /// <summary>Computes the selected CRC32 hash for the given file.
        /// </summary>
        /// <param name="filename">File to read</param>
        /// <returns>Computed hash</returns>
        private static string ComputeCrc32Algorithm(string filename)
        {
            string computedHash = String.Empty;
            CRC32 crc32Checksum = new CRC32();
            using (FileStream fs = File.Open(filename, FileMode.Open))
            {
                foreach (byte b in crc32Checksum.ComputeHash(fs))
                {
                    computedHash = computedHash.Insert(0, b.ToString("x2").ToLower());
                }//foreach
            }//using
            return computedHash;
        }
        #endregion

        #region Compute From String
        /// <summary>Gets the computed checksum for the provided file using the provided algorithm</summary>
        /// <param name="clearText">Input string to generate a checksum.</param>
        /// <param name="hashOption">Hash algorithm to use when generating checksum
        ///     Options are: "MD5", "MD5-CNG", "SHA-1", "SHA-1-Managed", "SHA-1-CNG", "SHA-256", "SHA-256-Managed", "SHA-256-CNG", 
        ///     "SHA-384", "SHA-384-Managed", "SHA-384-CNG", "SHA-512", "SHA-512-Managed", "SHA-512-CNG", 
        ///     "RIPEMD160", "RIPEMD160-Managed", "CRC16", or "CRC32"</param>
        /// <param name="encodingType">Text Encoding type to encode the input string as.
        ///     Options are: ASCII, BigEndianUnicode, Default (current system's default) 
        ///        Unicode, UTF7, UTF8, UTF32</param>
        /// <returns>Computed hash</returns>
        /// <remarks>Non-managed / non-CNG implementations of algorithms use CryptoServiceProvider implementation by default (unmanaged, uses CryptoAPI, FIPS certified). 
        /// Managed implementations use the Managed code libraries, and are non-FIPS certified (may also be slower).
        /// CNG (Cryptography Next Generation) are newer implementations by Microsoft with Win2k8/Vista and newer, which is also FIPS certified.
        /// Source: http://codeissue.com/issues/i34dda6deaad90a/difference-between-sha1-sha1cryptoserviceprovider-sha1managed-and-sha1cng </remarks>
        private static string ComputeHashAlgorithm(string clearText, HashOption hashOption, Encoding encodingType)
        {
            string computedHash = String.Empty;
            byte[] stringBytes = encodingType.GetBytes(clearText);

            switch (hashOption)
            {
                case HashOption.MD5:
                case HashOption.MD5CNG:
                    computedHash = ComputeMD5Algorithm(hashOption, stringBytes);
                    break;
                case HashOption.SHA1:
                case HashOption.SHA1CNG:
                case HashOption.SHA1Managed:
                    computedHash = ComputeSHA1Algorithm(hashOption, stringBytes);
                    break;
                case HashOption.SHA256:
                case HashOption.SHA256CNG:
                case HashOption.SHA256Managed:
                    computedHash = ComputeSHA256Algorithm(hashOption, stringBytes);
                    break;
                case HashOption.SHA384:
                case HashOption.SHA384CNG:
                case HashOption.SHA384Managed:
                    computedHash = ComputeSHA384Algorithm(hashOption, stringBytes);
                    break;
                case HashOption.SHA512:
                case HashOption.SHA512CNG:
                case HashOption.SHA512Managed:
                    computedHash = ComputeSHA512Algorithm(hashOption, stringBytes);
                    break;
                case HashOption.RIPEMD160:
                case HashOption.RIPEMD160Managed:
                    computedHash = ComputeRIPEMD160Algorithm(hashOption, stringBytes);
                    break;
                case HashOption.CRC16:
                    computedHash = ComputeCrc16Algorithm(stringBytes);
                    break;
                case HashOption.CRC32:
                    computedHash = ComputeCrc32Algorithm(stringBytes);
                    break;
                default:
                    computedHash = "Not a valid algorithm option!";
                    break;
            }

            return computedHash;
        }//ComputeHashAlgorithm

        /// <summary>Computes the selected MD5 hash for the given byte array. 
        /// </summary>
        /// <param name="hashOption">MD5 algorithm to generate checksum. 
        /// Options are: "MD5" (using CryptoServiceProvider) or "MD5-CNG"</param>
        /// <param name="bytes">Byte representation of item to encode</param>
        /// <returns>Computed hash</returns>
        private static string ComputeMD5Algorithm(HashOption hashOption, byte[] bytes)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.MD5)
            {
                MD5 md5Checksum = MD5.Create();
                computedHash = FormatHashChecksum(md5Checksum.ComputeHash(bytes));
            }//if
            else if (hashOption == HashOption.MD5CNG)
            {
                MD5Cng md5CNGChecksum = new MD5Cng();
                computedHash = FormatHashChecksum(md5CNGChecksum.ComputeHash(bytes));
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }

            return computedHash;
        }

        /// <summary>Computes the selected SHA-1 hash for the given byte array. 
        /// </summary>
        /// <param name="hashOption">SHA-1 algorithm to generate checksum. 
        /// Options are: "SHA-1" (using CryptoServiceProvider), "SHA-1-Managed", or "SHA-1-CNG"</param>
        /// <param name="bytes">Byte representation of item to encode</param>
        /// <returns>Computed hash</returns>
        private static string ComputeSHA1Algorithm(HashOption hashOption, byte[] bytes)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.SHA1)
            {
                SHA1 sha1Checksum = SHA1.Create();
                computedHash = FormatHashChecksum(sha1Checksum.ComputeHash(bytes));
            }//else if
            else if (hashOption == HashOption.SHA1Managed)
            {
                SHA1Managed sha1MngChecksum = new SHA1Managed();
                computedHash = FormatHashChecksum(sha1MngChecksum.ComputeHash(bytes));
            }
            else if (hashOption == HashOption.SHA1CNG)
            {
                SHA1Cng sha1CNGChecksum = new SHA1Cng();
                computedHash = FormatHashChecksum(sha1CNGChecksum.ComputeHash(bytes));
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }//else

            return computedHash;
        }

        /// <summary>Computes the selected SHA-256 hash for the given byte array. 
        /// </summary>
        /// <param name="hashOption">SHA-256 algorithm to generate checksum. 
        /// Options are: "SHA-256" (using CryptoServiceProvider), "SHA-256-Managed", or "SHA-256-CNG"</param>
        /// <param name="bytes">Byte representation of item to encode</param>
        /// <returns>Computed hash</returns>
        private static string ComputeSHA256Algorithm(HashOption hashOption, byte[] bytes)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.SHA256)
            {
                SHA256 sha256Checksum = SHA256.Create();
                computedHash = FormatHashChecksum(sha256Checksum.ComputeHash(bytes));
            }//else if
            else if (hashOption == HashOption.SHA256Managed)
            {
                SHA256Managed sha256MngChecksum = new SHA256Managed();
                computedHash = FormatHashChecksum(sha256MngChecksum.ComputeHash(bytes));
            }
            else if (hashOption == HashOption.SHA256CNG)
            {
                SHA256Cng sha256CNGChecksum = new SHA256Cng();
                computedHash = FormatHashChecksum(sha256CNGChecksum.ComputeHash(bytes));
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }//else

            return computedHash;
        }

        /// <summary>Computes the selected SHA-384 hash for the given byte array. 
        /// </summary>
        /// <param name="hashOption">SHA-384 algorithm to generate checksum. 
        /// Options are: "SHA-384" (using CryptoServiceProvider), "SHA-384-Managed", or "SHA-384-CNG"</param>
        /// <param name="bytes">Byte representation of item to encode</param>
        /// <returns>Computed hash</returns>
        private static string ComputeSHA384Algorithm(HashOption hashOption, byte[] bytes)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.SHA384)
            {
                SHA384 sha384Checksum = SHA384.Create();
                computedHash = FormatHashChecksum(sha384Checksum.ComputeHash(bytes)); 
            }//else if
            else if (hashOption == HashOption.SHA384Managed)
            {
                SHA384Managed sha384MngChecksum = new SHA384Managed();
                computedHash = FormatHashChecksum(sha384MngChecksum.ComputeHash(bytes));
            }
            else if (hashOption == HashOption.SHA384CNG)
            {
                SHA384Cng sha384CNGChecksum = new SHA384Cng();
                computedHash = FormatHashChecksum(sha384CNGChecksum.ComputeHash(bytes));
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }//else

            return computedHash;
        }

        /// <summary>Computes the selected SHA-512 hash for the given byte array. 
        /// </summary>
        /// <param name="hashOption">SHA-512 algorithm to generate checksum. 
        /// Options are: "SHA-512" (using CryptoServiceProvider), "SHA-512-Managed", or "SHA-512-CNG"</param>
        /// <param name="bytes">Byte representation of item to encode</param>
        /// <returns>Computed hash</returns>
        private static string ComputeSHA512Algorithm(HashOption hashOption, byte[] bytes)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.SHA512)
            {
                SHA512 sha512Checksum = SHA512.Create();
                computedHash = FormatHashChecksum(sha512Checksum.ComputeHash(bytes)); 
            }//else if
            else if (hashOption == HashOption.SHA512Managed)
            {
                SHA512Managed sha512MngChecksum = new SHA512Managed();
                computedHash = FormatHashChecksum(sha512MngChecksum.ComputeHash(bytes));
            }
            else if (hashOption == HashOption.SHA512CNG)
            {
                SHA512Cng sha512CNGChecksum = new SHA512Cng();
                computedHash = FormatHashChecksum(sha512CNGChecksum.ComputeHash(bytes));
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }//else

            return computedHash;
        }

        /// <summary>Computes the selected RIPEMD160 hash for the given byte array. 
        /// </summary>
        /// <param name="hashOption">RIPEMD160 algorithm to generate checksum. 
        /// Options are: "RIPEMD160" (using CryptoServiceProvider) or "RIPEMD160-Managed"</param>
        /// <param name="bytes">Byte representation of item to encode</param>
        /// <returns>Computed hash</returns>
        private static string ComputeRIPEMD160Algorithm(HashOption hashOption, byte[] bytes)
        {
            string computedHash = String.Empty;

            if (hashOption == HashOption.RIPEMD160)
            {
                RIPEMD160 ripemd160Checksum = RIPEMD160.Create();
                computedHash = FormatHashChecksum(ripemd160Checksum.ComputeHash(bytes));
            }//else if
            else if (hashOption == HashOption.RIPEMD160Managed)
            {
                RIPEMD160Managed ripemd160MgChecksum = new RIPEMD160Managed();
                computedHash = FormatHashChecksum(ripemd160MgChecksum.ComputeHash(bytes));
            }
            else
            {
                computedHash = "Not a valid algorithm option!";
            }//else

            return computedHash;
        }

        /// <summary> Computes the selected CRC16 hash for the given byte array. 
        /// </summary>
        /// <param name="bytes">Byte representation of item to encode</param>
        /// <returns>Computed hash</returns>
        private static string ComputeCrc16Algorithm(byte[] bytes)
        {
            var computedHash = String.Empty;
            CRC16 crc16Checksum = new CRC16();

            foreach (byte b in crc16Checksum.ComputeHash(bytes))
            {
                computedHash = computedHash.Insert(0, b.ToString("x2").ToLower());
            }//foreach
            //throws arithmetic overflow when choosing crc16
            return computedHash;
        }

        /// <summary> Computes the selected CRC32 hash for the given byte array. 
        /// </summary>
        /// <param name="bytes">Byte representation of item to encode</param>
        /// <returns>Computed hash</returns>
        private static string ComputeCrc32Algorithm(byte[] bytes)
        {
            var computedHash = String.Empty;
            CRC32 crc32Checksum = new CRC32();

            foreach (byte b in crc32Checksum.ComputeHash(bytes))
            {
                computedHash = computedHash.Insert(0, b.ToString("x2").ToLower());
            }//foreach
            return computedHash;
        }
        #endregion
        #endregion

        #region Formatting Methods
        /// <summary>Formats the inputted byte array by removing hyphens</summary>
        /// <param name="hashArray">Byte array computed from checksum</param>
        /// <returns>Formatted checksum string</returns>
        private static string FormatHashChecksum(byte[] hashArray)
        {
            return BitConverter.ToString(hashArray).Replace("-", "").ToLower();
        }//FormatHashChecksum
        #endregion

        #region Compare Methods
        /// <summary>Compares two checksums and returns true if they are the same, otherwise false.</summary>
        /// <param name="hash1">First checksum hash to compare</param>
        /// <param name="hash2">Second checksum hash to compare</param>
        /// <returns>True/False if hash strings are the same</returns>
        public static bool CompareHashes(string hash1, string hash2)
        {
            if (String.Equals(hash1.ToLower().Trim(), hash2.ToLower().Trim()))
                return true;
            else
                return false;
        }//CompareHashes

        /// <summary>Compares a list of checksum strings to a single inputted checksum.</summary>
        /// <param name="hash">Single checksum to compare against.</param>
        /// <param name="hashFileList">List of checksum strings to compare against the single checksum.</param>
        /// <returns></returns>
        public static bool CompareHashes(string hash, List<string> hashFileList)
        {
            return hashFileList.Contains(hash.Trim());
        }//CompareHashes
        #endregion
    }//ChecksumVerifier
}//namespace
