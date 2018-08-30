using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class PaymentsRepository
    {
        private ERPModelContainer cont;


        public PaymentsRepository(ERPModelContainer _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Payments> GetCollection()
        {

            return cont.PaymentsSet.OrderBy(cw => cw.Id);
        }

        public IEnumerable<Payments> GetEmplPays(int id)
        {
             return cont.PaymentsSet.Where(x => x.Contract.EmployeeSet.Id == id).OrderBy(cw => cw.Id);  
            
         }
        public int GetEmplPaysSum(int id)
        {
            try
            {
                return cont.PaymentsSet.Where(x => x.Contract.EmployeeSet.Id == id).Select(y => y.Receipt).Sum();
            }
            catch { return 0; }
        }
        public Payments GetElem(int id)
        {
            return cont.PaymentsSet.Find(id);
        }

        public Payments Add(Payments M)
        {

            cont.PaymentsSet.Add(M);
            cont.SaveChanges();

            return M;
        }

        public void Delete(int id)
        {
            Payments cw = GetElem(id);
            if (cw != null)
            {
                cont.PaymentsSet.Remove(cw);
                cont.SaveChanges();
            }
        }
    }
}