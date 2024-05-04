using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    abstract class RealAsset : Asset
    {
        protected decimal _initialValue;
        protected decimal _marcetValue;
        protected decimal _residualValue;
        public abstract decimal GetResidualValue();
    }
}
