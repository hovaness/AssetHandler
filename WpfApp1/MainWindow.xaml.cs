using AssetsHandler.Models;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public static ObservableCollection<Asset> assets = new ObservableCollection<Asset>();

        public MainWindow()
        {
            InitializeComponent();
            
            var money1 = new Money(100m, "$");
            var bank = new BankMoney(100m, "$", "Sber", 12);
            var bread = new DiffrentMoney(100m, "$", "хлеб", "ИжГТУ");
            var invent = new Inventory("Бревна", "кг", "$", 120, 120m, 140m);
            assets.Add(money1);
            assets.Add(bank);
            assets.Add(bread);
            assets.Add(invent);
            assetsList.ItemsSource = assets;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //AddWindow add = new AddWindow
            var add = new AddWindow();
            add.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Asset item = (Asset)assetsList.SelectedItem;
            if(item == null)
            {
                return;
            }
            if (MessageBox.Show($"Вы уверены что хотите удалить актив {item.GetName()}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                assets.Remove(item);
            }
            else return;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Asset item = (Asset)assetsList.SelectedItem;
            if(item!= null)
            {
                var change = new ChangeWindow(item);
                change.ShowDialog();
            }
        }
    }
}
