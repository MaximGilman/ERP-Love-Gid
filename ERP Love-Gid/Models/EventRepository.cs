using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class EventRepository
    {
        private ERP_x0020_modelContainer cont;


        public EventRepository(ERP_x0020_modelContainer _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Event> GetCollection()
        {

            return cont.EventSet.OrderBy(cw => cw.Id);
        }

        public Event GetElem(int id)
        {
            return cont.EventSet.Find(id);
        }

        public Event Add(Event M)
        {

            cont.EventSet.Add(M);
            cont.SaveChanges();

            return M;
        }

        public void Delete(int id)
        {
            Event cw = GetElem(id);
            if (cw != null)
            {
                cont.EventSet.Remove(cw);
                cont.SaveChanges();
            }
        }

    }
}