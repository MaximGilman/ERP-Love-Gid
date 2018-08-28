using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP_Love_Gid.Models;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
namespace ERP_Love_Gid.Controllers
{
    public class HomeController : Controller
    {
        private DataManager _DataManager;
        private static Employee CurEmployee;

        public HomeController(DataManager _DM)

        {

            _DataManager = _DM;
        }

        /// <summary>
        /// Мои договоры
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int idUser = 0, string sort = "")
        {


            CurEmployee = idUser == 0 ? CurEmployee : _DataManager.EmR.GetElem(idUser);
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");

            // ViewData["Years"] = new SelectList(_DataManager.ConR.GetYears()); 
            ViewData["Contracts"] = _DataManager.ConR.GetCollection();
            ViewBag.User = CurEmployee.FIO;
            return View();
        }
        [HttpPost]
        public ActionResult Index()
        {
            return RedirectToAction("AddContract");
        }
        [HttpGet]
        public ActionResult AddContract()
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");

            ViewBag.User = CurEmployee.FIO;

            if (_DataManager.EvR != null)
                ViewData["Events"] = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type");

            return View();
        }
        [HttpPost]
        public ActionResult AddContract(int id = 0)
        {
            _DataManager.ConR.Add(Convert.ToInt32(Request.Form["Cost"]), Convert.ToInt32(Request.Form["Event"]), Request.Form["Client"], new DateTime(Convert.ToInt32(Request.Form["calendarContract"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarContract"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarContract"].Split('-')[2]))
                , new DateTime(Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[2])), new DateTime(Convert.ToInt32(Request.Form["1pay"].Split('-')[0]), Convert.ToInt32(Request.Form["1pay"].Split('-')[1]), Convert.ToInt32(Request.Form["1pay"].Split('-')[2])),
               new DateTime(Convert.ToInt32(Request.Form["2pay"].Split('-')[0]), Convert.ToInt32(Request.Form["2pay"].Split('-')[1]), Convert.ToInt32(Request.Form["2pay"].Split('-')[2])),
              new DateTime(Convert.ToInt32(Request.Form["3pay"].Split('-')[0]), Convert.ToInt32(Request.Form["3pay"].Split('-')[1]), Convert.ToInt32(Request.Form["3pay"].Split('-')[2])),
              Convert.ToInt32(Request.Form["1paySum"]), Convert.ToInt32(Request.Form["2paySum"]), Convert.ToInt32(Request.Form["3paySum"]), Request.Form["Comment"], CurEmployee.Id);
            //Contract add = new Contract();
            //add.Sum_only_contract = Convert.ToInt32(Request.Form["Cost"]);
            //add.EventSet = _DataManager.EvR.GetElem(Convert.ToInt32(Request.Form["Event"]));

            //add.ClientSet = _DataManager.CliR.Add(Request.Form["Client"]);
            //add.Date_of_sign = new DateTime(Convert.ToInt32(Request.Form["calendarContract"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarContract"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarContract"].Split('-')[2]));
            //add.Date_of_event = new DateTime(Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[2]));
            //add.Payment1Date = new DateTime(Convert.ToInt32(Request.Form["1pay"].Split('-')[0]), Convert.ToInt32(Request.Form["1pay"].Split('-')[1]), Convert.ToInt32(Request.Form["1pay"].Split('-')[2]));
            //add.Payment2Date = new DateTime(Convert.ToInt32(Request.Form["2pay"].Split('-')[0]), Convert.ToInt32(Request.Form["2pay"].Split('-')[1]), Convert.ToInt32(Request.Form["2pay"].Split('-')[2]));
            //add.Payment3Date = new DateTime(Convert.ToInt32(Request.Form["3pay"].Split('-')[0]), Convert.ToInt32(Request.Form["3pay"].Split('-')[1]), Convert.ToInt32(Request.Form["3pay"].Split('-')[2]));
            //add.Payment1Sum = Convert.ToInt32(Request.Form["1paySum"]); add.Payment2Sum = Convert.ToInt32(Request.Form["2paySum"]);
            //add.Payment3Sum = Convert.ToInt32(Request.Form["3paySum"]);

            //add.Comment = Request.Form["Comment"];
            //add.Received = 0;
            //add.Sum_plus = 0;
            //add.Status = Status.Активный.ToString();
            //add.EmployeeSet = CurEmployee; // Убрать потом при регистрации!!!!!!!
            //add.Name = add.Date_of_event.ToString().Split(' ')[0] + " — " +" ("+ add.EmployeeSet.FIO +") "+ add.ClientSet.FIO;
            //_DataManager.ConR.Add(add);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult MyFinanses(string error = "")
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");

            ViewBag.User = CurEmployee.FIO;
            ViewBag.Payments = _DataManager.PayR.GetEmplPays(CurEmployee.Id);
            ViewBag.PaymentSum = _DataManager.PayR.GetEmplPaysSum(CurEmployee.Id);
            return View();
        }
        [HttpPost]
        public ActionResult MyFinanses()
        {

            if (!string.IsNullOrEmpty(Request.Params.AllKeys.FirstOrDefault(key => key.StartsWith("MyPays")))) //добавление детализации
            {
                return RedirectToAction("AddPaymentDetail");
            }
            // 
            else
                return RedirectToAction("MyFinanses");
        }
        [HttpGet]
        public ActionResult AddPaymentDetail(string error = "")
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");

            ViewBag.User = CurEmployee.FIO;
            ViewBag.Events = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type");
            ViewBag.Accounts = new SelectList(_DataManager.AccR.GetCollection(), "Id", "Type");
            ViewBag.Contracts = new SelectList(_DataManager.ConR.GetCollection(), "Id", "Name");
            ViewBag.Employees = new SelectList(_DataManager.EmR.GetCollection(), "Id", "FIO");
            return View();
        }
        public ActionResult Exit()
        {
            CurEmployee = null;

            return RedirectToAction("Log_in", "User");
        }
        [HttpPost]
        public ActionResult AddPaymentDetail()
        {
            Payments Adder = new Payments();
            Adder.Receipt = Convert.ToInt32(Request.Form["Receipt"]);
            Adder.Account = _DataManager.AccR.GetElem(Convert.ToInt32(Request.Form["Account"]));
            Adder.Contract = _DataManager.ConR.GetElem(Convert.ToInt32(Request.Form["Contract"]));
            Adder. = _DataManager.ConR.GetElem(Convert.ToInt32(Request.Form["Contract"]));

            _DataManager.PayR.Add(Adder);
            return RedirectToAction("MyFinanses");
        }
    }
}