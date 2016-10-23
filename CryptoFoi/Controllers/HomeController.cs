using CryptoFoi.Core.Exceptions;
using CryptoFoi.Core.Logic;
using CryptoFoi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CryptoFoi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Tab = "Encryption";
            return View(new CipherViewModel());
        }

        [HttpPost]
        public ActionResult Index(CipherViewModel model, string cryptDirection, string signDirection, string download, string hash,
            string Method_Encryption_Sign, string Method_Decryption_Verify)
        {
            try
            {
                if (!string.IsNullOrEmpty(download) && !string.IsNullOrEmpty(model.Filename))
                {
                    ViewBag.Tab = download;
                    var stream = new FileStream(model.Filename, FileMode.Open, FileAccess.Read);
                    return new FileStreamResult(stream, "text/plain");
                }
                if (!string.IsNullOrEmpty(hash))
                {
                    ViewBag.Tab = hash;
                    model.CalculateHash();
                }
                else {
                    var direction = string.IsNullOrEmpty(cryptDirection) ? signDirection : cryptDirection;
                    ViewBag.Tab = direction;
                    var method = (direction == "Encryption" || direction == "Sign") ? Method_Encryption_Sign : Method_Decryption_Verify;
                    model.Crypto = GetCryptoMethod(method);
                    DoWork(model, direction, Request.Files);
                }
            }
            catch (Exception e)
            {
                if (e.Source == "Virgil.Crypto")
                    model.FileError = "Don't cheat. It's bulletproof :)";
                else
                    model.FileError = e.Message;
            }

            return View(model);
        }

        private void DoWork(CipherViewModel model, string direction, HttpFileCollectionBase files)
        {
            HttpPostedFileBase file;
            switch (direction)
            {
                case "Encryption":
                    file = files[0];
                    if (file.ContentLength == 0) throw new FileNotFoundException();
                    model.Encrypt(file);
                    model.SuccessResult = "File encrypted succesfully.";
                    break;
                case "Decryption":
                    file = files[1];
                    if (file.ContentLength == 0) throw new FileNotFoundException();
                    model.SuccessResult = model.Decrypt(file);
                    break;
                case "Sign":
                    file = files[0];
                    if (file.ContentLength == 0) throw new FileNotFoundException();
                    model.Sign(file);
                    model.SuccessResult = "File signed succesfully.";
                    break;
                case "Verify":
                    file = files[1];
                    if (file.ContentLength == 0) throw new FileNotFoundException();
                    if (model.Verify(file))
                        model.SuccessResult = "Verification is success";
                    else
                        model.FileError = "Verification went wrong :(";
                    break;
                default:
                    throw new MethodAccessException();
            }
        }

        private ICrypto GetCryptoMethod(string methodName)
        {
            ICrypto crypto = null;
            if (methodName == "Asymetric") crypto = new AsymetricCrypto();
            if (methodName == "Symetric") crypto = new SymetricCrypto();

            return crypto;
        }
    }
}