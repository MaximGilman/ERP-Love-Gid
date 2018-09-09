using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class EmployeeRepository
    {
        private ERPModelContainer cont;

        
        public EmployeeRepository(ERPModelContainer _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Employee> GetCollection()
        {
          foreach (Employee em in cont.EmployeeSet)
            { em.FIO = em.Name + " " + em.Surname; }
            return cont.EmployeeSet.OrderBy(cw => cw.Name);
        }

        public Employee GetElem(int id)
        {
            return cont.EmployeeSet.Find(id);
        }
        public bool Contains(string login)
        {
            return cont.EmployeeSet.Where(x => x.Login== login).Count()>0 ? true: false;
        }
        public int Getid(string login)
        {
            return cont.EmployeeSet.Where(x => x.Login == login).Select(y=>y.Id).FirstOrDefault();
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