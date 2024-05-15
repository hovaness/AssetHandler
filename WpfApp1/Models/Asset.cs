using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsHandler.Models
{

    public abstract class Asset
    {
        protected decimal _amount;
        protected string _currency;
        protected int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (value > 0) { _id = value; }
            }
        }

        public abstract string GetName();
    }

    public class AssetsViewModel
    {
        public ObservableCollection<Asset> assets;
        
        public AssetsViewModel() {
            
        }
    }
}
