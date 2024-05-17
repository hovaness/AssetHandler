using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Models;

namespace WpfApp1.Repositories
{
    //Класс наследник AssetRepository, отвечает за работу с кассовыми активами
    public class MoneyAssetRepository : AssetRepository
    {

        public MoneyAssetRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public override List<Asset> GetAllAssets()
        {
            string query = "SELECT a.*, c.Amount, c.Currency " +
                       "FROM Assets a " +
                       "JOIN CashAssets c ON a.Id = c.AssetId " +
                       "WHERE a.Type = 'Деньги'";
            List<Dictionary<string, object>> results = _dbContext.ExecuteQuery(query);
            List<Asset> assets = new List<Asset>();
            foreach (var row in results)
            {
                var Id = (int)row["id"];
                var amount = (decimal)row["amount"];
                var currency = (string)row["currency"];
                Money asset = new Money(amount, currency);
                asset.Id = Id;
                assets.Add(asset);
            }
            return assets;
        }

        public override void AddAsset(Asset asset)
        {
            string insertAssetQuery = "INSERT INTO Assets (Type) " +
                                  "VALUES ('Деньги') RETURNING Id";
            string insertCashQuery = "INSERT INTO CashAssets (AssetId, Amount, Currency,Id) " +
                                     "VALUES (@AssetId, @Amount, @Currency, @Id)";

            int assetId;
            _dbContext.BeginTransaction();
            try
            {
                assetId = _dbContext.ExecuteScalar<int>(insertAssetQuery);
                ((Money)asset).Id = assetId;
                _dbContext.Execute(insertCashQuery, new Dictionary<string, object>
                {
                    {"id", assetId },
                    {"assetid", assetId },
                    {"amount", ((Money)asset).Amount},
                    {"currency", ((Money)asset).Currency },
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
            string deleteCashQuery = "DELETE FROM CashAssets WHERE AssetId = @Id";

            _dbContext.BeginTransaction();
            try
            {
                _dbContext.Execute(deleteCashQuery, new Dictionary<string, object> { { "id", id } });
                _dbContext.Execute(deleteAssetQuery, new Dictionary<string, object> { { "id", id } });
                _dbContext.CommitTransaction();
            }
            catch
            {
                _dbContext.RollbackTransaction();
                throw;
            }
        }


        public override void UpdateAsset(Asset asset)
        {
            string updateAssetQuery = "UPDATE Assets SET Type = @Type " +
                               "WHERE Id = @Id";

            string updateCashQuery = "UPDATE CashAssets SET Amount = @Amount, Currency = @Currency " +
                                     "WHERE AssetId = @AssetId";

            _dbContext.BeginTransaction();
            try
            {
                _dbContext.Execute(updateAssetQuery, new Dictionary<string, object>
                {
                    { "Type", "Деньги"},
                    { "Id" , asset.Id}
                });

                _dbContext.Execute(updateCashQuery, new Dictionary<string, object>
                {
                    { "Amount", ((Money)asset).Amount},
                    { "Currency" , ((Money)asset).Currency},
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
