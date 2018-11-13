using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP_Love_Gid.Models;
namespace ERP_Love_Gid.Controllers
{
    public class UserController : Controller
    {
        private DataManager _DataManager;
        public UserController(DataManager _DM)
        {
            _DataManager = _DM;

        }
        [HttpGet]
        public ActionResult Log_in(int id = 0, string errormessage = "")
        {
            
            ViewBag.UserId = id;
            ViewBag.Error = errormessage;
           

            return View();
        }

        [HttpPost]
        public ActionResult Log_in()
        {
            if (_DataManager.EmR.Contains(Request.Form["Login"]))
            {
                Employee curUser = _DataManager.EmR.GetElem(_DataManager.EmR.Getid(Request.Form["Login"]));
                 if (curUser.Login == Request.Form["Login"] &&
                curUser.Password == Request.Form["Password"])
                        return RedirectToAction("Index", "Home", new { idUser = _DataManager.EmR.Getid(Request.Form["Login"]) });
                 else return RedirectToAction("Log_in", new { errormessage = "Неверный логин или пароль" });
            }
            else { return RedirectToAction("Log_in", new { errormessage = "Данный пользователь не найден" }); }


        }
        public ActionResult Exit()
        {

            return RedirectToAction("Log_in", "User");
        }
    }
}