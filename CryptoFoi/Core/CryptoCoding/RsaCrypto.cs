using CryptoFoi.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using CryptoFoi.Models;

namespace CryptoFoi.Core.CryptoCoding
{
    public class RsaCrypto : ICrypto
    {
        public readonly static string _appDataPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Public");
        private readonly static string _secretKeyFileName = Path.Combine(_appDataPath, "tajni_kljuc.txt");
        private readonly static string _publicKeyFileName = Path.Combine(_appDataPath, "javni_kljuc.txt");
        private readonly static string _privateKeyFileName = Path.Combine(_appDataPath, "privatni_kljuc.txt");

        private CipherViewModel model;
        private HttpFileCollectionBase files;

        public RsaCrypto(CipherViewModel model, HttpFileCollectionBase files)
        {
            this.model = model;
            this.files = files;
        }

        public void Encrypt(string fileName)
        {
            // Instantiate the crypto provider
            RijndaelManaged rm = new RijndaelManaged();

            // Set up information needed by the provider
            byte[] Key = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                          0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14,
                          0x15, 0x16};
            byte[] IV = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                         0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14,
                         0x15, 0x16};

            // Create the encryption transformer
            ICryptoTransform trans = rm.CreateEncryptor(Key, IV);

            // Prepare the crypto stream for writing
            FileStream file = new FileStream(fileName,
                                                   FileMode.Create);
            CryptoStreamMode mode = CryptoStreamMode.Write;
            CryptoStream enc = new CryptoStream(file, trans, mode);

            // Write to the crypto stream
            StreamWriter writer = new StreamWriter(enc);
            writer.WriteLine("Hello, encrypted world");

            // Clean-up
            writer.Close();
            enc.Close();
            file.Close();
        }

        public string Decrypt(string fileName)
        {
            RijndaelManaged rm = new RijndaelManaged();

            // Set up information needed by the provider
            // (Must use the same key and IV vector as above)
            byte[] Key = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                          0x08, 0x09, 0x10, 0x11, 0x12, 0x13,
                          0x14, 0x15, 0x16};
            byte[] IV = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                         0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14,
                         0x15, 0x16};

            // Create the decryption transformer
            ICryptoTransform trans = rm.CreateDecryptor(Key, IV);

            // Prepare the crypto stream for reading
            FileStream file = new FileStream(fileName,
                                                   FileMode.Open);
            CryptoStreamMode mode = CryptoStreamMode.Read;
            CryptoStream enc = new CryptoStream(file, trans, mode);

            // Read from the crypto stream
            StreamReader reader = new StreamReader(enc);
            string buf = reader.ReadToEnd();

            // Clean-up
            reader.Close();
            enc.Close();
            file.Close();

            // Return the decrypted text
            return buf;
        }

        public void Sign()
        {
            throw new NotImplementedException();
        }
        public bool CheckSignature()
        {
            throw new NotImplementedException();
        }
    }
}