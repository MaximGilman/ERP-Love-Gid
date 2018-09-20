using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class EventRepository
    {
        private ERPModelContainer cont;


        public EventRepository(ERPModelContainer _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Event> GetCollection(string sort ="")
        {
 
            return cont.EventSet.OrderBy(cw => cw.Id).Select(x=>x);
        }

        public Event GetElem(int id)
        {
            return cont.EventSet.Find(id);
        }
        public Event GetElem(string name)
        {
            return cont.EventSet.Where(x=>x.Type==name).FirstOrDefault();
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