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
            ViewData["Contracts"] = _DataManager.ConR.GetCollection(CurEmployee.Id);
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
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
        public ActionResult AddContract(string error = "")
        {
 
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Error = error;
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.DateOfSign = String.Join("-", ((DateTime.Now).ToShortDateString()).Split('.').Reverse());

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
                try { pay1 = new DateTime(Convert.ToInt32(Request.Form["1pay"].Split('-')[0]), Convert.ToInt32(Request.Form["1pay"].Split('-')[1]), Convert.ToInt32(Request.Form["1pay"].Split('-')[2])); } catch { }
                try
                { pay2 = new DateTime(Convert.ToInt32(Request.Form["2pay"].Split('-')[0]), Convert.ToInt32(Request.Form["2pay"].Split('-')[1]), Convert.ToInt32(Request.Form["2pay"].Split('-')[2])); }
                catch { }
                try { pay3 = new DateTime(Convert.ToInt32(Request.Form["3pay"].Split('-')[0]), Convert.ToInt32(Request.Form["3pay"].Split('-')[1]), Convert.ToInt32(Request.Form["3pay"].Split('-')[2])); } catch { }

                Int32.TryParse(Request.Form["1paySum"], out int paysum1); Int32.TryParse(Request.Form["2paySum"], out int paysum2); Int32.TryParse(Request.Form["3paySum"], out int paysum3); string comment = Request.Form["Comment"];
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
            //var a = _DataManager.ConR.GetAllPaysForMonth(CurEmployee.Id).Count();
            ViewBag.InformerCount = _DataManager.ConR.GetAllPaysForMonth(CurEmployee.Id).Count();
            ViewBag.InformerSum = _DataManager.ConR.GetAllPaysForMonth(CurEmployee.Id).Select(x => x.Received).Sum();


            //ViewBag.MyJob = _DataManager.PayR.GetEmplJobs(CurEmployee.Id).Except(_DataManager.PayR.GetCollection().Where(x=>x.Employee.Id==CurEmployee.Id));
            ViewBag.MyJob = _DataManager.PayR.GetCollection(true).Where(x=>x.EmployeeTo.Id==CurEmployee.Id&&x.Employee.Id!=CurEmployee.Id);


            ViewBag.MySalary = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == CurEmployee.Id);
            return View();
        }
        [HttpPost]
        public ActionResult MyFinanses()
        {

            if (!string.IsNullOrEmpty(Request.Params.AllKeys.FirstOrDefault(key => key.StartsWith("MyPays")))) //добавление детализации
            {
                return RedirectToAction("AddPaymentDetail");
            }

            else if(!string.IsNullOrEmpty(Request.Params.AllKeys.FirstOrDefault(key => key.StartsWith("MyJob")))) //добавление детализации
            {
                return RedirectToAction("AddMyJobPayment");
            }
            else
            { return RedirectToAction("AddReceivedMoney"); }
        }
        #endregion



        #region Добавление_платежа_в_1_табл
        [HttpGet]
        public ActionResult AddPaymentDetail(string error = "")
        {
 
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");

             ViewBag.DateOfSign = String.Join("-", ((DateTime.Now).ToShortDateString()).Split('.').Reverse());

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Events = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type");
            ViewBag.Accounts = new SelectList(_DataManager.AccR.GetCollection(), "Id", "Type");
            ViewBag.Contracts = new SelectList(_DataManager.ConR.GetCollection(), "Id", "Name");
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
            Adder.Employee = _DataManager.EmR.GetElem(CurEmployee.Id);

            Adder.Comment = Request.Form["Comment"];
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));
            Adder.EmployeeTo = _DataManager.EmR.GetElem(Convert.ToInt32(Request.Form["Employee"]));


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
            ViewBag.DateOfSign = String.Join("-", ((DateTime.Now).ToShortDateString()).Split('.').Reverse());

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

        #region Изменение_контракта
        [HttpGet]
        public ActionResult EditContract(int id = 0, string error = "")
        {
            

            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Error = error;
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Cost = _DataManager.ConR.GetElem(id).Sum_only_contract;
            ViewData["Events"] = new SelectList(_DataManager.EvR.GetCollection("MyFirst"), "Id", "Type", _DataManager.ConR.GetElem(id).EventSet.Id);
            ViewBag.Client = _DataManager.ConR.GetElem(id).ClientSet.FIO;
            ViewBag.DateOfEvent = String.Join("-", (((DateTime)_DataManager.ConR.GetElem(id).Date_of_event).ToShortDateString()).Split('.').Reverse());
            ViewBag.DateOfSign = String.Join("-", (((DateTime)_DataManager.ConR.GetElem(id).Date_of_sign).ToShortDateString()).Split('.').Reverse());
            ViewBag.Comment = _DataManager.ConR.GetElem(id).Comment;
            try { ViewBag.DateOfPay1 = String.Join("-", (((DateTime)_DataManager.ConR.GetElem(id).Payment1Date).ToShortDateString()).Split('.').Reverse()); }
            catch{};
            try { ViewBag.DateOfPay2 = String.Join("-", (((DateTime)_DataManager.ConR.GetElem(id).Payment2Date).ToShortDateString()).Split('.').Reverse()); }
            catch{};
            try { ViewBag.DateOfPay3 = String.Join("-", (((DateTime)_DataManager.ConR.GetElem(id).Payment3Date).ToShortDateString()).Split('.').Reverse()); }
            catch { };
            ViewBag.PaySum1 = _DataManager.ConR.GetElem(id).Payment1Sum ?? null;
            ViewBag.PaySum2 = _DataManager.ConR.GetElem(id).Payment2Sum ?? null;
            ViewBag.PaySum3 = _DataManager.ConR.GetElem(id).Payment3Sum ?? null;
            ViewBag.ID = id;
            return View();
        }

        [HttpPost]
        public ActionResult EditContract()
        {
            try
            {

                DateTime pay1 = default(DateTime), pay2 = default(DateTime), pay3 = default(DateTime);
                int cost = Convert.ToInt32(Request.Form["Cost"]);
                int eventid = Convert.ToInt32(Request.Form["Event"]);
                string clientid = Request.Form["Client"];
                DateTime sign = new DateTime(Convert.ToInt32(Request.Form["calendarContract"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarContract"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarContract"].Split('-')[2]));
                DateTime dateevent = new DateTime(Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarEvent"].Split('-')[2]));
                try { pay1 = new DateTime(Convert.ToInt32(Request.Form["1pay"].Split('-')[0]), Convert.ToInt32(Request.Form["1pay"].Split('-')[1]), Convert.ToInt32(Request.Form["1pay"].Split('-')[2])); } catch { }
                try
                { pay2 = new DateTime(Convert.ToInt32(Request.Form["2pay"].Split('-')[0]), Convert.ToInt32(Request.Form["2pay"].Split('-')[1]), Convert.ToInt32(Request.Form["2pay"].Split('-')[2])); }
                catch { }
                try { pay3 = new DateTime(Convert.ToInt32(Request.Form["3pay"].Split('-')[0]), Convert.ToInt32(Request.Form["3pay"].Split('-')[1]), Convert.ToInt32(Request.Form["3pay"].Split('-')[2])); } catch { }

                Int32.TryParse(Request.Form["paySum1"], out int paysum1); Int32.TryParse(Request.Form["paySum2"], out int paysum2); Int32.TryParse(Request.Form["paySum3"], out int paysum3); string comment = Request.Form["Comment"];
                _DataManager.ConR.Edit(Convert.ToInt32(Request.Form["Id_Cont"]), cost, eventid, clientid, sign, dateevent, pay1, pay2, pay3, paysum1, paysum2, paysum3, comment, CurEmployee.Id);
            }

            catch (Exception) { return RedirectToAction("EditContract", new { error = "Не все поля заполнены" }); }
 
            return RedirectToAction("Index");
        }
            #endregion


            public ActionResult Exit()
        {
            CurEmployee = null;

            return RedirectToAction("Log_in", "User");
        }
        [HttpGet]
        public ActionResult EditPaymentDetail(int id = 0, string error = "")
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Receipt = _DataManager.PayR.GetElem(id).Receipt;
            ViewBag.Comment = _DataManager.PayR.GetElem(id).Comment;

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Events = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type", _DataManager.PayR.GetElem(id).Event.Id);
            ViewBag.Accounts = new SelectList(_DataManager.AccR.GetCollection(), "Id", "Type", _DataManager.PayR.GetElem(id).Account.Id);
            ViewBag.Contracts = new SelectList(_DataManager.ConR.GetCollection(), "Id", "Name", _DataManager.PayR.GetElem(id).Contract.Id);
            ViewBag.Employees = new SelectList(_DataManager.EmR.GetCollection(), "Id", "FIO", _DataManager.EmR.GetElem(CurEmployee.Id));
            ViewBag.Date = String.Join("-", (((DateTime)_DataManager.PayR.GetElem(id).Date).ToShortDateString()).Split('.').Reverse());

            ViewBag.DatePays = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = "Январь", Value = "1"},
                new SelectListItem{Text = "Февраль", Value = "2"},
                new SelectListItem{Text = "Март", Value = "3"},
                new SelectListItem{Text = "Апрель", Value = "4"},
                new SelectListItem{Text = "Май", Value = "5"},
                new SelectListItem{Text = "Июнь", Value = "6"},
                new SelectListItem{Text = "Июль", Value = "7"},
                new SelectListItem{Text = "Август", Value = "8"},
                new SelectListItem{Text = "Сентябрь", Value = "9"},
                new SelectListItem{Text = "Октябрь", Value = "10"},
                new SelectListItem{Text = "Ноябрь", Value = "11"},
                new SelectListItem{Text = "Декабрь", Value = "12"}
            },"Value", "Text", DateTime.Now.Month);
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public ActionResult EditPaymentDetail(int id=0)
        {
            Payments Adder = _DataManager.PayR.GetElem(id);

            Adder.Receipt = Convert.ToInt32(Request.Form["Receipt"]);
            Adder.Account = _DataManager.AccR.GetElem(Convert.ToInt32(Request.Form["Account"]));
            Adder.Contract = _DataManager.ConR.GetElem(Convert.ToInt32(Request.Form["Contract"]));
            Adder.Event = _DataManager.EvR.GetElem(Convert.ToInt32(Request.Form["Event"]));
            Adder.EmployeeTo = _DataManager.EmR.GetElem(Convert.ToInt32(Request.Form["Employee"]));
            Adder.Employee = _DataManager.EmR.GetElem(CurEmployee.Id);
            Adder.Comment = Request.Form["Comment"];
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));
            Adder.DateForPayment =  Convert.ToInt32(Request.Form["DatePay"])>=DateTime.Now.Month? new DateTime(DateTime.Today.Year, Convert.ToInt32(Request.Form["DatePay"]), Adder.Date.Day) : new DateTime(DateTime.Today.Year, Convert.ToInt32(Request.Form["DatePay"]), Adder.Date.Day).AddYears(1);
            _DataManager.PayR.EditPaymentDetail(Adder);
            return RedirectToAction("MyFinanses");
        }

        [HttpGet]
        public ActionResult EditPay_min(int id = 0, string error = "")
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Sum = _DataManager.Pay_minR.GetElem(id).Sum;
 
            ViewBag.Accounts = new SelectList(_DataManager.AccR.GetCollection(), "Id", "Type", _DataManager.Pay_minR.GetElem(id).Account.Id);
       
            ViewBag.Date = String.Join("-", (((DateTime)_DataManager.Pay_minR.GetElem(id).Date).ToShortDateString()).Split('.').Reverse());

            return View();
        }
        [HttpPost]
        public ActionResult EditPay_min(int id = 0)
        {
            Pay_min Adder = _DataManager.Pay_minR.GetElem(id);
            Adder.Sum = Convert.ToInt32(Request.Form["Sum"]);
            Adder.Account = _DataManager.AccR.GetElem(Convert.ToInt32(Request.Form["Account"]));
            Adder.Employee = _DataManager.EmR.GetElem(CurEmployee.Id);
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));
            Adder.Finished = false;


            _DataManager.Pay_minR.Edit_PayMin(Adder);
            return RedirectToAction("MyFinanses");
        }
        [HttpGet]
        public ActionResult AddMyJobPayment(string error = "")
        {

            if (CurEmployee == null) return RedirectToAction("Log_in", "User");

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Contract = new SelectList(_DataManager.ConR.GetCollection(), "Id", "Name");
            ViewBag.Event = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type");
            ViewBag.Employees = new SelectList(_DataManager.EmR.GetCollection(CurEmployee.Id), "Id", "FIO");
            ViewBag.Date = String.Join("-", ((DateTime.Now).ToShortDateString()).Split('.').Reverse());

            return View();
        }
        [HttpPost]
        public ActionResult AddMyJobPayment()

        {


            Payments Adder = new Payments();

            Adder.Receipt = Convert.ToInt32(Request.Form["Sum"]);
             Adder.Contract = _DataManager.ConR.GetElem(Convert.ToInt32(Request.Form["Contract"]));
            Adder.Event = _DataManager.EvR.GetElem(Convert.ToInt32(Request.Form["Event"]));
            Adder.Employee= _DataManager.EmR.GetElem(Convert.ToInt32(Request.Form["Employee"]));
            Adder.EmployeeTo =  _DataManager.EmR.GetElem(CurEmployee.Id);
            Adder.Comment = Request.Form["Comment"];
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));
            Adder.Account = _DataManager.AccR.GetCollection().Where(x => x.Type.Contains("указано")).FirstOrDefault();
                        _DataManager.PayR.Add(Adder);
            return RedirectToAction("MyFinanses");
        }

        public ActionResult DeletePaymentDetail (int id)
        {
            _DataManager.PayR.Delete(id);
            return RedirectToAction("MyFinanses");

        }
        [HttpGet]
        public ActionResult EditPayFromPeer(int id = 0, string error = "")
        {
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Sum = _DataManager.PayR.GetElem(id).Receipt;
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Contract = new SelectList(_DataManager.ConR.GetCollection(), "Id", "Name",_DataManager.PayR.GetElem(id).Contract.Id);
            ViewBag.Event = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type", _DataManager.PayR.GetElem(id).Event.Id);
            ViewBag.Employees = new SelectList(_DataManager.EmR.GetCollection(CurEmployee.Id), "Id", "FIO", _DataManager.PayR.GetElem(id).Employee.Id);
            ViewBag.Date = String.Join("-", ((_DataManager.PayR.GetElem(id).Date).ToShortDateString()).Split('.').Reverse());
            ViewBag.Comment = _DataManager.PayR.GetElem(id).Comment;

            return View();
        }
        [HttpPost]
        public ActionResult EditPayFromPeer(int id = 0)
        {
            Payments Adder = _DataManager.PayR.GetElem(id);

            Adder.Receipt = Convert.ToInt32(Request.Form["Receipt"]);
            Adder.Account = _DataManager.AccR.GetElem(Convert.ToInt32(Request.Form["Account"]));
            Adder.Contract = _DataManager.ConR.GetElem(Convert.ToInt32(Request.Form["Contract"]));
            Adder.Event = _DataManager.EvR.GetElem(Convert.ToInt32(Request.Form["Event"]));
            Adder.EmployeeTo = _DataManager.EmR.GetElem(Convert.ToInt32(Request.Form["Employee"]));
            Adder.EmployeeTo = _DataManager.EmR.GetElem(CurEmployee.Id);
            Adder.Comment = Request.Form["Comment"];
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));

            _DataManager.PayR.EditPaymentDetail(Adder);
            return RedirectToAction("MyFinanses");
        }
    }

}