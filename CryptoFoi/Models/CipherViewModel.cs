using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CryptoFoi.Models
{
    public class CipherViewModel
    {
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
        [Display(Name = "File to decrypt")]
        public HttpPostedFileBase EncryptedFile { get; set; }

        [FileExtensions(Extensions = "txt",
              ErrorMessage = "Specify a text file.")]
        [Display(Name = "File to encrypt")]
        public HttpPostedFileBase TextFile { get; set; }

        [Display(Name = "Message hash")]
        public string MessageHash { get; set; }

        public string FileError { get; set; }
    }
}