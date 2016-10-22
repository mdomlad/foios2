using CryptoFoi.Core.Logic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System;

namespace CryptoFoi.Models
{    
    public class CipherViewModel: CipherModel
    {
        public ICrypto Crypto { set { _crypto = value; } }
        private ICrypto _crypto;
        private List<string> _methods = new List<string> { "Symetric", "Asymetric" };
        public SelectList CryptMethods { get; set; }
        public SelectList Files { get { return new SelectList(CryptoLogic.Files, "Value", "Text"); } }

        public CipherViewModel()
        {
            CryptMethods = new SelectList(_methods);
            Init();
        }

        public CipherViewModel(ICrypto crypto)
            : this()
        {
            Crypto = crypto;
        }

        private void Init()
        {
            if (!CryptoLogic.LoadKeys())
            {
                CryptoLogic.GenerateKeys();
                CryptoLogic.LoadKeys();
            }
        }

        public void Encrypt(HttpPostedFileBase file)
        {
            var encryptedTxt = _crypto.Encrypt(file);
            File.WriteAllText(CryptoLogic.EncryptFileName, encryptedTxt);
        }

        public void CalculateHash()
        {
            var bytes = File.ReadAllBytes(CryptoLogic.PlainTxtFile);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));

                MessageHash = hashedInputStringBuilder.ToString();
            }
        }

        public string Decrypt(HttpPostedFileBase file)
        {
            return _crypto.Decrypt(file);
        }
    }
}