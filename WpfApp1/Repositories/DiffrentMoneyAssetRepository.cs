using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Repositories
{
    //Класс наследник AssetRepository, отвечает за работу с активами с твердым номиналом
    public class DifferentMoneyAssetRepository : AssetRepository
    {
        public DifferentMoneyAssetRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void AddAsset(Asset asset)
        {
            string addAssetQuery = "insert into assets (type) values ('Твердый номинал') returning id";
            string addDiffQuery = "insert into diffAssets (assetId, amount, currency, owner, holding, id)" +
                "values (@assetId, @amount, @currency, @owner, @holding, @id)";
            int assetId;
            _dbContext.BeginTransaction();
            try
            {
                assetId = _dbContext.ExecuteScalar<int>(addAssetQuery);
                ((DiffrentMoney)asset).Id = assetId;
                _dbContext.Execute(addDiffQuery, new Dictionary<string, object>
                {
                    {"id", assetId },
                    {"assetId", assetId },
                    {"amount", ((DiffrentMoney)asset).Amount},
                    {"currency", ((DiffrentMoney)asset).Currency },
                    {"owner", ((DiffrentMoney)asset).Owner},
                    {"holding", ((DiffrentMoney)asset).Holding},
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
            string deleteDiffQuery = "DELETE FROM diffAssets WHERE AssetId = @Id";

            _dbContext.BeginTransaction();
            try
            {
                _dbContext.Execute(deleteDiffQuery, new Dictionary<string, object> { { "id", id } });
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
            string query = "SELECT a.*, d.Amount, d.Currency, d.Owner, d.Holding " +
                       "FROM Assets a " +
                       "JOIN diffAssets d ON a.Id = d.AssetId " +
                       "WHERE a.Type = 'Твердый номинал'";
            List<Dictionary<string, object>> results = _dbContext.ExecuteQuery(query);
            List<Asset> assets = new List<Asset>();

            foreach(var row in results)
            {
                var id = (int)row["id"];
                var amount = (decimal)row["amount"];
                var currency = (string)row["currency"];
                var owner = (string)row["owner"];
                var holding = (string)row["holding"];
                var diffMoney = new DiffrentMoney(amount, currency, owner, holding);
                diffMoney.Id = id;
                assets.Add(diffMoney);
            }
            return assets;
        }

        public override void UpdateAsset(Asset asset)
        {
            string updateAssetQuery = "UPDATE Assets SET Type = @Type " +
                              "WHERE Id = @Id";

            string updateCashQuery = "UPDATE diffAssets SET Amount = @Amount, Currency = @Currency, " +
                                     "Owner = @Owner, Holding = @Holding " +
                                     "WHERE AssetId = @AssetId";

            _dbContext.BeginTransaction();
            try
            {
                _dbContext.Execute(updateAssetQuery, new Dictionary<string, object>
                {
                    { "Type", "Твердый номинал"},
                    { "Id" , asset.Id}
                });

                _dbContext.Execute(updateCashQuery, new Dictionary<string, object>
                {
                    { "Amount", ((DiffrentMoney)asset).Amount},
                    { "Currency" , ((DiffrentMoney)asset).Currency},
                    {"Owner", ((DiffrentMoney)asset).Owner },
                    {"Holding", ((DiffrentMoney)asset).Holding },
                    { "AssetId" , asset.Id }
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
