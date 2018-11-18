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
        public AdminController(DataManager _DM)

        {

            _DataManager = _DM;
        }
        [HttpGet]
        public ActionResult Index(int id = 0)
        {
            CurEmployee = id == 0 ? CurEmployee : _DataManager.EmR.GetElem(id);

            ViewBag.CurUserId = id;
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Contracts = _DataManager.ConR.GetCollection();
            ViewBag.ID = CurEmployee.Id;
            return View();
        }

        [HttpGet]
        public ActionResult CompanyFinanses(int CurUserId)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.TotalPlan = _DataManager.ConR.GetCollection().Select(x => x.Received).Sum(); //!!!

 
            //  ViewBag.Years = new SelectList(_DataManager.PayR.GetCollection(), "Id", "FIO");
            var tmplist = _DataManager.EmR.GetCollection();
            int[] minsalary = new int[tmplist.Count()];
            int[] sal = new int[tmplist.Count()];

 
            int counter = 0;

            foreach (Employee c in tmplist)
            {
                ViewBag.MySalary = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id);
                int sum = 0;
                foreach (Payments cw in (IEnumerable<Payments>)ViewBag.MySalary)
                {
                    if (@cw.Employee.Id != cw.EmployeeTo.Id)
                    { sum += cw.Receipt; }
                    else sum += cw.Event.Salary.Select(x => cw.Receipt - x.Value).FirstOrDefault();
                    ViewBag.TotalSumMinusSal = sum;
                }
                c.Scale = sum; //туть
                //    _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id).Select(x => x.Receipt).Sum();
                //minsalary[counter] = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id && x.StatusForSalary != true).Select(y => y.Receipt).Sum();
                //counter++;
            }

            ViewBag.Employees = tmplist;


            foreach (Employee c in _DataManager.EmR.GetCollection())
            {
                minsalary[counter] = c.Scale - (-1) * (_DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id).Select(x => x.Receipt).Sum() - _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id).Select(x => x.Receipt).Sum() -
               _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id && x.StatusForSalary != true).Select(y => y.Receipt).Sum());
               sal[counter] = (_DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id).Select(x => x.Receipt).Sum() -
               _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id && x.StatusForSalary != true).Select(y => y.Receipt).Sum())- minsalary[counter];
                counter++;
            }
            ViewBag.SalaryFact = minsalary;
            //    ViewBag.EmployeesSalary =
            //      _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == /*CurEmployee.Id*/ && x.StatusForSalary != true).Select(y => y.Receipt);
            ViewBag.ID = CurEmployee.Id;

            //for (int i = 0; i < sal.Length; i++)
            //{
            //    sal[i] = sal[i] - minsalary[i];
            //}

            ViewBag.EmployeesSalaryFact = sal;
            ViewBag.Id = CurEmployee.Id;
            ViewBag.ToCompany = _DataManager.SalR.GetCollection().Where(x => x.Employee.Id == CurEmployee.Id).Select(y => y.Value).Sum();

            return View();
        }
        [HttpGet]
        public ActionResult Employees(string error = "", int CurUserId=0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.ID = CurEmployee.Id;
            ViewBag.Employees = _DataManager.EmR.GetCollection();
            ViewBag.CurUserId = CurUserId;
            return View();
        }

        [HttpPost]
        public ActionResult Employees(int CurUserId=0)
        {
             
             
             

            return RedirectToAction("AddEmployee", new { CurUserId });
        }
        [HttpGet]
        public ActionResult ShowEmployee(int id = 0, int CurUserId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);

            ViewBag.CurUserId = id;
            //CurEmployee = id == 0 ? CurEmployee : _DataManager.EmR.GetElem(id);

            if (id != CurEmployee.Id)
                return RedirectToAction("Index", "Home", new { idUser = id, isadmin = true, adminId = CurEmployee.Id });
            else return RedirectToAction("Employees", new {CurUserId });
        }
        [HttpGet]
        public ActionResult ChangeUser(int id = 0, int CurUserId = 0)

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
        public ActionResult Events(int CurUserId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;
            ViewBag.Events = _DataManager.EvR.GetCollection();
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
        public ActionResult Salary(int CurUserId = 0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;
            ViewBag.Employees = _DataManager.EmR.GetCollection();
            ViewBag.Events = _DataManager.EvR.GetCollection();
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
        public ActionResult EditSalaryItem(int Emid=0, int Evid=0, int CurUserId=0, int adminId=0)
        {
            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurEmployee.Id;
            ViewBag.EmployeeId = _DataManager.EmR.GetElem(Emid).Id;
            ViewBag.EventId = _DataManager.EvR.GetElem(Evid).Id;

            ViewBag.Employee = _DataManager.EmR.GetElem(Emid);
            ViewBag.Event = _DataManager.EvR.GetElem(Evid);
            var text = (ViewBag.Employee as Employee).Salary.Where(x => x.Event.Id == (ViewBag.Event as Event).Id).Select(y => y.PercentOfSalary).FirstOrDefault();
            var selectedIndex = _DataManager.SalR.GetCollectionTypes().Where(x => x.Type == text).FirstOrDefault().Id;
            ViewBag.Types = new SelectList(_DataManager.SalR.GetCollectionTypes(), "Id", "Type", _DataManager.SalR.GetCollectionTypes().Last().Id);
            ViewBag.Salary = (ViewBag.Employee as Employee).Salary.Where(x => x.Event.Id == (ViewBag.Event as Event).Id).Select(y => y.Value).FirstOrDefault();
          /*  ViewBag.Salary = (ViewBag.Employee as Employee).Salary.Where(x => x.Event.Id == (ViewBag.Event as Event).Id).Select(y => y.ValueOlga).FirstOrDefault();
            ViewBag.Salary = (ViewBag.Employee as Employee).Salary.Where(x => x.Event.Id == (ViewBag.Event as Event).Id).Select(y => y.ValueSergey).FirstOrDefault();

    */

            return View();
        }
        [HttpPost]
        public ActionResult EditSalaryItem( int CurUserId = 0, int EventId=0, int EmployeeId=0)
        {
          


            var salaryItem = _DataManager.SalR.GetCollection().Where(x => x.Event.Id == EventId&&x.Employee.Id==EmployeeId).FirstOrDefault();

            salaryItem.PercentOfSalary = _DataManager.SalR.GetCollectionTypes().Select(x=>x.Type).ToArray<string>()[Convert.ToInt32(Request.Form["Types"])-1];
            salaryItem.Value =Convert.ToInt32( Request.Form["Salary"]);

            _DataManager.SalR.Edit(salaryItem);
            return RedirectToAction("Salary", new { CurUserId});
        }
        [HttpGet]
        public ActionResult AddEmployee(int id = 0, int CurUserId = 0)

        {

            CurEmployee = CurUserId == 0 ? CurEmployee : _DataManager.EmR.GetElem(CurUserId);
            ViewBag.CurUserId = CurUserId;

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
        
    }
    
}