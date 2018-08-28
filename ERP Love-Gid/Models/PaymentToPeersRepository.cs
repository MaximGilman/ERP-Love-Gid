using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class PaymentToPeersRepository
    {
        private ERPModelContainer cont;


        public PaymentToPeersRepository(ERPModelContainer _cont)
        {
            cont = _cont;
        }

        public IEnumerable<PaymentToPeers> GetCollection()
        {

            return cont.PaymentToPeersSet.OrderBy(cw => cw.Id);
        }

        public PaymentToPeers GetElem(int id)
        {
            return cont.PaymentToPeersSet.Find(id);
        }

        public PaymentToPeers Add(PaymentToPeers M)
        {

            cont.PaymentToPeersSet.Add(M);
            cont.SaveChanges();

            return M;
        }

        public void Delete(int id)
        {
            PaymentToPeers cw = GetElem(id);
            if (cw != null)
            {
                cont.PaymentToPeersSet.Remove(cw);
                cont.SaveChanges();
            }
        }
    }
}