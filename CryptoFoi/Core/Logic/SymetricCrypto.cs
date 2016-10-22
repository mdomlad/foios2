using System.Text;
using System.Web;
using Virgil.Crypto;

namespace CryptoFoi.Core.Logic
{
    public class SymetricCrypto : ICrypto
    {
        public string Decrypt(HttpPostedFileBase file)
        {
            var encryptedText = CryptoLogic.GetTxtFromFile(file);
            var password = Encoding.UTF8.GetString(CryptoLogic.KeyCollection[CryptoLogic.SECRET]);
            return CryptoHelper.Decrypt(encryptedText, password);
        }

        public string Encrypt(HttpPostedFileBase file)
        {
            var txt = CryptoLogic.GetTxtFromFile(file, true);
            var password = Encoding.UTF8.GetString(CryptoLogic.KeyCollection[CryptoLogic.SECRET]);            
            return CryptoHelper.Encrypt(txt,password);
        }
    }
}