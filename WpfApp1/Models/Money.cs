using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsHandler.Models
{
    internal class Money : Asset
    {
        public string Name
        {
            get { return GetName(); }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                if (value > 0)
                {
                    _amount = value;
                }
                else throw new ArgumentException("Сумма не может быть меньше 0");
            }
        }

        public string Currency
        {
            get { return _currency.ToString(); }
            set { _currency = value; }
        }

        //Констурктор для денег, которые лежат на кассе
        public Money(decimal amount, string currency)
        {
            _amount = amount;
            _currency = currency;
        }

        public override string GetName()
        {
              return string.Format("В кассе {0} {1}", _amount, _currency);
        }
    }
}
