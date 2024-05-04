using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsHandler.Models
{
    internal class DiffrentMoney : Money
    {
        private string _holding;
        private string _owner;

        public string Holding
        {
            get { return _holding; }
            set { _holding = value; }
        }
        public string Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        public DiffrentMoney(decimal amount, string currency, string asset, string owner) : base(amount, currency)
        {
            _holding = asset;
            _owner = owner;
        }

        public override string GetName()
        {
            return string.Format("В кассе {0} от {1} на сумму {2} {3}", _holding, _owner, _amount, _currency);
        }
    }
}
