using CryptoFoi.Core.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Virgil.Crypto;
using System.Web.Mvc;

namespace CryptoFoi.Core.Logic
{
    public static class CryptoLogic
    {
        public const string SECRET = "Secret";
        public const string PRIVATE = "Private";
        public const string PUBLIC = "Public";
        public const string SIGNATURE = "DigitalSignature";
        public const string ENCRYPTED = "Encrypted";
        public const string PLAIN_TEXT = "PlainText";

        public readonly static string PublicDataPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Public");

        public readonly static List<SelectListItem> Files = new List<SelectListItem>
        {
            new SelectListItem {  Text = SECRET, Value = GetFileName(SECRET) },
            new SelectListItem {  Text = PRIVATE, Value = GetFileName(PRIVATE) },
            new SelectListItem {  Text = PUBLIC, Value = GetFileName(PUBLIC) },
            new SelectListItem {  Text = SIGNATURE, Value = GetFileName(SIGNATURE, false) },
            new SelectListItem {  Text = ENCRYPTED, Value = GetFileName(ENCRYPTED, false) },
            new SelectListItem {  Text = PLAIN_TEXT, Value = GetFileName(PLAIN_TEXT, false) }

        };

        public static string EncryptFileName => GetFileName(ENCRYPTED, false);

        public static string SignatureFileName => GetFileName(SIGNATURE, false);

        public static string PlainTxtFile => GetFileName(PLAIN_TEXT, false);

        public static void GenerateKeys()
        {
            var keyPair = VirgilKeyPair
                .Generate(VirgilKeyPair.Type.FAST_EC_ED25519);
            //Will use generator but it is used for symetric crypto
            var secret = VirgilKeyPair
                .Generate(VirgilKeyPair.Type.FAST_EC_X25519)
                .PrivateKey();

            File.WriteAllBytes(GetFileName(PUBLIC), keyPair.PublicKey());
            File.WriteAllBytes(GetFileName(PRIVATE), keyPair.PrivateKey());
            File.WriteAllBytes(GetFileName(SECRET), secret);
        }

        public readonly static string[] KeyNames = { SECRET, PUBLIC, PRIVATE };

        private static Dictionary<string, byte[]> _keyCollection;

        public static Dictionary<string, byte[]> KeyCollection { get { return _keyCollection; } }

        public static bool LoadKeys()
        {
            _keyCollection = new Dictionary<string, byte[]>();
            foreach (var keyName in KeyNames)
            {
                if (!File.Exists(GetFileName(keyName))) return false;

                var val = File.ReadAllBytes(GetFileName(keyName));

                if (val == null) return false;

                _keyCollection.Add(keyName, val);

            }
            return true;
        }

        public static string GetFileName(string name, bool isKey = true)
        {
            return Path.Combine(PublicDataPath, string.Format("{0}{1}.txt", name, isKey ? "_Key" : string.Empty));
        }

        public static string GetTxtFromFile(HttpPostedFileBase file, bool saveOnServer = false)
        {
            if (file == null) throw new FileNotFoundException();

            var txt = System.Text
                 .Encoding.UTF8.GetString(GetBytesFromFile(file));

            if (txt.Length > 10000) throw new TextToLongException();

            if(saveOnServer) File.WriteAllText(PlainTxtFile, txt);

            return txt;
        }

        public static byte[] GetBytesFromFile(HttpPostedFileBase file)
        {
            if (file == null) throw new FileNotFoundException();

            BinaryReader b = new BinaryReader(file.InputStream);
            byte[] bytes = b.ReadBytes((int)file.InputStream.Length);

            if (bytes.Length > 80000) throw new TextToLongException();

            return bytes;
        }
    }
}