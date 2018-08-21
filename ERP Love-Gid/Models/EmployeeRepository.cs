using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class EmployeeRepository
    {
        private ERP_x0020_modelContainer cont;

        
        public EmployeeRepository(ERP_x0020_modelContainer _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Employee> GetCollection()
        {
          
            return cont.EmployeeSet.OrderBy(cw => cw.FIO);
        }

        public Employee GetElem(int id)
        {
            return cont.EmployeeSet.Find(id);
        }

        public Employee Add(Employee M)
        {

            cont.EmployeeSet.Add(M);
            cont.SaveChanges();

            return M;
        }

        public void Delete(int id)
        {
            Employee cw = GetElem(id);
            if (cw != null)
            {
                cont.EmployeeSet.Remove(cw);
                cont.SaveChanges();
            }
        }

        //public void Edit(int id, string fio, int cost, int count, int margin, string resol)
        //{
        //    Employee cw = GetElem(id);
        //    if (cw != null)
        //    {
        //        cw.FIO = fio;
        //        cw.
        //        cont.SaveChanges();
        //    }
        //}
    }
}