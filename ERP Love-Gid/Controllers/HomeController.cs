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
    public class HomeController : Controller
    {
        private DataManager _DataManager;
        private static Employee CurEmployee;

        public HomeController(DataManager _DM)

        {

            _DataManager = _DM;
        }
#region Мои_Договоры
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
            ViewBag.User = CurEmployee.Surname+" "+CurEmployee.Name;
            return View();
        }
        [HttpPost]
        public ActionResult Index()
        {
            return RedirectToAction("AddContract");
        }
        #endregion

#region Добавление_контракта
        [HttpGet]
        public ActionResult AddContract(string error="")
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Error = error;
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name; ;

            if (_DataManager.EvR != null)
                ViewData["Events"] = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type");

            return View();
        }
        [HttpPost]
        public ActionResult AddContract(int id = 0)
        {

            try
            {
                DateTime pay1 = default(DateTime), pay2 = default(DateTime), pay3 = default(DateTime);
                int cost = Convert.ToInt32(Request.Form["Cost"]);
                int eventid = Convert.ToInt32(Request.Form["Event"]);
                string clientid = Request.Form["Client"];
                DateTime sign = new DateTime(Convert.ToInt32(Request.Form["calendarContract"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarContract"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarContract"].Split('-')[2]));
                DateTime dateevent = new DateTime(Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[2]));
                try {   pay1 = new DateTime(Convert.ToInt32(Request.Form["1pay"].Split('-')[0]), Convert.ToInt32(Request.Form["1pay"].Split('-')[1]), Convert.ToInt32(Request.Form["1pay"].Split('-')[2])); } catch { }
                try
                {   pay2 = new DateTime(Convert.ToInt32(Request.Form["2pay"].Split('-')[0]), Convert.ToInt32(Request.Form["2pay"].Split('-')[1]), Convert.ToInt32(Request.Form["2pay"].Split('-')[2])); }
                catch { }
                try {   pay3 = new DateTime(Convert.ToInt32(Request.Form["3pay"].Split('-')[0]), Convert.ToInt32(Request.Form["3pay"].Split('-')[1]), Convert.ToInt32(Request.Form["3pay"].Split('-')[2])); } catch { }

                Int32.TryParse(Request.Form["1paySum"], out int paysum1); Int32.TryParse(Request.Form["2paySum"], out int paysum2); Int32.TryParse(Request.Form["3paySum"], out int paysum3);  string comment = Request.Form["Comment"];
                _DataManager.ConR.Add(cost, eventid, clientid, sign, dateevent, pay1, pay2, pay3, paysum1, paysum2, paysum3, comment, CurEmployee.Id);
            }
          
            catch (Exception) { return RedirectToAction("AddContract", new { error = "Не все поля заполнены" }); }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult MeesageHandler(string data)
        {
            var result = "Сообщение " + data + "принято";
    return Json(result);
        }

        #endregion

        #region Мои_Финансы
        [HttpGet]
        public ActionResult MyFinanses(string error = "")
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name; ;
            ViewBag.Payments = _DataManager.PayR.GetEmplPays(CurEmployee.Id);
            ViewBag.PaymentSum = _DataManager.PayR.GetEmplPaysSum(CurEmployee.Id);
            ViewBag.Pay_min = _DataManager.Pay_minR.GetEmplPays(CurEmployee.Id);
            ViewBag.Pay_minSum = _DataManager.Pay_minR.GetEmplPaysSum(CurEmployee.Id);

            ViewBag.Informer = _DataManager.ConR.GetAllPaysForMonth(CurEmployee.Id);
            var a = _DataManager.ConR.GetAllPaysForMonth(CurEmployee.Id).Count();
            ViewBag.InformerCount = _DataManager.ConR.GetAllPaysForMonth(CurEmployee.Id).Count();
            ViewBag.InformerSum = _DataManager.ConR.GetAllPaysForMonth(CurEmployee.Id).Select(x=>x.Received).Sum();




            return View();
        }
        [HttpPost]
        public ActionResult MyFinanses()
        {

            if (!string.IsNullOrEmpty(Request.Params.AllKeys.FirstOrDefault(key => key.StartsWith("MyPays")))) //добавление детализации
            {
                return RedirectToAction("AddPaymentDetail");
            }

            else { return RedirectToAction("AddReceivedMoney"); }
         }
        #endregion


      
#region Добавление_платежа_в_1_табл
        [HttpGet]
        public ActionResult AddPaymentDetail(string error = "")
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Events = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type");
            ViewBag.Accounts = new SelectList(_DataManager.AccR.GetCollection(), "Id", "Type");
            ViewBag.Contracts =  new SelectList(_DataManager.ConR.GetCollection(), "Id", "Name");
            ViewBag.Employees = new SelectList(_DataManager.EmR.GetCollection(), "Id", "FIO");
            return View();
        }
        [HttpPost]
        public ActionResult AddPaymentDetail()
        {
            Payments Adder = new Payments();
            
            Adder.Receipt = Convert.ToInt32(Request.Form["Receipt"]);
            Adder.Account = _DataManager.AccR.GetElem(Convert.ToInt32(Request.Form["Account"]));
            Adder.Contract = _DataManager.ConR.GetElem(Convert.ToInt32(Request.Form["Contract"]));
            Adder.Event = _DataManager.EvR.GetElem(Convert.ToInt32(Request.Form["Event"]));
            Adder.Employee = _DataManager.EmR.GetElem(Convert.ToInt32(Request.Form["Employee"]));

            Adder.Comment = Request.Form["Comment"];
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));



            _DataManager.PayR.Add(Adder);
            return RedirectToAction("MyFinanses");
        }
        #endregion

#region Добавление_платежа_в_табл_2
        /// <summary>
        /// добавление в таблицу СДАНО
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddReceivedMoney(string error = "")
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Account = new SelectList(_DataManager.AccR.GetCollection(), "Id", "Type");
            return View();
        }
        [HttpPost]
        public ActionResult AddReceivedMoney()

        {
            Pay_min Adder = new Pay_min();
            Adder.Sum = Convert.ToInt32(Request.Form["Sum"]);
            Adder.Account = _DataManager.AccR.GetElem(Convert.ToInt32(Request.Form["Account"]));
            Adder.Employee = _DataManager.EmR.GetElem(CurEmployee.Id);   
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));
            Adder.Finished = false;


            _DataManager.Pay_minR.Add(Adder);
            return RedirectToAction("MyFinanses");
        }
        #endregion

       
        public ActionResult EditContract(int id=0, string error = "")
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Error = error;
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name; 

            return View();
        }



        public ActionResult Exit()
        {
            CurEmployee = null;

            return RedirectToAction("Log_in", "User");
        }
    }
    
}