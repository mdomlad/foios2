using CryptoFoi.Core.CryptoCoding;
using CryptoFoi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CryptoFoi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new CipherViewModel());
        }

        [HttpPost]
        public ActionResult Index(CipherViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.SaveKeys();
                return RedirectToAction("Crypto");
            }

            return View(model);
        }

        public ActionResult Crypto()
        {
            return View(new CipherViewModel());
        }

        [HttpPost]
        public ActionResult Crypto(CipherViewModel model)
        {
            if (Request.Files.Count > 0)
            {
                var crypto = new RsaCrypto(model, Request.Files);
                model.MessageHash = crypto.GetHash();
            }
            else
            {
                model.FileError = "You must specify one of the files!";
            }
            return View(model);
        }
    }
}