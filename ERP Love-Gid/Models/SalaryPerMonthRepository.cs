using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class SalaryPerMonthRepository
    {
        private ERPModelContainer cont;


        public SalaryPerMonthRepository(ERPModelContainer _cont)
        {
            cont = _cont;
        }
        public SalaryPerMonth GetElem(int id)
        {
            return cont.SalaryPerMonthSet.Where(x => x.Id == id).FirstOrDefault();
        }
        
        internal void Add(SalaryPerMonth salPerMon)
        {
            cont.SalaryPerMonthSet.Add(salPerMon);
            cont.SaveChanges();
        }
        internal bool Contains(SalaryPerMonth salPerMon)
        {

            var a = cont.SalaryPerMonthSet.Where(x => x.DateMonth == salPerMon.DateMonth && x.DateYear == salPerMon.DateYear &&
            x.Employee.Id == salPerMon.Employee.Id).FirstOrDefault();

            return a!=null;

        }

        internal void Edit(SalaryPerMonth salPerMon)
        {
            var tmp = cont.SalaryPerMonthSet.Where(x => x.DateMonth==salPerMon.DateMonth&&x.DateYear==salPerMon.DateYear &&
            x.Employee.Id==salPerMon.Employee.Id ).FirstOrDefault();

            tmp.DateYear= salPerMon.DateYear;
            tmp.DateMonth = salPerMon.DateMonth;
            tmp.CurMonthSal = salPerMon.CurMonthSal;
            tmp.CurMonthSalFact = salPerMon.CurMonthSalFact;
            tmp.Employee = salPerMon.Employee;
            tmp.IncomeToCompany = salPerMon.IncomeToCompany;
            
            cont.SaveChanges();
        }

        public IEnumerable<SalaryPerMonth> GetCollection( )
        {

            return cont.SalaryPerMonthSet;
        }
    }
}