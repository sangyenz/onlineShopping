using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models;

namespace WebApplication11.Controllers
{
    public class DefaultController : Controller
    {
        private Entities db = new Entities();
        public DefaultController()
        {

            ViewBag.Categories = db.Categories.Where(flg => flg.CancelFlg == 0).ToList();
        }
    }
}
