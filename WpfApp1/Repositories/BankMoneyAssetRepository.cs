using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Repositories 
{
    public class BankMoneyAssetRepository : AssetRepository
    {
        private DbContext _dbContext;

        public BankMoneyAssetRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override List<Asset> GetAllAssets()
        {
            string query = "SELECT a.*, b.Amount, b.Currency, b.Bank, b.Bill " +
                       "FROM Assets a " +
                       "JOIN BankAssets b ON a.Id = b.AssetId " +
                       "WHERE a.Type = 'Банковский счет'";
            List<Dictionary<string, object>> resulst = _dbContext.ExecuteQuery(query);
            List<Asset> assets = new List<Asset>();

            foreach(var row in resulst)
            {
                var id = (int)row["id"];
                var amount = (decimal)row["amount"];
                var currency = (string)row["currency"];
                var bank = (string)row["bank"];
                var bill = (int)row["bill"];
                BankMoney bankMoney = new BankMoney(amount, currency, bank, bill);
                bankMoney.Id = id;
                assets.Add(bankMoney);
            }
            return assets;
        }

        public override void AddAsset(Asset asset)
        {
            string insertAssetQuery = "INSERT INTO Assets (Type) " +
                                  "VALUES ('Банковский счет') RETURNING Id";
            string insertBankQuery = "INSERT INTO BankAssets (AssetId, Amount, Currency, Bank, Bill, Id) " +
                                     "VALUES (@AssetId, @Amount, @Currency, @Bank, @Bill, @Id)";

            int assetId;
            _dbContext.BeginTransaction();
            try
            {
                assetId = _dbContext.ExecuteScalar<int>(insertAssetQuery);
                ((BankMoney)asset).Id = assetId;
                _dbContext.Execute(insertBankQuery, new Dictionary<string, object>
                {
                    {"id", assetId },
                    {"assetid", assetId },
                    {"amount", ((BankMoney)asset).Amount},
                    {"currency", ((BankMoney)asset).Currency },
                    {"bank", ((BankMoney)asset).Bank},
                    {"bill", ((BankMoney)asset).Bill},
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
            string deleteBankQuery = "DELETE FROM BankAssets WHERE AssetId = @Id";

            _dbContext.BeginTransaction();
            try
            {
                _dbContext.Execute(deleteBankQuery, new Dictionary<string, object> { { "id", id } });
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

            string updateCashQuery = "UPDATE bankAssets SET Amount = @Amount, Currency = @Currency, " +
                                     "Bank = @Bank, Bill = @Bill " +
                                     "WHERE AssetId = @AssetId";

            _dbContext.BeginTransaction();
            try
            {
                _dbContext.Execute(updateAssetQuery, new Dictionary<string, object>
                {
                    { "Type", "Банковский счет"},
                    { "Id" , asset.Id}
                });

                _dbContext.Execute(updateCashQuery, new Dictionary<string, object>
                {
                    { "Amount", ((BankMoney)asset).Amount},
                    { "Currency" , ((BankMoney)asset).Currency},
                    {"Bank", ((BankMoney)asset).Bank },
                    {"Bill", ((BankMoney)asset).Bill },
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
