using CryptoFoi.Core.Logic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CryptoFoi.Models
{
    public abstract class CipherModel: DigitalSignature
    {
        [Display(Name = "Message hash")]
        public string SuccessResult { get; set; }

        [Display(Name = "Message hash")]
        public string MessageHash { get; set; }

        [Display(Name = "Error: ")]
        public string FileError { get; set; }

        [Display(Name = "Filename: ")]
        public string Filename { get; set; }

        public string DecryptedText { get; set; }
    }
}