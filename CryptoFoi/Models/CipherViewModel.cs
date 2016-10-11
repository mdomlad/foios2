using CryptoFoi.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoFoi.Models
{    
    public class CipherViewModel
    {
        public CipherViewModel()
        {
            CryptMethods = new SelectList(_methods);
        }

        private List<string> _methods = new List<string>{ "Simetric", "Asymetric" };

        private string[] _keyNames = { "Secret", "Public", "Private" };

        public SelectList CryptMethods { get; set; }

        [Display(Name = "Choose algorithm type")]
        [Required]
        public string Method { get; set; }

        [Display(Name = "Secret")]
        [Required]
        public string SecretKeyText { get; set; }

        [Display(Name = "Public")]
        [Required]
        public string PublicKeyText { get; set; }

        [Display(Name = "Private")]
        [Required]
        public string PrivateKeyText { get; set; }

        [FileExtensions(Extensions = "txt",
              ErrorMessage = "Specify a text file.")]
        public HttpPostedFileBase TextFile { get; set; }

        [Display(Name = "Message hash")]
        public string MessageHash { get; set; }

        [Display(Name = "Error: ")]
        public string FileError { get; set; }

        internal void SaveKeys()
        {
            foreach (var keyName in _keyNames)
            {
                var val = GetType()
                    .GetProperty(GetPropName(keyName))
                    .GetValue(this, null)
                    .ToString();

                if(val != null)
                {
                    File.WriteAllText(GetFileName(keyName), val);
                }
            }
        }

        private string GetPropName(string keyName)
        {
            return string.Format("{0}KeyText", keyName);
        }

        private string GetFileName(string keyName)
        {
            return Path.Combine(FileHelper.PublicDataPath, string.Format("{0}_Key.txt", keyName));
        }
    }
}