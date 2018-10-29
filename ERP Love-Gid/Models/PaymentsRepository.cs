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
        public IEnumerable<Payments> GetCollection(bool flagofstatus)
        {
            foreach (Payments pm in cont.PaymentsSet)
            {
                try
                {

                    pm.StatusForSalary = cont.Pay_minSet.Where(x => x.Date == pm.Date && pm.Account.Id == x.Account.Id).Select(y => y.Finished).FirstOrDefault();

                }
                catch { pm.StatusForSalary = false; }





            }
            cont.SaveChanges();

            return cont.PaymentsSet.OrderBy(cw => cw.Id);
        }
        public IEnumerable<Payments> GetEmplPays(int id, int month = -1)
        {
            if(month!=-1) return cont.PaymentsSet.Where(x => x.Employee.Id == id&&x.Date.Month==month).OrderBy(cw => cw.Id);

            else return cont.PaymentsSet.Where(x => x.Employee.Id == id).OrderBy(cw => cw.Id);



        }
        public int GetEmplPaysSum(int id, int month = -1)
        {

            try
            {
                if (month != -1) return cont.PaymentsSet.Where(x => x.Employee.Id == id&& x.Date.Month==month).Select(y => y.Receipt).Sum();

                else  
                return cont.PaymentsSet.Where(x => x.Employee.Id == id).Select(y => y.Receipt).Sum();
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

        public IEnumerable<Payments> GetEmplJobs(int id)
        {
            return cont.PaymentsSet.Where(x => x.Employee.Id == id).OrderBy(cw => cw.Id);

        }

        public Payments EditPaymentDetail(Payments adder)
        {
            Payments edit = GetElem(adder.Id);
            edit = adder;
            cont.SaveChanges();
            return adder;
        }
    }
}