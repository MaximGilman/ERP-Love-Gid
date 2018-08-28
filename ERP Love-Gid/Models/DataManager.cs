using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class DataManager
    {
        public ERPModelContainer shop_cont;
        public AccountRepository AccR;
        public ClientRepository CliR;
        public ContractRepository ConR;
        public EmployeeRepository EmR;
        public EventRepository EvR;
        public PaymentsRepository PayR;
        public PaymentToPeersRepository PayToPR;



        public DataManager()
        {
            shop_cont = new ERPModelContainer();
            AccR = new AccountRepository(shop_cont);
            CliR = new ClientRepository(shop_cont);

            ConR = new ContractRepository(shop_cont);

            EmR = new EmployeeRepository(shop_cont);

            EvR = new EventRepository(shop_cont);

            PayR = new PaymentsRepository(shop_cont);

            PayToPR = new PaymentToPeersRepository(shop_cont);
             
          }          
    }
}