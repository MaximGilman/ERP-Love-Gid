using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class PaymentsRepository
    {
        private ERP_x0020_modelContainer cont;


        public PaymentsRepository(ERP_x0020_modelContainer _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Payments> GetCollection()
        {

            return cont.PaymentsSet.OrderBy(cw => cw.Id);
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