using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    internal class RealEstate : RealAsset
    {
        private string _addres;
        private int _constructionYear;
        private string _constructionType;
        private int _inventoryNumber;

        public int ConstructionYear
        {
            get { return _constructionYear; }
            set { _constructionYear = value; }
        }

        public string ConstructionType
        {
            get { return _constructionType; }
            set { _constructionType = value; }
        }

        public int InventoryNumber
        {
            get { return _inventoryNumber; }
            set { _inventoryNumber = value; }
        }


        public string Address
        {
            get { return _addres; }
            set { _addres = value; }
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

        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public string Name
        {
            get { return GetName(); }
        }

        public RealEstate(string addres, string type, string currency, int year,  int number, decimal init, decimal marcet )
        {
            _currency = currency;
            _addres = addres;
            _constructionYear = year;
            _constructionType = type;
            _inventoryNumber = number;
            _initialValue = init;
            _marcetValue = marcet;
            _residualValue = GetResidualValue();
        }

        public override decimal GetResidualValue()
        {
            return _marcetValue - _initialValue;
        }
        public override string GetName()
        {
            return string.Format("{0} по адресу {1}, год постройки {2}, начальная стоимость {3} {7}, рыночная стоимость {4} {7}, остаточная стоимость - {5} {7}, инвентарный номер {6}",
                _constructionType, _addres, _constructionYear, _initialValue, _marcetValue, _residualValue, _inventoryNumber, _currency);
        }


    }
}
