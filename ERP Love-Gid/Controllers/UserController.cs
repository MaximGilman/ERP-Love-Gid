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
            Init();
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
        public void Init()
        {
           if( _DataManager.EvR.GetCollection().Count()==0)
            {
                var tmpEv = new Event();
                tmpEv.Type = "Администрирование";
                _DataManager.EvR.Add(tmpEv);
                tmpEv.Type = "Договор Прочее";
                _DataManager.EvR.Add(tmpEv);
                tmpEv.Type = "Дизайн";
                _DataManager.EvR.Add(tmpEv);
                // и так далее
                _DataManager.shop_cont.SaveChanges();

            }
            if (_DataManager.AccR.GetCollection().Count() == 0)
            {
                var tmpAc = new Account();
                tmpAc.Type = "Рассчетный счет";
                _DataManager.AccR.Add(tmpAc);
                tmpAc.Type = "Сбербанк";
                _DataManager.AccR.Add(tmpAc);
                // и так далее
                _DataManager.shop_cont.SaveChanges();

            }
            if (_DataManager.STR.GetCollection().Count() == 0)
            {
                var tmpSalT = new SalaryTypes();
                tmpSalT.Type = "%";
                _DataManager.STR.Add(tmpSalT);
                tmpSalT.Type = "Оклад";
                _DataManager.STR.Add(tmpSalT);
                tmpSalT.Type = "Числовое значение";
                _DataManager.STR.Add(tmpSalT);
                tmpSalT.Type = "Указывается пользователем";
                _DataManager.STR.Add(tmpSalT);
                // и так далее
                _DataManager.shop_cont.SaveChanges();

            }
            if (_DataManager.ConR.GetCollection().Count() == 0)
            {
                var tmpContr = new Contract();
                tmpContr.Name = "Не указано";
                tmpContr.Date_of_event = DateTime.Now;
                tmpContr.Date_of_sign = DateTime.Now;
                _DataManager.shop_cont.SaveChanges();

            }
                if (_DataManager.EmR.GetCollection().Count() == 0)
            {
                var tmpEm = new Employee();
                tmpEm.Login = "0";
                tmpEm.Password = "0";
                tmpEm.Name = "UserName";
                tmpEm.Surname = "UserSurname";
                tmpEm.Patronymic = "UserPatronymic";
                tmpEm.IsAdmin = false;
                _DataManager.EmR.Add(tmpEm);

                var tmpAd = new Employee();
                tmpAd.Login = "1";
                tmpAd.Password = "1";
                tmpAd.Name = "AUserNameAdmin";
                tmpAd.Surname = "AUserSurnameAdmin";
                tmpAd.Patronymic = "AUserPatronymicAdmin";
                tmpAd.IsAdmin = true;
                _DataManager.EmR.Add(tmpAd);
                // и так далее
                _DataManager.shop_cont.SaveChanges();

            }
                if (_DataManager.SalR.GetCollection().Count()==0)
            {
                List<Salary> salaries = new List<Salary>();
                foreach(Employee empl in _DataManager.EmR.GetCollection())
                {
                    foreach (Event ev in _DataManager.EvR.GetCollection())
                    {
                        Salary salary = new Salary();
                        salary.Event = ev;
                        salary.Employee = empl;
                        salary.PercentOfSalary = "%";
                        salary.Value = 100;
                        salary.ValueOlga = 0;
                        salary.ValueSergey = 0;
                        salaries.Add(salary);

                    }

                }
                foreach (var salary in salaries)
                    _DataManager.SalR.Add(salary);
            }

        }
        public ActionResult Exit()
        {

            return RedirectToAction("Log_in", "User");
        }
    }
}