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
        private  Employee CurEmployee;
        int currmonth = DateTime.Now.Month;
        int currYear = DateTime.Now.Year;
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
        public AdminController(DataManager _DM)

        {

            _DataManager = _DM;
        }
        [HttpGet]
        public ActionResult Index(int numberofMonth = -1, int id = 0)
        {
            CurEmployee = id == 0 ? CurEmployee : _DataManager.EmR.GetElem(id);
            if (numberofMonth != -1) { currmonth = numberofMonth + 1;  }
            else { currmonth = DateTime.Now.Month; }
            ViewBag.CurUserId = id;
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Contracts = (IEnumerable<Contract>) _DataManager.ConR.GetCollection().Where(x=>x.Date_of_event.Value.Month== currmonth && x.Date_of_event.Value.Year==DateTime.Now.Year);
            ViewBag.ID = CurEmployee.Id;
            
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);
            return View();
        }

        [HttpGet]
        public ActionResult CompanyFinanses(int numberofMonth = -1, int CurUserId=0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;
            if (numberofMonth != -1) { currmonth = numberofMonth + 1; }
            else { currmonth = DateTime.Now.Month; }
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.TotalPlan =  _DataManager.ConR.GetCollection().Where(y=>y.Date_of_event.Value.Year==DateTime.Now.Year&&y.Date_of_event.Value.Month==currmonth).Select(x => x.Received).Sum(); //!!!

          
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);
            //  ViewBag.Years = new SelectList(_DataManager.PayR.GetCollection(), "Id", "FIO");
            var tmplist = _DataManager.EmR.GetCollection();
            //int[] minsalary = new int[tmplist.Count()];
            //int[] sal = new int[tmplist.Count()];


            //int counter = 0;
            List<SalaryPerMonth> salaryPerMonths = new List<SalaryPerMonth>();

            foreach (Employee c in tmplist)
            {
                ViewBag.MySalary = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id && x.Date.Month == currmonth);
                int sum = 0; int FactSum = 0; int total_sum = 0; int total_sum_fact = 0;
                foreach (Payments cw in (IEnumerable<Payments>)ViewBag.MySalary)
                {
                    total_sum += cw.Receipt;

                    if (cw.Employee.Id != cw.EmployeeTo.Id)
                    {
                        sum += cw.Receipt;
                        FactSum += cw.Receipt;
                    }
                    else if (cw.Employee.Salary.Where(x => x.Event.Id == cw.Event.Id).Select(y => y.PercentOfSalary).First().Contains("%"))
                    {
                        sum += cw.Event.Salary.Select(x => cw.Receipt * x.Value / 100).FirstOrDefault();
                        if (cw.StatusForSalary == false) FactSum += cw.Event.Salary.Select(x => cw.Receipt * x.Value / 100).FirstOrDefault();
                        else total_sum_fact += cw.Event.Salary.Select(x => cw.Receipt - (cw.Receipt * x.Value / 100)).FirstOrDefault();

                    }
                    else if (cw.Employee.Salary.Where(x => x.Event.Id == cw.Event.Id).Select(y => y.PercentOfSalary).First().Contains("значение"))
                    {
                        sum += cw.Event.Salary.Select(x => cw.Receipt * x.Value / 100).FirstOrDefault();
                        if (cw.StatusForSalary == false) FactSum += cw.Event.Salary.Select(x => x.Value).FirstOrDefault();
                        else total_sum_fact += cw.Event.Salary.Select(x => cw.Receipt - x.Value).FirstOrDefault();


                    }
                    else if (cw.Employee.Salary.Where(x => x.Event.Id == cw.Event.Id).Select(y => y.PercentOfSalary).First().Contains("Указывается"))
                    {
                        sum += cw.Receipt;
                        total_sum += cw.SumToOptyonallyUsing.Value ;
                        if (cw.StatusForSalary == false)
                        {
                            FactSum += cw.Receipt;

                        }
                        else total_sum_fact += cw.SumToOptyonallyUsing.Value;
                    }
                    //другие обработчики
                    else sum += 0;




                }


                var SalPerMon = new SalaryPerMonth();
                SalPerMon.CurMonthSal = sum;
                SalPerMon.CurMonthSalFact = sum - FactSum;
                SalPerMon.Employee = c;
                SalPerMon.DateMonth = (short)currmonth;
                SalPerMon.DateYear = DateTime.Now.Year;
                SalPerMon.IncomeToCompany = total_sum - sum; 

                SalPerMon.IncomeToCompanyFact = total_sum_fact;


                salaryPerMonths.Add(SalPerMon);

                c.Scale = sum; //туть
                //    _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id).Select(x => x.Receipt).Sum();
                //minsalary[counter] = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id && x.StatusForSalary != true).Select(y => y.Receipt).Sum();
                //counter++;
            }
            foreach (SalaryPerMonth salaryPerMon in salaryPerMonths)
            {
                if (_DataManager.SpmR.Contains(salaryPerMon)) { _DataManager.SpmR.Edit(salaryPerMon); }
                else
                    _DataManager.SpmR.Add(salaryPerMon);
            }
            ViewBag.CurMonth = currmonth;
            ViewBag.CurYear = DateTime.Now.Year;
            ViewBag.Employees = tmplist;
            ViewBag.SalaryPerMonth = _DataManager.SpmR.GetCollection();
 
          
            //    ViewBag.EmployeesSalary =
            //      _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == /*CurEmployee.Id*/ && x.StatusForSalary != true).Select(y => y.Receipt);
            ViewBag.ID = CurEmployee.Id;

            //for (int i = 0; i < sal.Length; i++)
            //{
            //    sal[i] = sal[i] - minsalary[i];
            //}

            //ViewBag.EmployeesSalaryFact = sal;
            ViewBag.Id = CurEmployee.Id;
            ViewBag.ToCompany = _DataManager.SalR.GetCollection().Where(x => x.Employee.Id == CurEmployee.Id).Select(y => y.Value).Sum();




            ViewBag.Sum= _DataManager.SpmR.GetSum(currmonth, currYear);
            ViewBag.SumFact= _DataManager.SpmR.GetFactSum(currmonth, currYear);

            return View();
        }
        [HttpGet]
        public ActionResult Employees(string error = "", int numberofMonth = -1, int CurUserId=0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.ID = CurEmployee.Id;
            ViewBag.Employees = _DataManager.EmR.GetCollection();
            ViewBag.CurUserId = CurUserId;
            if (numberofMonth != -1) { currmonth = numberofMonth + 1;   }
            else { currmonth = DateTime.Now.Month;   }
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);
            return View();
        }

        [HttpPost]
        public ActionResult Employees(int CurUserId=0)
        {
             
             
             

            return RedirectToAction("AddEmployee", new { CurUserId });
        }
        [HttpGet]
        public ActionResult ShowEmployee(int numberofMonth = -1, int id = 0, int CurUserId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            ViewBag.CurUserId = id;
            //CurEmployee = id == 0 ? CurEmployee : _DataManager.EmR.GetElem(id);
            if (numberofMonth != -1) { currmonth = numberofMonth + 1;   }
            else { currmonth = DateTime.Now.Month;   }
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);
            if (id != CurEmployee.Id)
                return RedirectToAction("Index", "Home", new { idUser = id, isadmin = true, adminId = CurEmployee.Id });
            else return RedirectToAction("Employees", new {CurUserId });
        }
        [HttpGet]
        public ActionResult ChangeUser(int numberofMonth = -1, int id = 0, int CurUserId = 0)

        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;

            ViewBag.Id = _DataManager.EmR.GetElem(id).Id;
            ViewBag.Name = _DataManager.EmR.GetElem(id).Name;
            ViewBag.Sname = _DataManager.EmR.GetElem(id).Surname;

            ViewBag.Pname = _DataManager.EmR.GetElem(id).Patronymic;
            ViewBag.Login = _DataManager.EmR.GetElem(id).Login;
            ViewBag.Password = _DataManager.EmR.GetElem(id).Password;
            ViewBag.IsAdmin = _DataManager.EmR.GetElem(id).IsAdmin;
            if (numberofMonth != -1) { currmonth = numberofMonth + 1;   }
            else { currmonth = DateTime.Now.Month;   }
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);

            return View();
        }
        [HttpPost]
        public ActionResult ChangeUser(int CurUserId = 0)

        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;
            Employee tmp = _DataManager.EmR.GetElem(Convert.ToInt32(Request.Form["IdValue"]));


            tmp.Name = Request.Form["Name"];
            tmp.Surname = Request.Form["Sname"];
            tmp.Patronymic = Request.Form["Pname"];

            tmp.Login = Request.Form["Login"];
            tmp.Password = Request.Form["Password"];
            var a = Request.Form["IsAdmin"];
            tmp.IsAdmin = Request.Form["IsAdmin"].Contains("true") ? true : false;
            _DataManager.EmR.Edit(tmp);
            return RedirectToAction("Employees", new { CurUserId });
        }
        public ActionResult ChangeToUser(int CurUserId)

        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;
 
            return RedirectToAction("Index", "Home", new { idUser = CurEmployee.Id,CurUserId });
        }
        public ActionResult Exit(int CurUserId =0)
        {
            return RedirectToAction("Exit", "Home", new { exitfromadmin = true, CurUserId });
        }
        [HttpGet]
        public ActionResult Events(int numberofMonth = -1, int CurUserId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;
            ViewBag.Events = _DataManager.EvR.GetCollection();
            if (numberofMonth != -1) { currmonth = numberofMonth + 1;   }
            else { currmonth = DateTime.Now.Month;   }
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);
            ViewBag.Types = new SelectList(_DataManager.SalR.GetCollectionTypes(), "Id", "Type");
            return View();

        }
       
        [HttpPost]
        public ActionResult Events(string error="", int CurUserId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;
            var a = Request.Form["Types"]; //выводит по порядку

            int counter = 0;
            foreach(Event cw in _DataManager.EvR.GetCollection())
            {
                
                cw.Percent = _DataManager.SalR.GetElemType(Convert.ToInt32(a.Split(',')[counter++])).Type ;

            }
            _DataManager.shop_cont.SaveChanges();
            return RedirectToAction("Salary", new { CurUserId }) ;

        }
        [HttpGet]
        public ActionResult Salary(int numberofMonth = -1, int CurUserId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;
            ViewBag.Employees = _DataManager.EmR.GetCollection();
            ViewBag.Events = _DataManager.EvR.GetCollection();
            if (numberofMonth != -1) { currmonth = numberofMonth + 1;   }
            else { currmonth = DateTime.Now.Month;   }
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);
            // ViewBag.Types = _DataManager.EvR.GetCollection().Select(x => x.Percent);
            ViewBag.Types = _DataManager.SalR.GetCollection();
             return View();

        }
        [HttpPost]
        public ActionResult Salary(string error = "", int CurUserId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;
            var a = Request.Form["SalaryValue"].Split(','); //по порядку вывода // фореачем до events.count();
            int index = 0;
            List<Salary> adder = new List<Salary>();
            foreach(Employee empl in _DataManager.EmR.GetCollection())
            {
                foreach( Event ev in _DataManager.EvR.GetCollection())
                {
                    Salary salary = new Salary();
                    salary.Employee = empl;
                    salary.Event = ev;
                    salary.PercentOfSalary = ev.Percent;
                    int nol;
                   
                    salary.Value = Int32.TryParse(a[index++], out nol)==false ? empl.Salary.Where(y => y.Employee.Id == empl.Id && y.Event.Id == ev.Id).Select(x => x.Value).FirstOrDefault() : nol;

                    adder.Add(salary);
                }

            }
            _DataManager.SalR.Add(adder);
            return RedirectToAction("CompanyFinanses", new { CurUserId });

        }
        
             [HttpGet]
        public ActionResult EditSalaryItem(int numberofMonth = -1, int Emid=0, int Evid=0, int CurUserId=0, int adminId=0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurEmployee.Id;
            ViewBag.EmployeeId = _DataManager.EmR.GetElem(Emid).Id;
            ViewBag.EventId = _DataManager.EvR.GetElem(Evid).Id;

            if (numberofMonth != -1) { currmonth = numberofMonth + 1;   }
            else { currmonth = DateTime.Now.Month;   }
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);
            ViewBag.Employee = _DataManager.EmR.GetElem(Emid);
            ViewBag.Event = _DataManager.EvR.GetElem(Evid);
            /* var text = (ViewBag.Employee as Employee).Salary.Where(x => x.Event.Id == (ViewBag.Event as Event).Id).Select(y => y.PercentOfSalary).FirstOrDefault();
             var selectedIndex = _DataManager.SalR.GetCollectionTypes().Where(x => x.Type == text).FirstOrDefault().Id;
             ViewBag.SelectedIndex = selectedIndex;
             var tyre = _DataManager.SalR.GetCollectionTypes().ToList();

           var bb = new SelectList(tyre, "Id", "Type", tyre[2]);


             bb.ElementAt(2).Selected = true;
             ViewBag.Types = bb;*/
            ViewBag.Types = new SelectList(_DataManager.SalR.GetCollectionTypes(), "Id", "Type");
            ViewBag.Salary = (ViewBag.Employee as Employee).Salary.Where(x => x.Event.Id == (ViewBag.Event as Event).Id).Select(y => y.Value).FirstOrDefault();
             ViewBag.SalaryOlga = (ViewBag.Employee as Employee).Salary.Where(x => x.Event.Id == (ViewBag.Event as Event).Id).Select(y => y.ValueOlga).FirstOrDefault();
              ViewBag.SalarySergey = (ViewBag.Employee as Employee).Salary.Where(x => x.Event.Id == (ViewBag.Event as Event).Id).Select(y => y.ValueSergey).FirstOrDefault();

      

            return View();
        }
        [HttpPost]
        public ActionResult EditSalaryItem( int CurUserId = 0, int EventId=0, int EmployeeId=0)
        {
          


            var salaryItem = _DataManager.SalR.GetCollection().Where(x => x.Event.Id == EventId&&x.Employee.Id==EmployeeId).FirstOrDefault()?? new Salary();

            salaryItem.PercentOfSalary = _DataManager.SalR.GetCollectionTypes().Select(x=>x.Type).ToArray<string>()[Convert.ToInt32(Request.Form["Types"])-1];
            try { salaryItem.Value = Convert.ToInt32(Request.Form["Salary"]); }
            catch { salaryItem.Value = 0; }
            try
            {
                salaryItem.ValueOlga = Convert.ToInt32(Request.Form["SalaryOlga"]);
            }
            catch { salaryItem.ValueOlga = 0; }
            try { 
            salaryItem.ValueSergey = Convert.ToInt32(Request.Form["SalarySergey"]);
            }
            catch { salaryItem.ValueSergey = 0; }


            if (_DataManager.SalR.GetCollection().Contains(salaryItem)) { _DataManager.SalR.Edit(salaryItem); }
            else {
                salaryItem.Employee = _DataManager.EmR.GetElem(EmployeeId);
                salaryItem.Event = _DataManager.EvR.GetElem(EventId);
                _DataManager.SalR.Add(salaryItem);
            }
            return RedirectToAction("Salary", new { CurUserId});
        }
        [HttpGet]
        public ActionResult AddEmployee(int numberofMonth = -1, int id = 0, int CurUserId = 0)

        {

            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;
            if (numberofMonth != -1) { currmonth = numberofMonth + 1;   }
            else { currmonth = DateTime.Now.Month;   }
            ViewBag.DateMonth1 = new SelectList(MothsForChoose, "Value", "Text", currmonth);
            return View();
        }
        [HttpPost]
        public ActionResult AddEmployee(int CurUserId = 0)

        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;

            Employee tmp = new Employee();


            tmp.Name = Request.Form["Name"];
            tmp.Surname = Request.Form["Sname"];
            tmp.Patronymic = Request.Form["Pname"];

            tmp.Login = Request.Form["Login"];
            tmp.Password = Request.Form["Password"];
            var a = Request.Form["IsAdmin"];
            tmp.IsAdmin = Request.Form["IsAdmin"].Contains("true") ? true : false;
            _DataManager.EmR.Add(tmp);
            return RedirectToAction("Employees", new { CurUserId });
        }
       /* [HttpGet]
        public void GetData(string salFact, string sal, int CurUserId)
        {
            if (currmonth == DateTime.Now.Month) { 
            Employee employee = _DataManager.EmR.GetElem(CurUserId);
            employee.CurMonthSal = sal == "" ? 0 : Convert.ToInt32(sal);
            employee.CurMonthSalFact = salFact == "" ? 0 : Convert.ToInt32(salFact);
            _DataManager.shop_cont.SaveChanges();
        }
        }*/
    }
    
}