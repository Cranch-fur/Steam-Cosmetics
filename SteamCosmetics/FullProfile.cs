using CranchyLib.Networking;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SteamCosmetics
{
    public static class FullProfile
    {
        public struct S_Profile
        {
            public string content;
            public int version;

            public S_Profile(string content, int version)
            {
                this.content = content;
                this.version = version;
            }
        }



        private const string SAVEFILE_AESKEY = "5BCC2D6A95D4DF04A005504E59A9B36E"; // HEX Format
        private const string SAVEFILE_INNER = "DbdDAQEB";
        private const string SAVEFILE_OUTER = "DbdDAgAC";




        private static void CreateBackup(string data)
        {
            if (Directory.Exists("Data\\Backups") == false)
            {
                Directory.CreateDirectory("Data\\Backups");
            }

            File.WriteAllText($"Data\\Backups\\[{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}] fullProfile.json", data);
        }




        private static byte[] PaddingWithNumber(byte[] buffer, int number)
        {
            byte[] numberBytes = BitConverter.GetBytes(number);
            byte[] result = new byte[numberBytes.Length + buffer.Length];
            Buffer.BlockCopy(numberBytes, 0, result, 0, numberBytes.Length);
            Buffer.BlockCopy(buffer, 0, result, numberBytes.Length, buffer.Length);
            return result;
        }
        private static byte[] ReadToEnd(Stream stream)
        {
            if (stream.CanSeek) stream.Position = 0;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }




        private static string RawDecrypt(string text)
        {
            byte[] inputBytes = Convert.FromBase64String(text);
            using (var rijndael = new RijndaelManaged { Mode = CipherMode.ECB, Padding = PaddingMode.Zeros })
            using (var transform = rijndael.CreateDecryptor(Encoding.ASCII.GetBytes(SAVEFILE_AESKEY), null))
            using (var memoryStream = new MemoryStream(inputBytes))
            using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
            {
                byte[] decryptedBytes = new byte[inputBytes.Length];
                int length = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes, 0, length);
            }
        }
        private static string Decrypt(string input)
        {
            string decryptedInput = RawDecrypt(input.Substring(8).Trim());
            var savefileAscii = new StringBuilder();

            foreach (char c in decryptedInput)
            {
                savefileAscii.Append((char)(c + 1));
            }

            string result = savefileAscii.ToString().Replace("\u0001", "");

            if (result.StartsWith(SAVEFILE_INNER))
            {
                byte[] array = Convert.FromBase64String(result.Substring(8));
                byte[] buffer = new byte[array.Length - 4];
                Array.Copy(array, 4, buffer, 0, buffer.Length);

                using (var memoryStream = new MemoryStream())
                using (var inflaterStream = new InflaterInputStream(new MemoryStream(buffer)))
                {
                    inflaterStream.CopyTo(memoryStream);
                    return Encoding.Unicode.GetString(ReadToEnd(memoryStream));
                }
            }

            return result;
        }




        private static string RawEncrypt(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            using (var rijndael = new RijndaelManaged { Mode = CipherMode.ECB, Padding = PaddingMode.Zeros })
            using (var transform = rijndael.CreateEncryptor(Encoding.ASCII.GetBytes(SAVEFILE_AESKEY), null))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        private static string Encrypt(string input)
        {
            byte[] inputBytes = Encoding.Unicode.GetBytes(input);

            using (var memoryStream = new MemoryStream())
            {
                using (var zOutputStream = new ZOutputStream(memoryStream, -1))
                {
                    zOutputStream.Write(inputBytes, 0, inputBytes.Length);
                    zOutputStream.Flush();
                }

                string saveFile = Convert.ToBase64String(PaddingWithNumber(memoryStream.ToArray(), inputBytes.Length));
                int padding = 16 - ((SAVEFILE_INNER.Length + saveFile.Length) % 16);
                saveFile = SAVEFILE_INNER + saveFile.PadRight(saveFile.Length + padding, '\u0001');

                StringBuilder encryptedSaveFile = new StringBuilder();
                foreach (char c in saveFile)
                {
                    encryptedSaveFile.Append((char)(c - 1));
                }

                return SAVEFILE_OUTER + RawEncrypt(encryptedSaveFile.ToString());
            }
        }




        public static S_Profile Get(string bhvrSession)
        {
            List<string> headers = new List<string>()
            {
                $"Cookie: bhvrSession={bhvrSession}"
            };

            var fullProfileResponse = Networking.Get("https://steam.live.bhvrdbd.com/api/v1/players/me/states/FullProfile/binary", headers);
            if (fullProfileResponse.statusCode != Networking.E_StatusCode.OK)
            {
                return new S_Profile(null, -1);
            }

            if (fullProfileResponse.content == null)
            {
                return new S_Profile(null, -1);
            }

            CreateBackup(fullProfileResponse.content);

            string decryptedFullProfile = Decrypt(fullProfileResponse.content);
            if (decryptedFullProfile.IsJson() == false)
            {
                return new S_Profile(null, -1);
            }

            foreach (string responseHeader in fullProfileResponse.headers)
            {
                if (responseHeader.StartsWith("Kraken-State-Version:"))
                {
                    return new S_Profile(decryptedFullProfile, Convert.ToInt32(responseHeader.Split(' ')[1]));
                }
            }

            return new S_Profile(null, -1);
        }
        public static bool Set(string bhvrSession, string fullProfile, int version)
        {
            string encryptedFullProfile = Encrypt(fullProfile);

            List<string> headers = new List<string>()
            {
                $"Cookie: bhvrSession={bhvrSession}",
                "Content-Type: application/octet-stream",
                "Accept-Encoding: gzip, deflate"
            };

            var fullProfileResponse = Networking.Post($"https://steam.live.bhvrdbd.com/api/v1/players/me/states/binary?schemaVersion=0&stateName=FullProfile&version={version}", headers, encryptedFullProfile);
            if (fullProfileResponse.statusCode != Networking.E_StatusCode.OK)
            {
                return false;
            }

            if (fullProfileResponse.content.IsJson() == false)
            {
                return false;
            }

            return true;
        }
    }
}
