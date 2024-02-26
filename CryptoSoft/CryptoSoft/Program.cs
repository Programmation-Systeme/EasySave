using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CryptoSoft
{
    internal class CryptoSoft
    {
        private static readonly string _KEY = "20sur20SVP";
        private static readonly string _ENCRYPTEDEXTENSION = ".encrypted";

        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                string EncryptionDelaysInMs = "";
                for (int i = 1; i < args.Length; i++)
                {
                    FileInfo fileInfo = new(args[i]);

                    if (fileInfo.Exists)
                    {
                        // Encryption
                        if (args[0] == "-e")
                        {
                            
                            string encryptedFileName = Path.Join(fileInfo.DirectoryName, fileInfo.Name + _ENCRYPTEDEXTENSION);

                            byte[] bytes = File.ReadAllBytes(fileInfo.FullName);

                            // Calculation of the time takken by the encryption
                            Stopwatch sw = Stopwatch.StartNew();
                            byte[] encryptedBytes = UseXorOnFile(bytes);
                            sw.Stop();

                            long encryptionTime = sw.ElapsedMilliseconds + 1;
                            // The encryption time of each file is added to the final string
                            EncryptionDelaysInMs += $"{fileInfo.Name}:{encryptionTime};";


                            // Writing of the encrypted content in a new file
                            File.WriteAllBytes(encryptedFileName, encryptedBytes);
                            
                            fileInfo.Delete();
                        }
                        // Decryption
                        else if (args[0] == "-d")
                        {
                            string decryptedFileName = Path.Join(fileInfo.DirectoryName, fileInfo.Name.Replace(_ENCRYPTEDEXTENSION,""));
                            byte[] bytes = File.ReadAllBytes(fileInfo.FullName);
                            byte[] decryptedBytes = UseXorOnFile(bytes);

                            File.WriteAllBytes(decryptedFileName, decryptedBytes);
                        }
                    }
                    else

                    {
                        // If file doesn't exist, error in encryption so -1
                        EncryptionDelaysInMs += fileInfo.Name + ":-1;";
                    }
                }
                Console.WriteLine(EncryptionDelaysInMs);
            }
        }

        /// <summary>
        /// Encrypt bytes with XOR
        /// </summary>
        /// <param name="bytes">Bytes from the file to encrypt</param>
        /// <returns>Encrypted bytes</returns>
        private static byte[] UseXorOnFile(byte[] bytes)
        {
            byte[] result = XORCipher(bytes);

            return result;
        }


        /// <summary>
        /// Apply XOR cipher on the entered string (to encrypt or decrypt)
        /// </summary>
        /// <param name="str">string to encrypt with the key</param>
        /// <returns></returns>
        private static byte[] XORCipher(byte[] str)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(_KEY);

            byte[] resultBytes = new byte[str.Length];

            for (int i = 0; i < str.Length; i++)
            {
                resultBytes[i] = (byte)(str[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return resultBytes;
        }

    }
}
