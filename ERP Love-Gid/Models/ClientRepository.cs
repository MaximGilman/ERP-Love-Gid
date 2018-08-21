using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class ClientRepository
    {
        private ERP_x0020_modelContainer cont;


        public ClientRepository(ERP_x0020_modelContainer _cont)
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

            cont.ClientSet.Add(M);
            cont.SaveChanges();

            return M;
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