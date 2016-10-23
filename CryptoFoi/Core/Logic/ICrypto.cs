using System.Web;

namespace CryptoFoi.Core.Logic
{
    public interface ICrypto
    {
        string Encrypt(HttpPostedFileBase file);
        string Decrypt(HttpPostedFileBase file);
    }
}