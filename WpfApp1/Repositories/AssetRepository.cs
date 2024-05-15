using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Repositories
{
    public abstract class AssetRepository
    {
        public abstract List<Asset> GetAllAssets();
        public abstract void AddAsset(Asset asset);
        public abstract void UpdateAsset(Asset asset);
        public abstract void DeleteAsset(int id);
    }
}
