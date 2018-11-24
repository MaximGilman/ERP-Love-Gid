using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class SalaryTypeRepository
    {
        private ERPModelContainer cont;


        public SalaryTypeRepository(ERPModelContainer _cont)
        {
            cont = _cont;
        }
        public IEnumerable<SalaryTypes> GetCollection()
        {

            return cont.SalaryTypesSet;
        }
        public void Add(SalaryTypes s)
        {
            cont.SalaryTypesSet.Add(s);
            cont.SaveChanges();
        }

    }
}