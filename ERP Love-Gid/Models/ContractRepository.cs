using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class ContractRepository
    {
        private ERP_x0020_modelContainer cont;


        public ContractRepository(ERP_x0020_modelContainer _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Contract> GetCollection()
        {

            return cont.ContractSet.OrderBy(cw => cw.Id);
        }

        public Contract GetElem(int id)
        {
            return cont.ContractSet.Find(id);
        }

        public Contract Add(Contract M)
        {

            cont.ContractSet.Add(M);
            cont.SaveChanges();

            return M;
        }

        public void Delete(int id)
        {
            Contract cw = GetElem(id);
            if (cw != null)
            {
                cont.ContractSet.Remove(cw);
                cont.SaveChanges();
            }
        }
    }
}
}