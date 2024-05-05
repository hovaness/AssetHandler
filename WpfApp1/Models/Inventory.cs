using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    internal class Inventory : RealAsset
    {
        private string _type;
        private int _quantity;
        private string _unit;

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
            }
        }

        public string Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
            }
        }


        public decimal InitialValue
        {
            get { return _initialValue; }
            set
            {
                _initialValue = value;
            }
        }

        public decimal MarketValue
        {
            get { return _marcetValue; }
            set
            {
                _marcetValue = value;
            }
        }

        public decimal ResidualValue
        {
            get { return GetResidualValue(); }
        }

        public  string  Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }


        public string Name
        {
            get { return GetName(); }
        }

        public Inventory(string type, string unit,string currency, int quantity, decimal init, decimal market) {
            _type = type;
            _unit = unit;
            _quantity = quantity;
            _currency = currency;
            _initialValue = init;
            _marcetValue = market;
            _residualValue = GetResidualValue();
        }

        public override string GetName()
        {
            return string.Format("{0} {1} {2} начальная стоимость {3}, рыночная стоимость {4}, остаточная стоимость - {5}",
                _quantity,_unit, _type, _initialValue,_marcetValue,_residualValue);
        }

        public override decimal GetResidualValue()
        {
            return _quantity * _marcetValue - _initialValue * _quantity;
        }
    }
}
