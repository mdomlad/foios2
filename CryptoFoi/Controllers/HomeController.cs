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
            return View();
        }

        [HttpPost]
        public ActionResult Index(CipherViewModel model)
        {
            if (Request.Files.Count > 0 
                && (Request.Files[0].ContentLength > 0 
                || Request.Files[1].ContentLength > 0))
            {
                var crypto = new RsaCrypto(model, Request.Files);
            } else
            {
                model.FileError = "You must specify one of the files!";
            }
            return View(model);
        }
    }
}