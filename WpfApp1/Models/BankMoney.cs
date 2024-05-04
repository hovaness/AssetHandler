using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsHandler.Models
{
    internal class BankMoney : Money
    {

        private int _bill;

        private string _bank;

        public int Bill
        {
            get { return _bill; }
            set { _bill = value; }
        }

        public string Bank
        {
            get { return _bank; }
            set { _bank = value; }
        }
        

        public BankMoney(decimal amount, string currency, string bank, int bill) : base(amount, currency)
        {
            _bank = bank;
            _bill = bill;
        }

        public override string GetName()
        {
            return string.Format("На счету {0} в {1}е {2} {3}", _bill, _bank, _amount, _currency);
        }
    }
}
