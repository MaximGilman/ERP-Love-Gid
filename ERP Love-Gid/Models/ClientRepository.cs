using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class ClientRepository
    {
        private ERPModelContainer cont;


        public ClientRepository(ERPModelContainer _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Client> GetCollection()
        {

            return cont.ClientSet.OrderBy(cw => cw.FIO);
        }

        public Client GetElem(int id)
        {
            return cont.ClientSet.Find(id);
        }

        public Client Add(Client M)
        {
            if (cont.ClientSet.Where(x=>x.FIO==M.FIO).Count()==0) cont.ClientSet.Add(M);

            cont.SaveChanges();

            return M;
        }
        public Client Add(string M)
        {
            Client add = new Client();
            add.FIO = M;
            if (cont.ClientSet.Where(x => x.FIO == M).Count() == 0) cont.ClientSet.Add(add);
            cont.SaveChanges();

            return add;
        }
        public void Delete(int id)
        {
            Client cw = GetElem(id);
            if (cw != null)
            {
                cont.ClientSet.Remove(cw);
                cont.SaveChanges();
            }
        }

    }
}