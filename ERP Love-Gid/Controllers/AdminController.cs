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
        public ActionResult Index(int id=0)
        {
            CurEmployee = id == 0 ? CurEmployee : _DataManager.EmR.GetElem(id);

             ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.Contracts = _DataManager.ConR.GetCollection();
            ViewBag.ID = CurEmployee.Id;
            return View();
        }

        [HttpGet]
        public ActionResult CompanyFinanses(int id=0)
        {

            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.TotalPlan = _DataManager.ConR.GetCollection().Select(x => x.Received).Sum(); //!!!

            //  ViewBag.Years = new SelectList(_DataManager.PayR.GetCollection(), "Id", "FIO");
            var tmplist = _DataManager.EmR.GetCollection();
            int[] minsalary = new int[tmplist.Count()];
            int counter = 0;
            foreach (Employee c in tmplist) { c.Scale = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id).Select(x => x.Receipt).Sum();
                minsalary[counter] = _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == c.Id && x.StatusForSalary != true).Select(y => y.Receipt).Sum();
                counter++;   }
            ViewBag.Employees = tmplist;
        //    ViewBag.EmployeesSalary =
          //      _DataManager.PayR.GetCollection().Where(x => x.EmployeeTo.Id == /*CurEmployee.Id*/ && x.StatusForSalary != true).Select(y => y.Receipt);
            var sal = tmplist.Select(x => x.Scale).ToArray<int>();
            ViewBag.ID = CurEmployee.Id;

            for (int i = 0; i < sal.Length; i++)
            {
                sal[i] = sal[i]- minsalary[i];
            }

            ViewBag.EmployeesSalaryFact = sal;
             return View();
        }
        [HttpGet]
        public ActionResult Employees()
        {
            ViewBag.User = CurEmployee.Surname + " " + CurEmployee.Name;
            ViewBag.ID = CurEmployee.Id;
            ViewBag.Employees = _DataManager.EmR.GetCollection();

            return View();
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
            return RedirectToAction("Exit", "Home", new { exitfromadmin =true});
        }
        [HttpGet]
        public ActionResult Events()
        {
            ViewBag.Events = _DataManager.EvR.GetCollection();

            return View();
        
    }
}