using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class Pay_minRepository
    {
        private ERPModelContainer cont;


        public Pay_minRepository(ERPModelContainer _cont)
        {
            cont = _cont;
        }
        public IEnumerable<Pay_min> GetCollection()
        {

            return cont.Pay_minSet.OrderBy(cw => cw.Id);
        }

        public IEnumerable<Pay_min> GetEmplPays(int id)
        {
            foreach (Pay_min pm in cont.Pay_minSet)
            {
                 try
                {

                    var a = cont.Pay_minSet.Where(
                       x => x.Account.Id == pm.Account.Id && x.Date == pm.Date&&x.Employee.Id==pm.Employee.Id).Select(y => y.Sum).Sum();
                    var b = cont.PaymentsSet.Where(x => x.Account.Id == pm.Account.Id && x.Date == pm.Date&&x.Employee.Id==pm.Employee.Id).Select(y => y.Receipt).Sum();
                   
                    pm.Finished = a ==
                       b;

                }
                catch { pm.Finished = false; }
                




            }
            cont.SaveChanges();
            return cont.Pay_minSet.Where(x => x.Employee.Id == id).OrderBy(cw => cw.Id);

        }
        public int GetEmplPaysSum(int id, int month=-1)
        {
            try
            {
                if (month != -1) return cont.Pay_minSet.Where(x => x.Employee.Id == id&&x.Date.Month==month).Select(y => y.Sum).Sum();

                else
                    return cont.Pay_minSet.Where(x => x.Employee.Id == id).Select(y => y.Sum).Sum();
            }
            catch { return 0; }
        }
        public Pay_min GetElem(int id)
        {
            return cont.Pay_minSet.Find(id);
        }

        public Pay_min Add(Pay_min M)
        {
            
             cont.Pay_minSet.Add(M);
            cont.SaveChanges();

            return M;
        }

        public Pay_min Edit_PayMin(Pay_min adder)
        {
            Pay_min edit = GetElem(adder.Id);
            edit = adder;
            cont.SaveChanges();
            return adder;
         }
        public void Delete(int id)
        {
            Pay_min cw = GetElem(id);
            if (cw != null)
            {
                cont.Pay_minSet.Remove(cw);
                cont.SaveChanges();
            }
        }
    }
}