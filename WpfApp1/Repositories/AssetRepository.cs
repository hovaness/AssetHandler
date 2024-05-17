using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Repositories
{
    //Базовый класс репозиторий который содержит в себе поля и методы для всех наследников,
    //реализация CRUD-методов у наследников различается лишь запросами к бд.
    public abstract class AssetRepository
    {
        //контекст базы данных
        protected DbContext _dbContext;
        //получение всех элементов
        public abstract List<Asset> GetAllAssets();
        //добавление элемента
        public abstract void AddAsset(Asset asset);
        //обновление элемента
        public abstract void UpdateAsset(Asset asset);
        //удаление элемента
        public abstract void DeleteAsset(int id);
    }
}
