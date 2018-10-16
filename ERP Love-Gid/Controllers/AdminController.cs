using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP_Love_Gid.Models;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Runtime.InteropServices;

namespace ERP_Love_Gid.Controllers
{
    public class AdminController : Controller
    {
        private DataManager _DataManager;
        private static Employee CurEmployee;
        public AdminController(DataManager _DM)

        {

            _DataManager = _DM;
        }
        [HttpGet]
        public ActionResult Index(int id)
        {
            CurEmployee = _DataManager.EmR.GetElem(id);
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            return View();
        }
        public ActionResult ChangeToUser()
        {
            return RedirectToAction("Index", "Home", new { idUser = CurEmployee.Id });
        }
        public ActionResult Exit()
        {
           return RedirectToActionPermanent("Exit", "Home");
        }
    }
}