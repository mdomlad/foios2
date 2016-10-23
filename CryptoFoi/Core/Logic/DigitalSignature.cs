using System.IO;
using System.Web;
using Virgil.Crypto;

namespace CryptoFoi.Core.Logic
{
    public abstract class DigitalSignature
    {
        private VirgilSigner _signer;

        public DigitalSignature()
        {
            _signer = new VirgilSigner(Virgil.Crypto.Foundation.VirgilHash.Algorithm.SHA512);
        }
        public void Sign(HttpPostedFileBase file)
        {
            var data = CryptoLogic.GetBytesFromFile(file);
            var digitalSignature = _signer.Sign(data, CryptoLogic.KeyCollection[CryptoLogic.PRIVATE]);
            File.WriteAllBytes(CryptoLogic.SignatureFileName, digitalSignature);
        }

        public bool Verify(HttpPostedFileBase file)
        {
            var rawData = File.ReadAllBytes(CryptoLogic.PlainTxtFile);
            var signedData = CryptoLogic.GetBytesFromFile(file);
            var isVerified = _signer.Verify(rawData, signedData, CryptoLogic.KeyCollection[CryptoLogic.PUBLIC]);

            return isVerified;
        }
    }
}