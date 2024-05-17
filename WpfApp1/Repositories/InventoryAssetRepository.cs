using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Repositories
{
    //Класс наследник AssetRepository, отвечает за работу с инвентарными активами
    public class InventoryAssetRepository : AssetRepository
    {
        public InventoryAssetRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void AddAsset(Asset asset)
        {
            string addAssetQuery = "insert into assets (type) values ('Инвентарь') returning id";
            string addInventoryQuery = "insert into inventoryassets (assetId, InitValue, marketValue, type, unit, quantity, currency,id) " +
                "values (@assetId, @initValue, @marketValue, @type, @unit, @quantity, @currency, @id)";
            int assetId;
            _dbContext.BeginTransaction();
            try
            {
                assetId = _dbContext.ExecuteScalar<int>(addAssetQuery);
                ((Inventory)asset).Id = assetId;
                _dbContext.Execute(addInventoryQuery, new Dictionary<string, object>
                {
                    {"id", assetId },
                    {"assetId", assetId },
                    {"InitValue", ((Inventory)asset).InitialValue},
                    {"marketValue", ((Inventory)asset).MarketValue},
                    {"unit", ((Inventory)asset).Unit},
                    {"type", ((Inventory)asset).Type},
                    {"quantity", ((Inventory)asset).Quantity },
                    {"currency", ((Inventory)asset).Currency}
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
            string deleteInventoryQuery = "DELETE FROM InventoryAssets WHERE AssetId = @Id";

            _dbContext.BeginTransaction();
            try
            {
                _dbContext.Execute(deleteInventoryQuery, new Dictionary<string, object> { { "id", id } });
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
            string query = "SELECT a.*, i.* " +
                       "FROM Assets a " +
                       "JOIN inventoryAssets i ON a.Id = i.AssetId " +
                       "WHERE a.Type = 'Инвентарь'";
            List<Dictionary<string, object>> resulst = _dbContext.ExecuteQuery(query);
            List<Asset> assets = new List<Asset>();

            foreach (var row in resulst)
            {
                var id = (int)row["id"];
                var init = (decimal)row["initvalue"];
                var market = (decimal)row["marketvalue"];
                var currency = (string)row["currency"];
                var type = (string)row["type"];
                var unit = (string)row["unit"];
                var quantity = (int)row["quantity"];
                var inventory = new Inventory(type, unit, currency, quantity, init, market);
                inventory.Id = id;
                assets.Add(inventory);
            }
            return assets;
        }

        public override void UpdateAsset(Asset asset)
        {
            string updateAssetQuery = "UPDATE Assets SET Type = @Type " +
                              "WHERE Id = @Id";
            string updateCashQuery = "UPDATE inventoryAssets SET type = @type, currency = @currency, " +
                                     "initValue = @initValue, marketValue = @marketValue, unit = @unit, quantity = @quantity " +
                                     "WHERE AssetId = @AssetId";
            _dbContext.BeginTransaction();
            try
            {
                _dbContext.Execute(updateAssetQuery, new Dictionary<string, object>
                {
                    { "Type", "Инвентарь"},
                    { "Id" , asset.Id}
                });

                _dbContext.Execute(updateCashQuery, new Dictionary<string, object>
                {
                    {"type", ((Inventory)asset).Type},
                    {"currency" , ((Inventory)asset).Currency},
                    {"initValue", ((Inventory)asset).InitialValue },
                    {"marketValue", ((Inventory)asset).MarketValue },
                    {"unit", ((Inventory)asset).Unit},
                    {"quantity", ((Inventory)asset).Quantity},
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
