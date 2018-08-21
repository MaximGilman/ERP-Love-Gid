using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class AccountRepository
    {
        private ERP_x0020_modelContainer cont;


        public AccountRepository(ERP_x0020_modelContainer _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Account> GetCollection()
        {

            return cont.AccountSet.OrderBy(cw => cw.Id);
        }

        public Account GetElem(int id)
        {
            return cont.AccountSet.Find(id);
        }

        public Account Add(Account M)
        {

            cont.AccountSet.Add(M);
            cont.SaveChanges();

            return M;
        }

        public void Delete(int id)
        {
            Account cw = GetElem(id);
            if (cw != null)
            {
                cont.AccountSet.Remove(cw);
                cont.SaveChanges();
            }
        }
    }
}