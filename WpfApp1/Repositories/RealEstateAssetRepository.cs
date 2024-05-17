using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Repositories
{
    public class RealEstateAssetRepository : AssetRepository
    {
        //Класс наследник AssetRepository, отвечает за работу с активами-недвижимостью.
        public RealEstateAssetRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void AddAsset(Asset asset)
        {
            string addAssetQuery = "insert into assets (type) values ('Недвижимость') returning id";
            string addInventoryQuery = "insert into estateassets (assetId, InitValue, marketValue, addres, construction, year, inumber, currency,id) " +
                "values (@assetId, @InitValue, @marketValue, @addres, @construction, @year, @inumber, @currency,@id)";
            int assetId;
            _dbContext.BeginTransaction();
            try
            {
                assetId = _dbContext.ExecuteScalar<int>(addAssetQuery);
                ((RealEstate)asset).Id = assetId;
                _dbContext.Execute(addInventoryQuery, new Dictionary<string, object>
                {
                    {"id", assetId },
                    {"assetId", assetId },
                    {"InitValue", ((RealEstate)asset).InitialValue},
                    {"marketValue", ((RealEstate)asset).MarketValue},
                    {"addres", ((RealEstate)asset).Address},
                    {"construction", ((RealEstate)asset).ConstructionType},
                    {"year", ((RealEstate)asset).ConstructionYear },
                    {"inumber", ((RealEstate)asset).InventoryNumber },
                    {"currency", ((RealEstate)asset).Currency}
                });
                _dbContext.CommitTransaction();
            }
            catch
            {
                _dbContext.RollbackTransaction();
                throw;
            }
        }

        public override void DeleteAsset(int id)
        {
            string deleteAssetQuery = "DELETE FROM Assets WHERE Id = @Id";
            string deleteEstateQuery = "DELETE FROM estateassets WHERE AssetId = @Id";

            _dbContext.BeginTransaction();
            try
            {
                _dbContext.Execute(deleteEstateQuery, new Dictionary<string, object> { { "id", id } });
                _dbContext.Execute(deleteAssetQuery, new Dictionary<string, object> { { "id", id } });
                _dbContext.CommitTransaction();
            }
            catch
            {
                _dbContext.RollbackTransaction();
                throw;
            }
        }

        public override List<Asset> GetAllAssets()
        {
            string query = "SELECT a.*, e.* " +
                       "FROM Assets a " +
                       "JOIN estateAssets e ON a.Id = e.AssetId " +
                       "WHERE a.Type = 'Недвижимость'";
            List<Dictionary<string, object>> resulst = _dbContext.ExecuteQuery(query);
            List<Asset> assets = new List<Asset>();

            foreach (var row in resulst)
            {
                var id = (int)row["id"];
                var init = (decimal)row["initvalue"];
                var market = (decimal)row["marketvalue"];
                var currency = (string)row["currency"];
                var addres = (string)row["addres"];
                var construct = (string)row["construction"];
                var year = (int)row["year"];
                var number = (int)row["inumber"];
                var estate = new RealEstate(addres,construct, currency, year, number, init, market);
                estate.Id = id;
                assets.Add(estate);
            }
            return assets;
        }

        public override void UpdateAsset(Asset asset)
        {
            string updateAssetQuery = "UPDATE Assets SET Type = @Type " +
                              "WHERE Id = @Id";
            string updateCashQuery = "UPDATE estateAssets SET year = @year, currency = @currency, inumber = @inumber, " +
                                     "initValue = @initValue, marketValue = @marketValue, addres = @addres, " +
                                     "construction = @construction WHERE AssetId = @AssetId";
            _dbContext.BeginTransaction();
            try
            {
                _dbContext.Execute(updateAssetQuery, new Dictionary<string, object>
                {
                    { "Type", "Недвижимость"},
                    { "Id" , asset.Id}
                });

                _dbContext.Execute(updateCashQuery, new Dictionary<string, object>
                {
                    {"year", ((RealEstate)asset).ConstructionYear},
                    {"currency" , ((RealEstate)asset).Currency},
                    {"initValue", ((RealEstate)asset).InitialValue },
                    {"marketValue", ((RealEstate)asset).MarketValue },
                    {"addres", ((RealEstate)asset).Address},
                    {"construction", ((RealEstate)asset).ConstructionType},
                    {"inumber", ((RealEstate)asset).InventoryNumber},
                    {"AssetId" , asset.Id }
                });

                _dbContext.CommitTransaction();
            }
            catch
            {
                _dbContext.RollbackTransaction();
                throw;
            }
        }
    }
}
