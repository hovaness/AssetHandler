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
        //лист для хранения всех типов активов
        //репозитории
        private MoneyAssetRepository moneyAssetRepository;
        private BankMoneyAssetRepository bankMoneyAssetRepository;
        private DifferentMoneyAssetRepository differentMoneyAssetRepository;

        public CollectionViewSource collectionViewSource = new CollectionViewSource();

        private DbContext dbContext;
        string connectionString = "Server=localhost;Port=5433;Database=test;User Id=postgres;Password=12345;";

        public MainWindow()
        {
            InitializeComponent();
            dbContext = new DbContext(connectionString);
            moneyAssetRepository = new MoneyAssetRepository(dbContext);
            bankMoneyAssetRepository = new BankMoneyAssetRepository(dbContext);
            differentMoneyAssetRepository = new DifferentMoneyAssetRepository(dbContext);
            UpdateAssetList();
        }

        public void UpdateAssetList()
        {
            collectionViewSource.Source = new CompositeCollection()
            {
                new CollectionContainer() { Collection = moneyAssetRepository.GetAllAssets() },
                new CollectionContainer() { Collection = bankMoneyAssetRepository.GetAllAssets() },
                new CollectionContainer() { Collection = differentMoneyAssetRepository.GetAllAssets()},
            };
            assetsList.ItemsSource = collectionViewSource.View;
        }

        //assetsList.ItemsSource = assets;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //AddWindow add = new AddWindow
            var add = new AddWindow();
            Hide();
            add.ShowDialog();
        }

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

                else if(item is DiffrentMoney)
                    differentMoneyAssetRepository.DeleteAsset(item.Id);

                else if(item is Money)
                    moneyAssetRepository.DeleteAsset(item.Id);

                UpdateAssetList();
            }
            else return;
        }

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
