using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class ContractRepository
    {

        private ERPModelContainer cont;


        public ContractRepository(ERPModelContainer _cont)
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
        public IEnumerable<int> GetYears()
        {
            return cont.ContractSet.Select(x => x.Date_of_event.Year).Distinct().OrderBy(y => y);
        }
        public Contract Add(int sumonly, int eventid, string client, DateTime sign, DateTime _event, DateTime pay1, DateTime pay2 ,DateTime pay3,int paysum1, int paysum2, int paysum3, string comment, int emplid  )
        {
            Contract add = new Contract();
            add.Sum_only_contract = sumonly;
            add.EventSet = cont.EventSet.Find(eventid);
            Client cladd = new Client();
            cladd.FIO = client;
            add.ClientSet = cont.ClientSet.Add(cladd);
            add.Date_of_sign = sign;
            add.Date_of_event   = _event;
            add.Payment1Date =       pay1;
            add.Payment2Date =      pay2;
            add.Payment3Date =pay3;
            add.Payment1Sum = paysum1; add.Payment2Sum = paysum2;

            add.Payment3Sum = paysum3;

            add.Comment = comment;
            add.Received = 0;
            add.Sum_plus = 0;
            add.Status = Status.Активный.ToString();
            add.EmployeeSet = cont.EmployeeSet.Find(emplid); // Убрать потом при регистрации!!!!!!!
            add.Name = add.Date_of_event.ToString().Split(' ')[0] + " — " + " (" + add.EmployeeSet.FIO + ") " + add.ClientSet.FIO;
            cont.ContractSet.Add(add);
            cont.SaveChanges();
            return add;
        }
        public Contract Add(Contract M)
        {
            Contract a = new Contract();
            a = M;
            cont.ContractSet.Add(a);
            cont.SaveChanges();

            return a;
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
