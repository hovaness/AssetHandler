using AssetsHandler.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Models;
using WpfApp1.Repositories;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        //репозитории всех видов активов
        private MoneyAssetRepository moneyAssetRepository;
        private BankMoneyAssetRepository bankMoneyAssetRepository;
        private DifferentMoneyAssetRepository differentMoneyAssetRepository;
        private InventoryAssetRepository inventoryAssetRepository;
        private RealEstateAssetRepository realEstateAssetRepository;

        //общий лист со всеми активами
        public CollectionViewSource collectionViewSource = new CollectionViewSource();

        //контекст базы данных со строкой подключения
        private DbContext dbContext;

        public MainWindow()
        {
            InitializeComponent();
            dbContext = new DbContext();
            moneyAssetRepository = new MoneyAssetRepository(dbContext);
            bankMoneyAssetRepository = new BankMoneyAssetRepository(dbContext);
            differentMoneyAssetRepository = new DifferentMoneyAssetRepository(dbContext);
            inventoryAssetRepository = new InventoryAssetRepository(dbContext);
            realEstateAssetRepository = new RealEstateAssetRepository(dbContext);
            UpdateAssetList();
        }

        //метод используется для обновления листа с активами, после выполнения каких либо CRUD-операций
        public void UpdateAssetList()
        {
            collectionViewSource.Source = new CompositeCollection()
            {
                new CollectionContainer() { Collection = moneyAssetRepository.GetAllAssets() },
                new CollectionContainer() { Collection = bankMoneyAssetRepository.GetAllAssets() },
                new CollectionContainer() { Collection = differentMoneyAssetRepository.GetAllAssets()},
                new CollectionContainer(){ Collection = inventoryAssetRepository.GetAllAssets()},
                new CollectionContainer(){ Collection= realEstateAssetRepository.GetAllAssets()}
            };
            assetsList.ItemsSource = collectionViewSource.View;
        }

        //открывает окно добавления актива
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var add = new AddWindow();
            Hide();
            add.ShowDialog();
        }


        //метод удаления активов с окном подтверждения
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var item = (Asset)assetsList.SelectedItem;
            if(item == null)
            {
                return;
            }
            if (MessageBox.Show($"Вы уверены что хотите удалить актив {item.GetName()}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (item is BankMoney)
                    bankMoneyAssetRepository.DeleteAsset(item.Id);

                else if (item is DiffrentMoney)
                    differentMoneyAssetRepository.DeleteAsset(item.Id);

                else if (item is Money)
                    moneyAssetRepository.DeleteAsset(item.Id);

                else if (item is Inventory)
                    inventoryAssetRepository.DeleteAsset(item.Id);

                else if (item is RealEstate)
                    realEstateAssetRepository.DeleteAsset(item.Id);
                UpdateAssetList();
            }
            else return;
        }

        //открывает окно редактирования актива
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Asset item = (Asset)assetsList.SelectedItem;
            if(item!= null)
            {
                var change = new ChangeWindow(item);
                Hide();
                change.ShowDialog();
            }
        }

    }
}
