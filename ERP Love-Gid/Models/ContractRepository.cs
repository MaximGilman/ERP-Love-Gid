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
            return cont.ContractSet.Select(x => x.Date_of_event.Value.Year).Distinct().OrderBy(y => y);
        }
        public Contract Add(int sumonly, int eventid, string client, DateTime sign, DateTime _event, DateTime pay1, DateTime pay2, DateTime pay3, int paysum1, int paysum2, int paysum3, string comment, int emplid)
        {
            Contract add = new Contract();
            add.Sum_only_contract = sumonly;
            add.EventSet = cont.EventSet.Find(eventid);
            Client cladd = new Client();
            cladd.FIO = client;
            if (cont.ClientSet.Where(x => x.FIO == cladd.FIO).Count() == 0) {cont.ClientSet.Add(cladd);  cont.SaveChanges(); }
            add.ClientSet = cont.ClientSet.Where(x=>x.FIO==cladd.FIO).FirstOrDefault();
            add.Date_of_sign = sign;
            add.Date_of_event   = _event;
            if (pay1!= default(DateTime))
            add.Payment1Date =       pay1;
            if (pay2 != default(DateTime))
                add.Payment2Date =      pay2;
            if (pay3 != default(DateTime))
                add.Payment3Date =pay3;
            if(paysum1!=0)add.Payment1Sum = paysum1;
            if (paysum2 != 0) add.Payment2Sum = paysum2;

            if (paysum3 != 0) add.Payment3Sum = paysum3;

            add.Comment = comment;
            add.Received = 0;
            add.Sum_plus = 0;
            add.Status = Status.Активный.ToString();
            add.EmployeeSet = cont.EmployeeSet.Find(emplid); // Убрать потом при регистрации!!!!!!!
            add.Name = add.Date_of_event.ToString().Split(' ')[0] + " — " + " (" + add.EmployeeSet.Name.First()+add.EmployeeSet.Surname.First() + ") " + add.ClientSet.FIO;
            cont.ContractSet.Add(add);
            cont.SaveChanges();
            return add;
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
        public IEnumerable<Contract> GetAllPaysForMonth(int id, int year = 0, int month = 0 )
        {
            if (year == 0) year = DateTime.Now.Year; if (month == 0) month = DateTime.Now.Month;
            return cont.ContractSet.Where(x => ( x.Payment1Date.Value.Month == month && x.Payment1Date.Value.Year == year|| x.Payment2Date.Value.Month == month && x.Payment2Date.Value.Year == year|| x.Payment3Date.Value.Month == month && x.Payment3Date.Value.Year == year) && x.EmployeeSet.Id==id).OrderBy(sort=>sort.Id);
        }
        //public int GetAllPaysForMonthSum(int id, int year = 0, int month = 0)
        //{
        //    if (year == 0) year = DateTime.Now.Year; if (month == 0) month = DateTime.Now.Month;
        //    return cont.ContractSet.Where(x => (x.Payment1Date.Month == month && x.Payment1Date.Year == year || x.Payment2Date.Month == month && x.Payment2Date.Year == year || x.Payment3Date.Month == month && x.Payment3Date.Year == year) && x.EmployeeSet.Id == id).OrderBy(sort => sort.Id).Select();
        //}

        public Contract Edit(int id,int sumonly, int eventid, string client, DateTime sign, DateTime _event, DateTime pay1, DateTime pay2, DateTime pay3, int paysum1, int paysum2, int paysum3, string comment, int emplid)
        {
            Contract add = GetElem(id);
            add.Sum_only_contract = sumonly;
            add.EventSet = cont.EventSet.Find(eventid);
            Client cladd = new Client();
            cladd.FIO = client;
            if (cont.ClientSet.Where(x => x.FIO == cladd.FIO).Count() == 0) { cont.ClientSet.Add(cladd); cont.SaveChanges(); }
            add.ClientSet = cont.ClientSet.Where(x => x.FIO == cladd.FIO).FirstOrDefault();
            add.Date_of_sign = sign;
            add.Date_of_event = _event;
            if (pay1 != default(DateTime))
                add.Payment1Date = pay1;
            if (pay2 != default(DateTime))
                add.Payment2Date = pay2;
            if (pay3 != default(DateTime))
                add.Payment3Date = pay3;
            if (paysum1 != 0) add.Payment1Sum = paysum1;
            if (paysum2 != 0) add.Payment2Sum = paysum2;

            if (paysum3 != 0) add.Payment3Sum = paysum3;

            add.Comment = comment;
            add.Received = 0;
            add.Sum_plus = 0;
            add.Status = Status.Активный.ToString();
            add.EmployeeSet = cont.EmployeeSet.Find(emplid); // Убрать потом при регистрации!!!!!!!
            add.Name = add.Date_of_event.ToString().Split(' ')[0] + " — " + " (" + add.EmployeeSet.Name.First() + add.EmployeeSet.Surname.First() + ") " + add.ClientSet.FIO;
          
            cont.SaveChanges();
            return add;
        }
    }
}
