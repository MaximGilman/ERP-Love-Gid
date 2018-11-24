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
        int currmonth = DateTime.Now.Month;
        dynamic MothsForChoose = new List<SelectListItem>
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
            };
        private DataManager _DataManager;
        private Employee CurEmployee;
        private  Employee AdminEmployee;
         public static bool lookfromAdmin = false;


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
        public ActionResult Index(int idUser = 0, string sort = "", bool isadmin = false, int adminId = 0, int numberofMonth = -1, int CurUserId = -1)
            {
            if (CurUserId == -1)
                CurEmployee = idUser == 0 ? CurEmployee : _DataManager.EmR.GetElem(idUser);

            else
                CurEmployee = CurUserId == -1 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);


            if (CurEmployee == null) return RedirectToAction("Log_in", "User");

            // ViewData["Years"] = new SelectList(_DataManager.ConR.GetYears()); 

            ViewBag.CurUserId = CurEmployee.Id;
            ViewBag.adminId = adminId;
            AdminEmployee = _DataManager.EmR.GetElem(adminId);
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            if (CurEmployee.IsAdmin) ViewBag.Admin = true;
            if (isadmin) { lookfromAdmin = true; AdminEmployee = _DataManager.EmR.GetElem(adminId); ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name; }
            if (lookfromAdmin) { ViewBag.EmplNameForAdmin = "просматривает работника " + CurEmployee.Name; ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name; }
            if (numberofMonth != -1) { currmonth = numberofMonth + 1; ViewData["Contracts"] = _DataManager.ConR.GetCollection(CurEmployee.Id).Where(x => x.Date_of_event.Value.Month == (currmonth)); }
            else { currmonth = DateTime.Now.Month; ViewData["Contracts"] = _DataManager.ConR.GetCollection(CurEmployee.Id).Where(x => x.Date_of_event.Value.Month == (currmonth)); }
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);
            return View();
        }
        [HttpPost]
        public ActionResult Index(int CurUserId, int adminId)
        {


            return RedirectToAction("AddContract", new { CurUserId, adminId });
        }
        #endregion

        #region Добавление_контракта
        [HttpGet]
        public ActionResult AddContract(string error = "", int CurUserId = 0, int adminId =0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Error = error;
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.DateOfSign = String.Join("-", ((DateTime.Now).ToShortDateString()).Split('.').Reverse());
            ViewBag.CurUserId = CurEmployee.Id;
            ViewBag.adminId = adminId;

            if (CurEmployee.IsAdmin) ViewBag.Admin = true;
            AdminEmployee = _DataManager.EmR.GetElem(adminId);

            if (_DataManager.EvR != null)
                ViewData["Events"] = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type");
            if (lookfromAdmin) { ViewBag.EmplNameForAdmin = "просматривает работника " + CurEmployee.Name; ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name; }

            return View();
        }
        [HttpPost]
        public ActionResult AddContract(int id = 0, int CurUserId = 0, int adminId = 0)
        {

            try
            {
                CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

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

            catch (Exception) { return RedirectToAction("AddContract", new { error = "Не все поля заполнены", CurUserId , adminId }); }
            return RedirectToAction("Index", new { idUser = CurUserId, adminId });
        }
        //[HttpPost]
        //public JsonResult MeesageHandler(string data)
        //{
        //    var result = "Сообщение " + data + "принято";
        //    return Json(result);
        //}

        #endregion

        #region Мои_Финансы
        [HttpGet]
        public ActionResult MyFinanses(string error = "", int numberofMonth = -1, int CurUserId = 0, int adminId=0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
 
            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            if (numberofMonth != -1)
            {
                currmonth = numberofMonth + 1;
            }
            else currmonth = DateTime.Now.Month;
            //////
            

            ViewBag.CurUserId = CurEmployee.Id;
            ViewBag.adminId = adminId;
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name; ;
            ViewBag.Payments = _DataManager.PayR.GetEmplPays(CurEmployee.Id).Where(x => x.Date.Month == currmonth);
            ViewBag.PaymentSum = _DataManager.PayR.GetEmplPaysSum(CurEmployee.Id, currmonth);
            ViewBag.Pay_min = _DataManager.Pay_minR.GetEmplPays(CurEmployee.Id).Where(x => x.Date.Month == currmonth);
            ViewBag.Pay_minSum = _DataManager.Pay_minR.GetEmplPaysSum(CurEmployee.Id, currmonth);

            ViewBag.Informer = _DataManager.ConR.GetAllPaysForMonth(CurEmployee.Id, DateTime.Now.Year, currmonth); //Опасно. Было только CurID
            ViewBag.InformerCount = _DataManager.ConR.GetAllPaysForMonth(CurEmployee.Id, DateTime.Now.Year, currmonth).Count();
            ViewBag.InformerSum = _DataManager.ConR.GetAllPaysForMonth(CurEmployee.Id).Where(x => x.Date_of_event.Value.Month == currmonth).Select(x => x.Received).Sum();

            ViewBag.SalarySum = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == CurEmployee.Id && x.Date.Month == currmonth).Select(x => x.Receipt).Sum();

            ViewBag.FactSalarySum = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == CurEmployee.Id && x.Date.Month == currmonth).Select(x => x.Receipt).Sum() -
               _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == CurEmployee.Id && x.StatusForSalary != true && x.Date.Month == currmonth).Select(y => y.Receipt).Sum();

            //ViewBag.MyJob = _DataManager.PayR.GetEmplJobs(CurEmployee.Id).Except(_DataManager.PayR.GetCollection().Where(x=>x.Employee.Id==CurEmployee.Id));
            ViewBag.MyJob = _DataManager.PayR.GetCollection(true).Where(x => x.EmployeeTo.Id == CurEmployee.Id && x.Employee.Id != CurEmployee.Id && x.Date.Month == currmonth);
            if (CurEmployee.IsAdmin) ViewBag.Admin = true;

            ViewBag.SalarySumFinal = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == CurEmployee.Id && x.StatusForSalary == true && x.Date.Month == currmonth).Select(x => x.Receipt).Sum();

            ViewBag.MySalary = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == CurEmployee.Id && x.Date.Month == currmonth);

            AdminEmployee = _DataManager.EmR.GetElem(adminId);

            if (lookfromAdmin) { ViewBag.EmplNameForAdmin = "просматривает работника " + CurEmployee.Name; ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name; }

            int sum = 0;
            int FactSum = 0;
            foreach (Payments cw in (IEnumerable<Payments>)ViewBag.MySalary)
            {
              
                if (cw.Employee.Id != cw.EmployeeTo.Id)
                { sum += cw.Receipt;
                     FactSum += cw.Receipt;
                }
                else if (cw.Employee.Salary.Where(x => x.Event.Id == cw.Event.Id).Select(y => y.PercentOfSalary).First().Contains("%"))
                { sum += cw.Event.Salary.Select(x => cw.Receipt * x.Value / 100).FirstOrDefault();
                    if (cw.StatusForSalary == false) FactSum += cw.Event.Salary.Select(x => cw.Receipt * x.Value / 100).FirstOrDefault();
                }
                else if (cw.Employee.Salary.Where(x => x.Event.Id == cw.Event.Id).Select(y => y.PercentOfSalary).First().Contains("значение"))
                {
                    sum += cw.Event.Salary.Select(x => cw.Receipt * x.Value / 100).FirstOrDefault();
                    if (cw.StatusForSalary == false) FactSum += cw.Event.Salary.Select(x => x.Value).FirstOrDefault();

                }

                //другие обработчики
                else sum += 0;


                 

            }

            ViewBag.TotalSumMinusSal = sum;

            ViewBag.TotalFactSumMinusSal = sum-FactSum;
              //  sum - (-1) * (_DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == CurEmployee.Id && x.Date.Month == currmonth).Select(x => x.Receipt).Sum() - _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == CurEmployee.Id && x.Date.Month == currmonth).Select(x => x.Receipt).Sum() -
              // _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == CurEmployee.Id && x.StatusForSalary != true && x.Date.Month == currmonth).Select(y => y.Receipt).Sum());
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);

            var SalPerMon = new SalaryPerMonth();
            SalPerMon.CurMonthSal = sum;
            SalPerMon.CurMonthSalFact = sum- FactSum ;
            SalPerMon.Employee = CurEmployee;
            SalPerMon.DateMonth = (short)currmonth ;
            SalPerMon.DateYear = DateTime.Now.Year;
            SalPerMon.IncomeToCompany = FactSum;

            if (_DataManager.SpmR.Contains(SalPerMon)) { _DataManager.SpmR.Edit(SalPerMon); }
            else 
            _DataManager.SpmR.Add(SalPerMon);
            return View();
        }
        [HttpPost]
        public ActionResult MyFinanses(int CurUserId = 0, int adminId = 0)
        {
            

            if (!string.IsNullOrEmpty(Request.Params.AllKeys.FirstOrDefault(key => key.StartsWith("MyPays")))) //добавление детализации
            {
                return RedirectToAction("AddPaymentDetail", new { CurUserId, adminId });
            }

            else if (!string.IsNullOrEmpty(Request.Params.AllKeys.FirstOrDefault(key => key.StartsWith("MyJob")))) //добавление детализации
            {
                return RedirectToAction("AddMyJobPayment", new { CurUserId, adminId });
            }
            else
            { return RedirectToAction("AddReceivedMoney", new { CurUserId, adminId }); }
        }
        #endregion



        #region Добавление_платежа_в_1_табл
        [HttpGet]
        public ActionResult AddPaymentDetail(string error = "", int CurUserId = 0, int adminId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);


            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.adminId = adminId;

            ViewBag.DateOfSign = String.Join("-", ((DateTime.Now).ToShortDateString()).Split('.').Reverse());
            ViewBag.CurUserId = CurEmployee.Id;
            AdminEmployee = _DataManager.EmR.GetElem(adminId);

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Events = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type");
            ViewBag.Accounts = new SelectList(_DataManager.AccR.GetCollection(), "Id", "Type");
            ViewBag.Contracts = new SelectList(_DataManager.ConR.GetCollection(), "Id", "Name");
            ViewBag.Employees = new SelectList(_DataManager.EmR.GetCollection(), "Id", "FIO");
            if (CurEmployee.IsAdmin) ViewBag.Admin = true;
            if (lookfromAdmin) { ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name; ViewBag.EmplNameForAdmin = "просматривает работника " + CurEmployee.Name; }

                return View();
        }
        [HttpPost]
        public ActionResult AddPaymentDetail(int CurUserId = 0, int adminId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

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
            return RedirectToAction("MyFinanses", new { CurUserId, adminId });
        }
        #endregion

        #region Добавление_платежа_в_табл_2
        /// <summary>
        /// добавление в таблицу СДАНО
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddReceivedMoney(string error = "", int CurUserId = 0, int adminId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            if (CurEmployee.IsAdmin) ViewBag.Admin = true;
            ViewBag.CurUserId = CurEmployee.Id;
            ViewBag.adminId = adminId;

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Account = new SelectList(_DataManager.AccR.GetCollection(), "Id", "Type");
            ViewBag.DateOfSign = String.Join("-", ((DateTime.Now).ToShortDateString()).Split('.').Reverse()); ViewBag.DateMonth1 = MothsForChoose;
            AdminEmployee = _DataManager.EmR.GetElem(adminId);

            if (lookfromAdmin) { ViewBag.EmplNameForAdmin = "просматривает работника " + CurEmployee.Name; ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name; }

            return View();
        }
        [HttpPost]
        public ActionResult AddReceivedMoney(int CurUserId = 0, int adminId = 0)

        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            Pay_min Adder = new Pay_min();
            Adder.Sum = Convert.ToInt32(Request.Form["Sum"]);
            Adder.Account = _DataManager.AccR.GetElem(Convert.ToInt32(Request.Form["Account"]));
            Adder.Employee = _DataManager.EmR.GetElem(CurEmployee.Id);
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));
            Adder.Finished = false;


            _DataManager.Pay_minR.Add(Adder);
            return RedirectToAction("MyFinanses", new { CurUserId, adminId });
        }
        #endregion

        #region Изменение_контракта
        [HttpGet]
        public ActionResult EditContract(int id = 0, bool IsAdmin = false, string error = "", int CurUserId = 0, int adminId = 0)
        {

            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Error = error; if (CurEmployee.IsAdmin) ViewBag.Admin = true;
            ViewBag.CurUserId = CurEmployee.Id;
            ViewBag.adminId = adminId;

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Cost = _DataManager.ConR.GetElem(id).Sum_only_contract;
            ViewData["Events"] = new SelectList(_DataManager.EvR.GetCollection("MyFirst"), "Id", "Type", _DataManager.ConR.GetElem(id).EventSet.Id);
            ViewBag.Client = _DataManager.ConR.GetElem(id).ClientSet.FIO;
            ViewBag.DateOfEvent = String.Join("-", (((DateTime)_DataManager.ConR.GetElem(id).Date_of_event).ToShortDateString()).Split('.').Reverse());
            ViewBag.DateOfSign = String.Join("-", (((DateTime)_DataManager.ConR.GetElem(id).Date_of_sign).ToShortDateString()).Split('.').Reverse());
            ViewBag.Comment = _DataManager.ConR.GetElem(id).Comment;
            try { ViewBag.DateOfPay1 = String.Join("-", (((DateTime)_DataManager.ConR.GetElem(id).Payment1Date).ToShortDateString()).Split('.').Reverse()); }
            catch { };
            try { ViewBag.DateOfPay2 = String.Join("-", (((DateTime)_DataManager.ConR.GetElem(id).Payment2Date).ToShortDateString()).Split('.').Reverse()); }
            catch { };
            try { ViewBag.DateOfPay3 = String.Join("-", (((DateTime)_DataManager.ConR.GetElem(id).Payment3Date).ToShortDateString()).Split('.').Reverse()); }
            catch { };
            ViewBag.PaySum1 = _DataManager.ConR.GetElem(id).Payment1Sum ?? null;
            ViewBag.PaySum2 = _DataManager.ConR.GetElem(id).Payment2Sum ?? null;
            ViewBag.PaySum3 = _DataManager.ConR.GetElem(id).Payment3Sum ?? null;
            ViewBag.ID = id;
            if (IsAdmin) ViewBag.IsAdmin = true;
            AdminEmployee = _DataManager.EmR.GetElem(adminId);

            if (lookfromAdmin) { ViewBag.EmplNameForAdmin = "просматривает работника " + CurEmployee.Name; ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name; }

            return View();
        }

        [HttpPost]
        public ActionResult EditContract(int CurUserId = 0, int adminId = 0)
        {
            try
            {
                CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

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

            catch (Exception) { return RedirectToAction("EditContract", new { error = "Не все поля заполнены", CurUserId, adminId }); }

            return RedirectToAction("Index", new { idUser = CurUserId , adminId });
        }
        #endregion


        public ActionResult Exit(bool exitfromadmin = false, int CurUserId = 0, int adminId = 0)
            {
            try
            {
                CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
                AdminEmployee = _DataManager.EmR.GetElem(adminId);
                ViewBag.adminId = adminId;

                if (exitfromadmin)
                {
                    CurEmployee = null;

                    return RedirectToAction("Log_in", "User");
                }
                else
            if (lookfromAdmin) { return RedirectToAction("Index", "Admin", new { id=adminId });   }
                else
                {
                    CurEmployee = null;

                    return RedirectToAction("Log_in", "User");
                }
            }
            catch (Exception e)
            {
                CurEmployee = null;
                return RedirectToAction("Log_in", "User");

            }
            finally { lookfromAdmin = false; }
        }
        [HttpGet]
        public ActionResult EditPaymentDetail(int id = 0, string error = "", int CurUserId = 0, int adminId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Receipt = _DataManager.PayR.GetElem(id).Receipt;
            ViewBag.Comment = _DataManager.PayR.GetElem(id).Comment;
            if (CurEmployee.IsAdmin) ViewBag.Admin = true;
            ViewBag.CurUserId = CurEmployee.Id;
            ViewBag.adminId = adminId;
            
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Events = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type", _DataManager.PayR.GetElem(id).Event.Id);
            //_DataManager.PayR.GetElem(id).Account ? _DataManager.PayR.GetElem(id).Account.Id : 0
            ViewBag.Accounts = new SelectList(_DataManager.AccR.GetCollection(), "Id", "Type");
            int SelectedId = _DataManager.PayR.GetElem(id).Contract == null ? 0 : _DataManager.PayR.GetElem(id).Contract.Id;
          
            ViewBag.Contracts = new SelectList(_DataManager.ConR.GetCollection(), "Id", "Name", SelectedId);
            ViewBag.Employees = new SelectList(_DataManager.EmR.GetCollection(), "Id", "FIO", _DataManager.EmR.GetElem(CurEmployee.Id));
            ViewBag.Date = String.Join("-", (((DateTime)_DataManager.PayR.GetElem(id).Date).ToShortDateString()).Split('.').Reverse());

            ViewBag.DatePays = MothsForChoose;
            //new SelectList(new List<SelectListItem>
            //{
            //    new SelectListItem{Text = "Январь", Value = "1"},
            //    new SelectListItem{Text = "Февраль", Value = "2"},
            //    new SelectListItem{Text = "Март", Value = "3"},
            //    new SelectListItem{Text = "Апрель", Value = "4"},
            //    new SelectListItem{Text = "Май", Value = "5"},
            //    new SelectListItem{Text = "Июнь", Value = "6"},
            //    new SelectListItem{Text = "Июль", Value = "7"},
            //    new SelectListItem{Text = "Август", Value = "8"},
            //    new SelectListItem{Text = "Сентябрь", Value = "9"},
            //    new SelectListItem{Text = "Октябрь", Value = "10"},
            //    new SelectListItem{Text = "Ноябрь", Value = "11"},
            //    new SelectListItem{Text = "Декабрь", Value = "12"}
            //},"Value", "Text", DateTime.Now.Month);
            AdminEmployee = _DataManager.EmR.GetElem(adminId);

            ViewBag.Id = id; if (lookfromAdmin)
            {
                ViewBag.EmplNameForAdmin = "просматривает работника " + CurEmployee.Name; ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name;
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditPaymentDetail(int id = 0, int CurUserId = 0, int adminId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            Payments Adder = _DataManager.PayR.GetElem(id);

            Adder.Receipt = Convert.ToInt32(Request.Form["Receipt"]);
            Adder.Account = _DataManager.AccR.GetElem(Convert.ToInt32(Request.Form["Account"]));
            Adder.Contract = _DataManager.ConR.GetElem(Convert.ToInt32(Request.Form["Contract"]));
            Adder.Event = _DataManager.EvR.GetElem(Convert.ToInt32(Request.Form["Event"]));
            Adder.EmployeeTo = _DataManager.EmR.GetElem(Convert.ToInt32(Request.Form["Employee"]));
            Adder.Employee = _DataManager.EmR.GetElem(CurEmployee.Id);
            Adder.Comment = Request.Form["Comment"];
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));
            Adder.DateForPayment = Convert.ToInt32(Request.Form["DatePay"]) >= DateTime.Now.Month ? new DateTime(DateTime.Today.Year, Convert.ToInt32(Request.Form["DatePay"]), Adder.Date.Day) : new DateTime(DateTime.Today.Year, Convert.ToInt32(Request.Form["DatePay"]), Adder.Date.Day).AddYears(1);
            _DataManager.PayR.EditPaymentDetail(Adder);
            return RedirectToAction("MyFinanses", new { CurUserId , adminId });
        }

        [HttpGet]
        public ActionResult EditPay_min(int id = 0, string error = "", int CurUserId = 0,int adminId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.Sum = _DataManager.Pay_minR.GetElem(id).Sum;
            if (CurEmployee.IsAdmin) ViewBag.Admin = true;
            ViewBag.CurUserId = CurEmployee.Id;
            ViewBag.adminId = adminId;
            ViewBag.Id = id;
            ViewBag.Accounts = new SelectList(_DataManager.AccR.GetCollection(), "Id", "Type", _DataManager.Pay_minR.GetElem(id).Account.Id);
            AdminEmployee = _DataManager.EmR.GetElem(adminId);

            ViewBag.Date = String.Join("-", (((DateTime)_DataManager.Pay_minR.GetElem(id).Date).ToShortDateString()).Split('.').Reverse());
            if (lookfromAdmin) { ViewBag.EmplNameForAdmin = "просматривает работника " + CurEmployee.Name; ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name; }

            return View();
        }
        [HttpPost]
        public ActionResult EditPay_min(int id = 0, int CurUserId = 0, int adminId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            Pay_min Adder = _DataManager.Pay_minR.GetElem(id);
            Adder.Sum = Convert.ToInt32(Request.Form["Sum"]);
            Adder.Account = _DataManager.AccR.GetElem(Convert.ToInt32(Request.Form["Account"]));
            Adder.Employee = _DataManager.EmR.GetElem(CurEmployee.Id);
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));
            Adder.Finished = false;


            _DataManager.Pay_minR.Edit_PayMin(Adder);
            return RedirectToAction("MyFinanses", new { CurUserId, adminId });
        }
        [HttpGet]
        public ActionResult AddMyJobPayment(string error = "", int CurUserId = 0, int adminId=0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            if (CurEmployee.IsAdmin) ViewBag.Admin = true;
            ViewBag.CurUserId = CurEmployee.Id;
            AdminEmployee = _DataManager.EmR.GetElem(adminId);
            ViewBag.adminId = adminId;

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Contract = new SelectList(_DataManager.ConR.GetCollection(), "Id", "Name");
            ViewBag.Event = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type");
            ViewBag.Employees = new SelectList(_DataManager.EmR.GetCollection(CurEmployee.Id), "Id", "FIO");
            ViewBag.Date = String.Join("-", ((DateTime.Now).ToShortDateString()).Split('.').Reverse());
            if (lookfromAdmin) { ViewBag.EmplNameForAdmin = "просматривает работника " + CurEmployee.Name; ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name; }

            return View();
        }
        [HttpPost]
        public ActionResult AddMyJobPayment(int CurUserId = 0, int adminId = 0)

        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);


            Payments Adder = new Payments();

            Adder.Receipt = Convert.ToInt32(Request.Form["Sum"]);
            Adder.Contract = _DataManager.ConR.GetElem(Convert.ToInt32(Request.Form["Contract"]));
            Adder.Event = _DataManager.EvR.GetElem(Convert.ToInt32(Request.Form["Event"]));
            Adder.Employee = _DataManager.EmR.GetElem(Convert.ToInt32(Request.Form["Employee"]));
            Adder.EmployeeTo = _DataManager.EmR.GetElem(CurEmployee.Id);
            Adder.Comment = Request.Form["Comment"];
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));
            Adder.Account = _DataManager.AccR.GetCollection().Where(x => x.Type.Contains("указано")).FirstOrDefault();
            _DataManager.PayR.Add(Adder);
            return RedirectToAction("MyFinanses", new { CurUserId, adminId });
        }

        public ActionResult DeletePaymentDetail(int id, int CurUserId = 0, int adminId = 0)
        {
            _DataManager.PayR.Delete(id);
            return RedirectToAction("MyFinanses", new { CurUserId, adminId });

        }
        public ActionResult DeletePay_minDetail(int id, int CurUserId = 0, int adminId = 0)
        {
            _DataManager.Pay_minR.Delete(id);
            return RedirectToAction("MyFinanses", new { CurUserId, adminId });

        }
        public ActionResult DeletePaymentForPeerDetail(int id, int CurUserId = 0, int adminId = 0)
        {
            _DataManager.PayR.Delete(id);
            return RedirectToAction("MyFinanses", new { CurUserId, adminId });

        }
        [HttpGet]
        public ActionResult EditPayFromPeer(int id = 0, string error = "", int CurUserId = 0, int adminId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            if (CurEmployee == null) return RedirectToAction("Log_in", "User");
            ViewBag.CurUserId = CurEmployee.Id;
            ViewBag.adminId = adminId;
            ViewBag.Id = id;
            ViewBag.Sum = _DataManager.PayR.GetElem(id).Receipt;
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Contract = new SelectList(_DataManager.ConR.GetCollection(), "Id", "Name", _DataManager.PayR.GetElem(id).Contract.Id);
            ViewBag.Event = new SelectList(_DataManager.EvR.GetCollection(), "Id", "Type", _DataManager.PayR.GetElem(id).Event.Id);
            ViewBag.Employees = new SelectList(_DataManager.EmR.GetCollection(CurEmployee.Id), "Id", "FIO", _DataManager.PayR.GetElem(id).Employee.Id);
            ViewBag.Date = String.Join("-", ((_DataManager.PayR.GetElem(id).Date).ToShortDateString()).Split('.').Reverse());
            ViewBag.Comment = _DataManager.PayR.GetElem(id).Comment;
            AdminEmployee = _DataManager.EmR.GetElem(adminId);

            if (CurEmployee.IsAdmin) ViewBag.Admin = true; if (lookfromAdmin)
            {
                ViewBag.User = AdminEmployee.Surname + " " + AdminEmployee.Name; ViewBag.EmplNameForAdmin = "просматривает работника " + CurEmployee.Name;
            }

                return View();
        }
        [HttpPost]
        public ActionResult EditPayFromPeer(int id = 0, int CurUserId = 0, int adminId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            Payments Adder = _DataManager.PayR.GetElem(id);

            Adder.Receipt = Convert.ToInt32(Request.Form["Sum"]);
            Adder.Account = _DataManager.AccR.GetElem(Convert.ToInt32(Request.Form["Account"]));
            Adder.Contract = _DataManager.ConR.GetElem(Convert.ToInt32(Request.Form["Contract"]));
            Adder.Event = _DataManager.EvR.GetElem(Convert.ToInt32(Request.Form["Event"]));
            Adder.Employee = _DataManager.EmR.GetElem(Convert.ToInt32(Request.Form["Employee"]));
            Adder.EmployeeTo = _DataManager.EmR.GetElem(CurEmployee.Id);
            Adder.Comment = Request.Form["Comment"];
            Adder.Date = new DateTime(Convert.ToInt32(Request.Form["calendarPay"].Split('-')[0]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[1]), Convert.ToInt32(Request.Form["calendarPay"].Split('-')[2]));

            _DataManager.PayR.EditPaymentDetail(Adder);
            return RedirectToAction("MyFinanses", new { CurUserId, adminId });
        }
        public ActionResult ChangeToAdmin(int CurUserId = 0, int adminId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            return RedirectToAction("CompanyFinanses", "Admin", new { CurUserId });
        }
    }

}