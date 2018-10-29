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
        public ActionResult Index(int id = 0)
        {
            CurEmployee = id == 0 ? CurEmployee : _DataManager.EmR.GetElem(id);

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Contracts = _DataManager.ConR.GetCollection();
            ViewBag.ID = CurEmployee.Id;
            return View();
        }

        [HttpGet]
        public ActionResult CompanyFinanses()
        {

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
        public ActionResult Employees(string error = "")
        {
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.ID = CurEmployee.Id;
            ViewBag.Employees = _DataManager.EmR.GetCollection();

            return View();
        }

        [HttpPost]
        public ActionResult Employees()
        {
             
             
             

            return RedirectToAction("AddEmployee");
        }
        [HttpGet]
        public ActionResult ShowEmployee(int id = 0)
        {
            if (id != CurEmployee.Id)
                return RedirectToAction("Index", "Home", new { idUser = id, isadmin = true, adminId = CurEmployee.Id });
            else return RedirectToAction("EMployees");
        }
        [HttpGet]
        public ActionResult ChangeUser(int id = 0)

        {
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
        public ActionResult ChangeUser()

        {

            Employee tmp = _DataManager.EmR.GetElem(Convert.ToInt32(Request.Form["IdValue"]));


            tmp.Name = Request.Form["Name"];
            tmp.Surname = Request.Form["Sname"];
            tmp.Patronymic = Request.Form["Pname"];

            tmp.Login = Request.Form["Login"];
            tmp.Password = Request.Form["Password"];
            var a = Request.Form["IsAdmin"];
            tmp.IsAdmin = Request.Form["IsAdmin"].Contains("true") ? true : false;
            _DataManager.EmR.Edit(tmp);
            return RedirectToAction("Employees");
        }
        public ActionResult ChangeToUser()

        {

            return RedirectToAction("Index", "Home", new { idUser = CurEmployee.Id });
        }
        public ActionResult Exit()
        {
            return RedirectToAction("Exit", "Home", new { exitfromadmin = true });
        }
        [HttpGet]
        public ActionResult Events()
        {
            ViewBag.Events = _DataManager.EvR.GetCollection();
            ViewBag.Types = new SelectList(_DataManager.SalR.GetCollectionTypes(), "Id", "Type");
            return View();

        }
       
        [HttpPost]
        public ActionResult Events(string error="")
        {
            var a = Request.Form["Types"]; //выводит по порядку

            int counter = 0;
            foreach(Event cw in _DataManager.EvR.GetCollection())
            {
                
                cw.Percent = _DataManager.SalR.GetElemType(Convert.ToInt32(a.Split(',')[counter++])).Type ;

            }
            _DataManager.shop_cont.SaveChanges();
            return RedirectToAction("Salary") ;

        }
        [HttpGet]
        public ActionResult Salary()
        {
            ViewBag.Employees = _DataManager.EmR.GetCollection();
            ViewBag.Events = _DataManager.EvR.GetCollection();
            ViewBag.Types = _DataManager.EvR.GetCollection().Select(x => x.Percent);
            return View();

        }
        [HttpPost]
        public ActionResult Salary(string error = "")
        {

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
            return RedirectToAction("CompanyFinanses");

        }
        [HttpGet]
        public ActionResult AddEmployee(int id = 0)

        {
             


            return View();
        }
        [HttpPost]
        public ActionResult AddEmployee()

        {

            Employee tmp = new Employee();


            tmp.Name = Request.Form["Name"];
            tmp.Surname = Request.Form["Sname"];
            tmp.Patronymic = Request.Form["Pname"];

            tmp.Login = Request.Form["Login"];
            tmp.Password = Request.Form["Password"];
            var a = Request.Form["IsAdmin"];
            tmp.IsAdmin = Request.Form["IsAdmin"].Contains("true") ? true : false;
            _DataManager.EmR.Add(tmp);
            return RedirectToAction("Employees");
        }
        
    }
    
}