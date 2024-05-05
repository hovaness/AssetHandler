using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsHandler.Models
{

    public abstract class Asset
    {
        protected decimal _amount;
        protected string _currency;

        public abstract string GetName();
    }
}
