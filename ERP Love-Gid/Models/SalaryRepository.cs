using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class SalaryRepository
    {
        private ERPModelContainer cont;


        public SalaryRepository(ERPModelContainer _cont)
        {
            cont = _cont;
        }
        public void Add(Salary s)
        { 
            cont.SalarySet.Add(s);
            cont.SaveChanges();
        }
        public void Add(List<Salary> s)
        {
            cont.SalarySet.RemoveRange(cont.SalarySet);
            foreach(Salary f in s)
            cont.SalarySet.Add(f);
            cont.SaveChanges();
        }
        public Salary GetElem(int id)
        {

            return cont.SalarySet.Find(id);
        }
        public IEnumerable<Salary> GetCollection()
        {

            return cont.SalarySet;
        }
        public IEnumerable<SalaryTypes> GetCollectionTypes()
        {

            return cont.SalaryTypesSet ;
        }

        public SalaryTypes GetElemType(int id)
        {
            return cont.SalaryTypesSet.Find(id);
        }

        internal void Edit(Salary salaryItem)
        {
            Salary tmp = GetElem(salaryItem.Id);

            tmp = salaryItem;
            cont.SaveChanges();

        }
         
    }
}