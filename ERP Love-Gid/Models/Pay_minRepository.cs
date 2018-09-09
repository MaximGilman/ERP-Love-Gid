﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_Love_Gid.Models
{
    public class Pay_minRepository
    {
        private ERPModelContainer cont;


        public Pay_minRepository(ERPModelContainer _cont)
        {
            cont = _cont;
        }
        public IEnumerable<Pay_min> GetCollection()
        {

            return cont.Pay_minSet.OrderBy(cw => cw.Id);
        }

        public IEnumerable<Pay_min> GetEmplPays(int id)
        {
            foreach (Pay_min pm in cont.Pay_minSet)
            {
                 try
                {
                    pm.Finished = cont.Pay_minSet.Where(
                        x => x.Account.Id == pm.Account.Id && x.Date == pm.Date).Select(y => y.Sum).Sum() ==
                       cont.PaymentsSet.Where(x => x.Account.Id == pm.Account.Id && x.Date == pm.Date).Select(y => y.Receipt).Sum();

                }
                catch { pm.Finished = false; }
                




            }
            cont.SaveChanges();
            return cont.Pay_minSet.Where(x => x.Employee.Id == id).OrderBy(cw => cw.Id);

        }
        public int GetEmplPaysSum(int id)
        {
            try
            {
                return cont.Pay_minSet.Where(x => x.Employee.Id == id).Select(y => y.Sum).Sum();
            }
            catch { return 0; }
        }
        public Pay_min GetElem(int id)
        {
            return cont.Pay_minSet.Find(id);
        }

        public Pay_min Add(Pay_min M)
        {
            
             cont.Pay_minSet.Add(M);
            cont.SaveChanges();

            return M;
        }
    }
}