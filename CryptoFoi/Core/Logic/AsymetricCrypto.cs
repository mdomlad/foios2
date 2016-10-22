using System.Web;
using Virgil.Crypto;

namespace CryptoFoi.Core.Logic
{
    public class AsymetricCrypto : ICrypto
    {
        private string _testRecipient = "test";
        public string Decrypt(HttpPostedFileBase file)
        {
            VirgilCipher vc = new VirgilCipher();
            var encryptedText = CryptoLogic.GetTxtFromFile(file);
            return CryptoHelper.Decrypt(encryptedText, _testRecipient, 
                CryptoLogic.KeyCollection[CryptoLogic.PRIVATE]);
        }

        public string Encrypt(HttpPostedFileBase file)
        {
            var txt = CryptoLogic.GetTxtFromFile(file, true);
            return CryptoHelper.Encrypt(txt, _testRecipient, 
                CryptoLogic.KeyCollection[CryptoLogic.PUBLIC]);
        }
    }
}